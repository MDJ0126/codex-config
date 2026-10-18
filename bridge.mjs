import fs from 'node:fs';
import path from 'node:path';
import os from 'node:os';
import crypto from 'node:crypto';
import http from 'node:http';
import {spawn} from 'node:child_process';
import {DatabaseSync} from 'node:sqlite';
import {fileURLToPath} from 'node:url';

export function openStore(dir) {
  fs.mkdirSync(dir,{recursive:true});
  const db=new DatabaseSync(path.join(dir,'routing.sqlite'));
  db.exec(`PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;
    CREATE TABLE IF NOT EXISTS routes(message_id TEXT PRIMARY KEY,thread_id TEXT NOT NULL,title TEXT,project TEXT,created INTEGER);
    CREATE TABLE IF NOT EXISTS events(id TEXT PRIMARY KEY,status TEXT NOT NULL,thread_id TEXT,created INTEGER);`);
  return db;
}
export function register(db,r) {
  if(!/^[0-9a-f-]{36}$/i.test(r.threadId)) throw Error('Invalid thread ID');
  for(const id of r.messageIds) db.prepare('INSERT OR REPLACE INTO routes VALUES(?,?,?,?,?)').run(String(id),r.threadId,r.title||'',r.project||'',Date.now());
}
export function resolve(db,event,localThread) {
  if(event.type!=='message'||event.message?.type!=='text') return null;
  const quote=event.message.quotedMessageId;
  // A foreign/unknown quote NEVER falls back to a different local thread.
  const r=quote?db.prepare('SELECT * FROM routes WHERE message_id=?').get(String(quote)):
    db.prepare('SELECT * FROM routes ORDER BY created DESC,rowid DESC LIMIT 1').get();
  return r&&localThread(r.thread_id)?r:null;
}
export function validSignature(body,signature,secret) {
  if(!signature||!secret)return false;
  const expected=crypto.createHmac('sha256',secret).update(body).digest();
  const actual=Buffer.from(signature,'base64');
  return actual.length===expected.length&&crypto.timingSafeEqual(actual,expected);
}
const here=path.dirname(fileURLToPath(import.meta.url));
const home=process.env.CODEX_HOME||path.join(os.homedir(),'.codex');
const state=path.join(home,'line-bridge');
function localThread(id) {
  const d=new DatabaseSync(path.join(home,'state_5.sqlite'),{readOnly:true});
  try {const r=d.prepare('SELECT id,cwd,archived FROM threads WHERE id=?').get(id);return r&&!r.archived&&fs.existsSync(r.cwd)?r:null;}finally{d.close();}
}
function config(){return JSON.parse(fs.readFileSync(path.join(here,'line-notification.private.json'),'utf8').replace(/^\uFEFF/,''));}
function log(event,extra={}){fs.appendFileSync(path.join(state,'events.log'),JSON.stringify({time:new Date().toISOString(),event,...extra})+'\n');}
async function main(){
 const db=openStore(state);
 if(process.argv[2]==='register'){
   let input='';for await(const part of process.stdin)input+=part;
   const r=JSON.parse(input);if(!localThread(r.threadId))throw Error('Thread is not present locally');register(db,r);return;
 }
 const cfg=config();
 const secret=JSON.parse(fs.readFileSync(path.join(state,'secret.json'),'utf8').replace(/^\uFEFF/,'')).channelSecret;
 const running=new Set();
 async function reply(token,text,route){
   const res=await fetch('https://api.line.me/v2/bot/message/reply',{method:'POST',headers:{Authorization:'Bearer '+cfg.ChannelAccessToken,'Content-Type':'application/json'},body:JSON.stringify({replyToken:token,messages:[{type:'text',text}]})});
   if(!res.ok)throw Error('LINE reply HTTP '+res.status);
   const result=await res.json();if(route)register(db,{threadId:route.thread_id,title:route.title,project:route.project,messageIds:(result.sentMessages||[]).map(m=>m.id)});
 }
 async function dispatch(event,route){
   const thread=localThread(route.thread_id);if(!thread)return;
   if(running.has(route.thread_id)){await reply(event.replyToken,'해당 세션의 LINE 요청을 처리 중입니다. 완료 후 다시 보내 주세요.',route);return;}
   running.add(route.thread_id);
   db.prepare('UPDATE events SET status=? WHERE id=?').run('running',event.webhookEventId);
   try{
     await reply(event.replyToken,`📨 요청 접수\n📁 ${route.project||'프로젝트 없는 대화'}\n[${route.title}]\n이 세션에서 요청을 처리하겠습니다.`,route);
     const env={...process.env,LINE_BRIDGE_REQUEST:'1'};
     for(const key of Object.keys(env))if(key.startsWith('CODEX_')&&key!=='CODEX_HOME')delete env[key];
     const prompt='다음은 LINE 서명 검증 및 본인 수신자 확인을 통과한 사용자의 요청입니다. 이 기존 세션에서 처리하십시오. 이번 요청의 LINE 결과 발송은 브리지가 담당하므로 직접 알림을 발송하지 마십시오. 최종 답변은 3200자 이내로 작성하십시오. 요청 본문:\n'+event.message.text;
     const child=spawn(process.env.LINE_CODEX_BIN||'codex',['exec','-s','workspace-write','resume','--json','--skip-git-repo-check',route.thread_id,'-'],{cwd:thread.cwd,env,windowsHide:true,stdio:['pipe','pipe','pipe']});
     child.stdin.end(prompt);
     // Do not persist prompts, model output or credentials in bridge logs.
     let buffer='',answer='';
     child.stdout.on('data',chunk=>{buffer+=chunk.toString();const lines=buffer.split('\n');buffer=lines.pop();for(const line of lines){try{const item=JSON.parse(line);if(item.type==='item.completed'&&item.item?.type==='agent_message')answer=item.item.text;}catch{}}});
     child.stderr.resume();
     const code=await new Promise((resolve,reject)=>{child.once('error',reject);child.once('close',resolve);});
     db.prepare('UPDATE events SET status=? WHERE id=?').run(code===0?'completed':'failed',event.webhookEventId);
     log('dispatch_finished',{threadId:route.thread_id,exitCode:code});
     let summary=code===0?(answer||'요청 처리가 종료되었습니다. 상세 결과는 해당 Codex 세션에서 확인해 주세요.'):'요청을 해당 세션으로 전달했으나 Codex 실행에 실패했습니다. PC에서 세션 상태와 필요한 승인을 확인해 주세요.';
     if(summary.length>3500){let end=summary.lastIndexOf('\n',3300);summary=summary.slice(0,end>0?end:3300)+'\n상세 결과는 Codex 세션에서 확인해 주세요.';}
     const notifier=spawn(process.env.LINE_PWSH_BIN||'pwsh',['-NoProfile','-File',path.join(here,'Send-LineNotification.ps1'),'-SessionId',route.thread_id,'-SessionTitle',route.title,'-ProjectName',route.project||'프로젝트 없는 대화','-Status',code===0?'완료':'조치 필요','-Message',summary],{windowsHide:true,stdio:'ignore'});
     const notified=await new Promise((resolve,reject)=>{notifier.once('error',reject);notifier.once('close',resolve);});
     if(notified!==0)log('result_notification_failed',{threadId:route.thread_id});
   }catch{db.prepare('UPDATE events SET status=? WHERE id=?').run('failed',event.webhookEventId);log('dispatch_failed',{threadId:route.thread_id});}
   finally{running.delete(route.thread_id);}
 }
 const server=http.createServer(async(req,res)=>{
   if(req.method==='GET'&&req.url==='/health'){res.writeHead(200);res.end('ok');return;}
   if(req.method!=='POST'||req.url!=='/webhook'){res.writeHead(404);res.end();return;}
   let body=Buffer.alloc(0);
   for await(const chunk of req){body=Buffer.concat([body,chunk]);if(body.length>1024*1024){res.writeHead(413);res.end();return;}}
   if(!validSignature(body,req.headers['x-line-signature'],secret)){log('invalid_signature');res.writeHead(401);res.end();return;}
   let payload;try{payload=JSON.parse(body);}catch{res.writeHead(400);res.end();return;}
   log('webhook_received',{eventCount:(payload.events||[]).length});
   for(const event of payload.events||[]){
     if(event.source?.type!=='user'||event.source.userId!==cfg.UserId||!event.webhookEventId){log('ignored_sender_or_event');continue;}
     const route=resolve(db,event,localThread);
     if(!route){log('ignored_unknown_or_foreign_session');continue;}
     const inserted=db.prepare('INSERT OR IGNORE INTO events VALUES(?,?,?,?)').run(event.webhookEventId,'queued',route.thread_id,Date.now());
     if(inserted.changes)setImmediate(()=>dispatch(event,route));
   }
   res.writeHead(200);res.end('ok');
 });
 server.listen(17843,'127.0.0.1',()=>log('listening'));
}
if(process.argv[1]&&path.resolve(process.argv[1])===fileURLToPath(import.meta.url))main().catch(()=>{console.error('LINE bridge startup failed; check local configuration.');process.exitCode=1;});
