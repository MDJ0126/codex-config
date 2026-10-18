import {createHmac,timingSafeEqual} from 'node:crypto';
const json=(data,status=200)=>Response.json(data,{status});
const same=(a,b)=>{const x=Buffer.from(a||''),y=Buffer.from(b||'');return x.length===y.length&&timingSafeEqual(x,y);};
async function rawInput(req){
 if(!req.body)return Buffer.alloc(0);const reader=req.body.getReader();const chunks=[];let size=0;
 try{while(true){const{value,done}=await reader.read();if(done)break;size+=value.length;if(size>65536){await reader.cancel();throw Error('INPUT_TOO_LARGE');}chunks.push(Buffer.from(value));}}finally{reader.releaseLock();}
 return Buffer.concat(chunks);
}
async function input(req){return JSON.parse((await rawInput(req)).toString('utf8'));}
async function line(env,method,body){
 const res=await fetch('https://api.line.me/v2/bot/message/'+method,{method:'POST',headers:{Authorization:'Bearer '+env.LINE_TOKEN,'Content-Type':'application/json'},body:JSON.stringify(body)});
 if(!res.ok)throw Error('LINE_HTTP_'+res.status);return res.json();
}
export async function acceptEvent(db,event,now){
 const quote=event.message.quotedMessageId;
 const route=quote?await db.prepare('SELECT * FROM routes WHERE message_id=?').bind(String(quote)).first():await db.prepare('SELECT * FROM routes ORDER BY seq DESC LIMIT 1').first();
 if(!route)return {unknown:true};
 const r=await db.prepare('INSERT OR IGNORE INTO jobs(id,device_id,thread_id,title,project,prompt,status,created) VALUES(?,?,?,?,?,?,?,?)').bind(event.webhookEventId,route.device_id,route.thread_id,route.title,route.project,event.message.text,'pending',now).run();
 if(!r.meta.changes)return {duplicate:true};
 const device=await db.prepare('SELECT * FROM devices WHERE id=?').bind(route.device_id).first();
 return {route,deviceName:device?.name||'대상 PC',offline:!device||now-device.last_seen>90000};
}
export async function poll(db,body,now){
 await db.prepare('INSERT INTO devices VALUES(?,?,?) ON CONFLICT(id) DO UPDATE SET name=excluded.name,last_seen=excluded.last_seen').bind(body.deviceId,body.deviceName||body.deviceId,now).run();
 const lease=crypto.randomUUID();
 const job=await db.prepare("UPDATE jobs SET status='leased',lease_token=?,lease_until=? WHERE id=(SELECT id FROM jobs WHERE device_id=? AND (status='pending' OR (status='leased' AND lease_until<?)) ORDER BY created,id LIMIT 1) RETURNING *").bind(lease,now+120000,body.deviceId,now).first();
 return {job};
}
export default {async fetch(req,env,ctx){
 try{
  const url=new URL(req.url);const db=env.DB;
  if(req.method==='GET'&&url.pathname==='/health')return json({ok:true,mode:'central'});
  if(req.method!=='POST')return json({error:'not_found'},404);
  if(Number(req.headers.get('content-length'))>65536)return json({error:'too_large'},413);
  if(url.pathname==='/webhook'){
   const raw=await rawInput(req);
   const signature=createHmac('sha256',env.LINE_SECRET).update(raw).digest('base64');
   if(!same(signature,req.headers.get('x-line-signature')))return json({error:'signature'},401);
   const payload=JSON.parse(raw.toString('utf8'));
   for(const event of payload.events||[]){
    if(event.type!=='message'||event.message?.type!=='text'||event.source?.type!=='user'||event.source.userId!==env.LINE_USER_ID||!event.webhookEventId)continue;
    const accepted=await acceptEvent(db,event,Date.now());if(accepted.duplicate)continue;
    const text=accepted.unknown?'알림의 PC·세션 정보를 찾을 수 없어 요청을 전달하지 않았습니다. 새 알림에 답장해 주세요.':`${accepted.offline?'💤 PC 오프라인으로 대기 중':'📨 요청 접수'}\n🖥 ${accepted.deviceName}\n📁 ${accepted.route.project||'프로젝트 없는 대화'}\n[${accepted.route.title}]\n${accepted.offline?'대상 PC가 다시 연결되면 이 세션으로만 전달합니다.':'대상 PC·세션으로 전달하겠습니다.'}`;
    if(event.replyToken)ctx.waitUntil(line(env,'reply',{replyToken:event.replyToken,messages:[{type:'text',text}]}).catch(()=>console.log(JSON.stringify({event:'reply_failed'}))));
   }
   return json({ok:true});
  }
  if(!same(req.headers.get('authorization'),'Bearer '+env.BRIDGE_KEY))return json({error:'unauthorized'},401);
  const body=await input(req);const now=Date.now();
  if(!/^[0-9a-f-]{36}$/i.test(body.deviceId||''))return json({error:'device'},400);
  if(url.pathname==='/notify'){
   if(!/^[0-9a-f-]{36}$/i.test(body.threadId||'')||typeof body.text!=='string'||body.text.length>4500)return json({error:'notification'},400);
   const quotaRes=await fetch('https://api.line.me/v2/bot/message/quota',{headers:{Authorization:'Bearer '+env.LINE_TOKEN}});
   const usedRes=await fetch('https://api.line.me/v2/bot/message/quota/consumption',{headers:{Authorization:'Bearer '+env.LINE_TOKEN}});
   if(!quotaRes.ok||!usedRes.ok)throw Error('LINE_QUOTA_FAILED');const quota=await quotaRes.json(),used=await usedRes.json();
   if(quota.type==='limited'&&used.totalUsage>=quota.value)return json({error:'quota'},429);
   const result=await line(env,'push',{to:env.LINE_USER_ID,messages:[{type:'text',text:body.text}]});
   const statements=(result.sentMessages||[]).map(m=>db.prepare('INSERT INTO routes(message_id,device_id,thread_id,title,project,created) VALUES(?,?,?,?,?,?)').bind(String(m.id),body.deviceId,body.threadId,body.title||'',body.project||'',now));
   if(statements.length)await db.batch(statements);
   return json(result);
  }
  if(url.pathname==='/poll')return json(await poll(db,body,now));
  if(url.pathname==='/ack'){
   if(!['forwarded','unknown','missing_session'].includes(body.status))return json({error:'status'},400);
   const r=await db.prepare('UPDATE jobs SET status=?,prompt=?,lease_until=NULL WHERE id=? AND device_id=? AND lease_token=? AND status=\'leased\'').bind(body.status,'',body.id,body.deviceId,body.leaseToken).run();
   return json({ok:r.meta.changes===1});
  }
  return json({error:'not_found'},404);
 }catch{console.log(JSON.stringify({event:'central_request_failed'}));return json({error:'request_failed'},500);}
}};
