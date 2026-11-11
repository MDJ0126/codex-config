import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import {spawn} from 'node:child_process';
import readline from 'node:readline';
import http from 'node:http';
import {fileURLToPath} from 'node:url';
import {CompanionWorld,startServer} from './server.mjs';
import {LocalSpeech,inspectWave} from './speech.mjs';

function temporary() {
  const directory=fs.mkdtempSync(path.join(os.tmpdir(),'fantasya-bridge-'));
  return {directory,clean(){const resolved=path.resolve(directory);assert.ok(resolved.startsWith(path.resolve(os.tmpdir())+path.sep+'fantasya-bridge-'));fs.rmSync(resolved,{recursive:true,force:true});}};
}
function snapshot(world) {
  world.sync({sessionId:'actual-test-session',companions:[{id:'luna',name:'루나',personality:'신중한 정찰 동료',alive:true,health:100,action:'follow',targets:[{id:17,health:20,distance:2}]}]});
}

test('동료별 기억·약속이 재시작 후에도 분리되어 유지되고 인용문은 명령으로 실행되지 않는다',async()=>{
  const temp=temporary();
  try {
    const world=new CompanionWorld(temp.directory);
    await world.call('remember_companion',{companionId:'luna',text:'플레이어는 방패 역할을 좋아한다.'},'player');
    await world.call('talk_to_companion',{companionId:'luna',text:'내가 신호할 때까지 기다려'},'player');
    const reloaded=new CompanionWorld(temp.directory);
    assert.equal(reloaded.observe('luna').promise.mode,'hold');
    assert.equal(reloaded.observe('luna').memories.length,1);
    assert.equal(reloaded.observe('bram').memories.length,0);
    assert.equal(reloaded.parseInstruction('어제 "기다려"라고 했었지?'),null);
    assert.equal(reloaded.parseInstruction('따라와').mode,'follow');
    assert.equal(reloaded.parseInstruction('루나, 내가 신호할 때까지 기다려주세요.').mode,'hold');
  } finally {temp.clean();}
});

test('제어권 독점·약속 우선·현재 대상 검증·게임 오프라인 거부가 함께 적용된다',async()=>{
  const temp=temporary();
  try {
    const world=new CompanionWorld(temp.directory);snapshot(world);
    await world.call('claim_companion',{companionId:'luna',agentName:'Codex',seconds:30},'A');
    await assert.rejects(world.call('claim_companion',{companionId:'luna',agentName:'Claude'},'B'),/Already controlled/);
    await world.call('set_promise',{companionId:'luna',mode:'hold',allowAttack:false,text:'신호까지 기다린다.'},'player');
    await assert.rejects(world.call('act_companion',{companionId:'luna',action:'attack',targetId:17,reason:'적 공격'},'A'),/promise takes priority/);
    await world.call('set_promise',{companionId:'luna',mode:'auto',allowAttack:true,text:'자율 행동'},'player');
    await assert.rejects(world.call('act_companion',{companionId:'luna',action:'attack',targetId:99,reason:'없는 적'},'A'),/not currently permitted/);
    assert.equal((await world.call('act_companion',{companionId:'luna',action:'attack',targetId:17,reason:'관측한 적만 공격'},'A')).queued,true);
    await assert.rejects(world.call('act_companion',{companionId:'luna',action:'hold',reason:'중복 요청'},'A'),/queue is full/);
    world.games.get('luna').receivedAt-=4000;
    await assert.rejects(world.call('act_companion',{companionId:'luna',action:'hold',reason:'오프라인 요청'},'A'),/fresh live game/);
    world.leases.get('luna').expiresAt=Date.now()-1;
    assert.equal(world.lease('luna'),null);
  } finally {temp.clean();}
});

test('외부 에이전트가 게임 대화를 직접 받고 응답한 메시지만 확인 처리한다',async()=>{
  const temp=temporary();
  try {
    const world=new CompanionWorld(temp.directory);snapshot(world);
    world.model.endpoint='http://127.0.0.1:1/never-call-model';
    await world.call('claim_companion',{companionId:'luna',agentName:'Codex',seconds:30},'A');
    const first=await world.call('talk_to_companion',{companionId:'luna',text:'지금 뭘 보고 있어?'},'game-player');
    assert.equal(first.source,'external-agent');
    const inbox=await world.call('wait_for_companion_events',{companionId:'luna',seconds:1},'A');
    assert.equal(inbox.pendingPlayerMessages.length,1);
    const second=await world.call('talk_to_companion',{companionId:'luna',text:'내 역할도 기억하니?'},'game-player');
    await world.call('say_as_companion',{companionId:'luna',text:'주변 상황을 확인하고 있어요.',replyToMessageId:first.messageId},'A');
    assert.deepEqual(world.observe('luna').pendingPlayerMessages.map(item=>item.id),[second.messageId]);
    assert.equal(world.sync({sessionId:'actual-test-session',companions:[{id:'luna',name:'루나'}]}).companions[0].awaitingExternalReply,true);
    await world.call('release_companion',{companionId:'luna'},'A');
    assert.equal(world.observe('luna').pendingPlayerMessages.length,0);
    assert.equal(world.observe('luna').messages.at(-1).source,'connection');
  } finally {temp.clean();}
});

