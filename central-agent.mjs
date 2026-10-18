import fs from'node:fs';import path from'node:path';import os from'node:os';import{fileURLToPath}from'node:url';import{deviceId}from'./device-id.mjs';import{openStore,localThread}from'./bridge.mjs';import{appTool,appRequest}from'./app-client.mjs';
const here=path.dirname(fileURLToPath(import.meta.url));const home=process.env.CODEX_HOME||path.join(os.homedir(),'.codex');const state=path.join(home,'line-bridge');
const cfg=JSON.parse(fs.readFileSync(path.join(here,'line-notification.private.json'),'utf8').replace(/^\uFEFF/,''));
const db=openStore(state),device=deviceId();
function log(event,extra={}){fs.appendFileSync(path.join(state,'events.log'),JSON.stringify({time:new Date().toISOString(),event,...extra})+'\n');}
async function api(route,data){const r=await fetch(cfg.CentralUrl+route,{method:'POST',headers:{Authorization:'Bearer '+cfg.BridgeKey,'Content-Type':'application/json'},body:JSON.stringify({deviceId:device,...data}),signal:AbortSignal.timeout(25000)});if(!r.ok)throw Error('CENTRAL_HTTP_'+r.status);return r.json();}
async function tick(){
 const contextFile=path.join(state,'app-context.json');if(!fs.existsSync(contextFile))return;
 const context=JSON.parse(fs.readFileSync(contextFile,'utf8').replace(/^\uFEFF/,''));
 // A running background process alone does not mean Codex is available.
 await appRequest(context.pipePath,'tools/list',{threadStartKind:'all'});
 const {job}=await api('/poll',{deviceName:os.hostname()});if(!job)return;
 const ack=status=>api('/ack',{id:job.id,leaseToken:job.lease_token,status});
 if(!localThread(job.thread_id)){await ack('missing_session');log('central_missing_local_session',{threadId:job.thread_id});return;}
 const old=db.prepare('SELECT status FROM events WHERE id=?').get(job.id);
 if(old){await ack(old.status==='forwarded'?'forwarded':'unknown');return;}
 // Persist before forwarding. Ambiguous failures are never automatically replayed.
 db.prepare('INSERT INTO events VALUES(?,?,?,?)').run(job.id,'dispatching',job.thread_id,Date.now());
 try{
  await appTool(context.pipePath,context.callerThreadId,'send_message_to_thread',{threadId:job.thread_id,hostId:'local',prompt:'다음은 중앙 LINE 서버가 서명과 본인 User ID를 검증한 사용자 요청입니다. 사용자는 이 PC·세션으로의 전달을 승인했습니다. 요청을 처리하고 전역 지침대로 질문을 포함한 LINE 결과 알림을 한 번 보내십시오. 사용자 요청:\n\n'+job.prompt});
  db.prepare('UPDATE events SET status=? WHERE id=?').run('forwarded',job.id);await ack('forwarded');log('central_forwarded',{threadId:job.thread_id});
 }catch{db.prepare('UPDATE events SET status=? WHERE id=?').run('unknown',job.id);await ack('unknown');log('central_delivery_unknown',{threadId:job.thread_id});}
}
log('central_agent_started');
while(true){try{await tick();}catch{log('central_poll_failed');}await new Promise(r=>setTimeout(r,30000));}
