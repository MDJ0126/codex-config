import{test}from'node:test';import assert from'node:assert/strict';import{DatabaseSync}from'node:sqlite';import fs from'node:fs';import{createHmac}from'node:crypto';import worker,{acceptEvent,poll}from'./worker.mjs';
function store(){const d=new DatabaseSync(':memory:');d.exec(fs.readFileSync(new URL('./schema.sql',import.meta.url),'utf8'));return{prepare(sql){const statement=(values=[])=>({bind(...v){return statement(v)},async first(){return d.prepare(sql).get(...values)||null},async run(){return{meta:d.prepare(sql).run(...values)}}});return statement();},d};}
test('quotes stay with original PC; plain targets global latest; offline persists and only target PC receives',async()=>{
 const db=store();db.d.prepare('INSERT INTO routes(message_id,device_id,thread_id,title,project,created) VALUES(?,?,?,?,?,?)').run('a','PC-A','session-A','A','project',1);db.d.prepare('INSERT INTO routes(message_id,device_id,thread_id,title,project,created) VALUES(?,?,?,?,?,?)').run('b','PC-B','session-B','B','project',2);
 let e={webhookEventId:'event-1',message:{text:'request',quotedMessageId:'a'}};let r=await acceptEvent(db,e,1000000);assert.equal(r.route.device_id,'PC-A');assert.equal(r.offline,true);
 assert.equal((await poll(db,{deviceId:'PC-B'},1000001)).job,null);
 let job=(await poll(db,{deviceId:'PC-A'},1000002)).job;assert.equal(job.thread_id,'session-A');assert.equal(job.device_id,'PC-A');
 assert.equal((await acceptEvent(db,e,1000003)).duplicate,true);
 r=await acceptEvent(db,{webhookEventId:'event-2',message:{text:'plain'}},1000004);assert.equal(r.route.device_id,'PC-B');
 assert.equal((await acceptEvent(db,{webhookEventId:'event-3',message:{text:'unknown',quotedMessageId:'missing'}},1000005)).unknown,true);
 assert.equal(db.d.prepare('select count(*) n from jobs').get().n,2);
 db.d.close();
});
test('HTTP webhook: offline acknowledgement, reconnection after days, wrong-PC ack rejection and deduplication',async()=>{
 const db=store(),a='11111111-1111-1111-1111-111111111111',b='22222222-2222-2222-2222-222222222222',thread='33333333-3333-3333-3333-333333333333';
 db.d.prepare('INSERT INTO routes(message_id,device_id,thread_id,title,project,created) VALUES(?,?,?,?,?,?)').run('original-A',a,thread,'session','project',1);
 const env={DB:db,BRIDGE_KEY:'test-key',LINE_SECRET:'test-secret',LINE_TOKEN:'fake',LINE_USER_ID:'test-user'};const tasks=[],replies=[];const ctx={waitUntil(p){tasks.push(p)}};const savedFetch=globalThis.fetch;
 globalThis.fetch=async(url,options)=>{assert.equal(url,'https://api.line.me/v2/bot/message/reply');replies.push(JSON.parse(options.body));return Response.json({sentMessages:[]});};
 const call=(route,body,key='test-key')=>worker.fetch(new Request('https://central.test'+route,{method:'POST',headers:{authorization:'Bearer '+key},body:JSON.stringify(body)}),env,ctx);
 try{
  const payload=JSON.stringify({events:[{type:'message',webhookEventId:'http-event',replyToken:'test-reply',source:{type:'user',userId:'test-user'},message:{type:'text',text:'preserve this request',quotedMessageId:'original-A'}}]});
  const headers={'x-line-signature':createHmac('sha256','test-secret').update(payload).digest('base64')};
  assert.equal((await worker.fetch(new Request('https://central.test/webhook',{method:'POST',body:payload,headers}),env,ctx)).status,200);await Promise.all(tasks);
  assert.match(replies[0].messages[0].text,/PC 오프라인으로 대기 중/);
  await worker.fetch(new Request('https://central.test/webhook',{method:'POST',body:payload,headers}),env,ctx);assert.equal(db.d.prepare('SELECT count(*) n FROM jobs').get().n,1);
  assert.equal((await (await call('/poll',{deviceId:b})).json()).job,null);
  const leased=await poll(db,{deviceId:a},Date.now()+7*86400000);assert.equal(leased.job.prompt,'preserve this request');assert.equal(leased.job.device_id,a);
  assert.equal((await (await call('/ack',{deviceId:b,id:'http-event',leaseToken:leased.job.lease_token,status:'forwarded'})).json()).ok,false);
  assert.equal((await (await call('/ack',{deviceId:a,id:'http-event',leaseToken:leased.job.lease_token,status:'forwarded'})).json()).ok,true);
  assert.equal(db.d.prepare('SELECT prompt FROM jobs').get().prompt,'');
  assert.equal((await call('/poll',{deviceId:a},'wrong-key')).status,401);
  assert.equal((await worker.fetch(new Request('https://central.test/webhook',{method:'POST',body:payload}),env,ctx)).status,401);
 }finally{globalThis.fetch=savedFetch;db.d.close();}
});