test('소형 모델의 잘못된 응답 형식은 한 번만 교정하고 검증된 답변만 저장한다',async()=>{
  const temp=temporary();let requests=0;
  const model=http.createServer((request,response)=>{
    request.resume();request.on('end',()=>{
      requests++;response.setHeader('Content-Type','application/json');
      response.end(JSON.stringify({choices:[{message:{content:JSON.stringify(requests===1?{wrong:'잘못된 형식'}:{reply:'방패 역할을 좋아한다고 기억해요.'})}}]}));
    });
  });
  await new Promise(resolve=>model.listen(0,'127.0.0.1',resolve));
  try {
    const world=new CompanionWorld(temp.directory);world.model.endpoint=`http://127.0.0.1:${model.address().port}/v1/chat/completions`;
    const answer=await world.call('talk_to_companion',{companionId:'luna',text:'내 역할이 뭐였지?'},'player');
    assert.equal(requests,2);assert.equal(answer.reply,'방패 역할을 좋아한다고 기억해요.');
    assert.equal(world.observe('luna').messages.filter(item=>item.role==='companion').length,1);
  } finally {await new Promise(resolve=>model.close(resolve));temp.clean();}
});

test('음성 입력은 정해진 WAV만 받으며 무음·미설치 상태를 구분하고 대화는 만들지 않는다',async()=>{
  const temp=temporary();
  try {
    const speech=new LocalSpeech(temp.directory);
    assert.equal((await speech.status()).ready,false);
    assert.throws(()=>inspectWave(Buffer.from('not audio')),/WAV/);
    const wave=Buffer.alloc(44+16000*2);
    wave.write('RIFF');wave.writeUInt32LE(wave.length-8,4);wave.write('WAVEfmt ',8);wave.writeUInt32LE(16,16);wave.writeUInt16LE(1,20);wave.writeUInt16LE(1,22);wave.writeUInt32LE(16000,24);wave.writeUInt32LE(32000,28);wave.writeUInt16LE(2,32);wave.writeUInt16LE(16,34);wave.write('data',36);wave.writeUInt32LE(wave.length-44,40);
    assert.equal(inspectWave(wave).seconds,1);
    await assert.rejects(speech.transcribe(wave),/목소리가/);
    assert.equal(speech.busy,false);
    assert.equal(fs.existsSync(path.join(temp.directory,'companions.json')),false);
  } finally {temp.clean();}
});

test('예약 키 입력은 상태를 변경하지 않고 거부되며 인증 없는 로컬 HTTP도 거부된다',async()=>{
  const temp=temporary();
  const {server,world,manifest}=await startServer({directory:temp.directory,port:0});
  try {
    const before=fs.readFileSync(world.file,'utf8');
    await assert.rejects(world.call('remember_companion',JSON.parse('{"companionId":"luna","text":"hello","__proto__":{"polluted":true}}'),'bad'),/Reserved property/);
    assert.equal(fs.readFileSync(world.file,'utf8'),before);
    assert.equal({}.polluted,undefined);
    const unauthorized=await fetch(manifest.url+'/health');assert.equal(unauthorized.status,403);
    const origin=await fetch(manifest.url+'/health',{headers:{Authorization:'Bearer '+manifest.token,Origin:'http://example.invalid'}});assert.equal(origin.status,403);
  } finally {await new Promise(resolve=>server.close(resolve));temp.clean();}
});

function mcpClient(directory) {
  const child=spawn(process.execPath,[fileURLToPath(new URL('mcp.mjs',import.meta.url))],{windowsHide:true,env:{...process.env,FANTASYA_AGENT_DATA_DIR:directory},stdio:['pipe','pipe','pipe']});
  const pending=new Map();let counter=0;
  readline.createInterface({input:child.stdout}).on('line',line=>{const message=JSON.parse(line);pending.get(message.id)?.(message);pending.delete(message.id);});
  return {child,request(method,params={}){const id=++counter;return new Promise((resolve,reject)=>{const timeout=setTimeout(()=>{pending.delete(id);reject(new Error('MCP timeout'));},10000);pending.set(id,value=>{clearTimeout(timeout);resolve(value);});child.stdin.write(JSON.stringify({jsonrpc:'2.0',id,method,params})+'\n');});}};
}
test('실제 stdio MCP 프로세스 두 개가 동일 기억을 읽고 제어권 충돌을 처리한다',async()=>{
  const temp=temporary();const {server}=await startServer({directory:temp.directory,port:0});
  const first=mcpClient(temp.directory),second=mcpClient(temp.directory);
  try {
    for(const client of [first,second]) {
      const initialized=await client.request('initialize',{protocolVersion:'2025-06-18',capabilities:{},clientInfo:{name:'integration-client',version:'1'}});
      assert.equal(initialized.result.serverInfo.name,'fantasya-companions');
      const listing=await client.request('tools/list');assert.ok(listing.result.tools.some(tool=>tool.name==='talk_to_companion'));
    }
    const firstClaim=await first.request('tools/call',{name:'claim_companion',arguments:{companionId:'luna',agentName:'Codex'}});assert.equal(firstClaim.result.isError,false);
    const secondClaim=await second.request('tools/call',{name:'claim_companion',arguments:{companionId:'luna',agentName:'Cursor'}});assert.equal(secondClaim.result.isError,true);
    const memory=await first.request('tools/call',{name:'remember_companion',arguments:{companionId:'luna',text:'다음 모험에서 방패 역할을 논의했다.'}});assert.equal(memory.result.isError,false);
    const observed=await second.request('tools/call',{name:'observe_companion',arguments:{companionId:'luna'}});
    assert.equal(JSON.parse(observed.result.content[0].text).memories.length,1);
  } finally {first.child.kill();second.child.kill();await new Promise(resolve=>server.close(resolve));temp.clean();}
});
