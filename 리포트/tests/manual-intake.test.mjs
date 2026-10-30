import test from 'node:test';
import assert from 'node:assert/strict';
import {mkdtemp,readFile,writeFile,mkdir,cp,rm,readdir} from 'node:fs/promises';
import os from 'node:os';import path from 'node:path';import {spawnSync} from 'node:child_process';
import {issueBody,validateReport} from '../assets/report-contract.mjs';
test('manual CLI and GitHub intake retain failed QA, immutable versions and reject truncation',async()=>{
 const root=await mkdtemp(path.join(os.tmpdir(),'tracefolio-manual-'));
 try{
  for(const file of ['create-report.mjs','update-manual.mjs','sync-issue.mjs','discussion-data.mjs','layout.mjs','assets/report-contract.mjs','assets/discussion-contract.mjs','assets/manual-contract.mjs']){await mkdir(path.dirname(path.join(root,file)),{recursive:true});await cp(new URL('../'+file,import.meta.url),path.join(root,file));}
  await mkdir(path.join(root,'projects/pixora/posts'),{recursive:true});await writeFile(path.join(root,'projects.json'),JSON.stringify({pixora:{name:'Pixora'}}));
  const payload={schemaVersion:2,project:'pixora',kind:'manual',title:'실제 순차 계획',summary:'격리 접수 검사',outcome:'계획 생성',manualId:'intake-test',manualState:'planned',phase:'initial',round:1,goal:'목표',completionCriteria:'실제 전체 QA',tasks:[{id:'T01',title:'작업',action:'실행',expected:'기대값',qa:'실제 비교',revision:1,status:'pending',dependsOn:[]}],qaRuns:[]};
  const command=async(script,args=[])=>{await writeFile(path.join(root,'input.json'),JSON.stringify(payload));return spawnSync(process.execPath,[path.join(root,script),...args],{encoding:'utf8',windowsHide:true});};
  let result=await command('create-report.mjs',['--input',path.join(root,'input.json')]);assert.equal(result.status,0,result.stderr);const first='projects/pixora/posts/'+(await readdir(path.join(root,'projects/pixora/posts')))[0],original=await readFile(path.join(root,first),'utf8');
  payload.manualState='qa';payload.tasks[0].status='qa';payload.qaRuns=[{id:'Q01',kind:'step',at:'2026-10-30T09:30:00Z',summary:'실제 실패',results:[{taskId:'T01',revision:1,verdict:'fail',actual:'불일치',evidence:'fixture execution',dependencies:{}}]}];
  result=await command('update-manual.mjs',['--from',first,'--input',path.join(root,'input.json')]);assert.equal(result.status,0,result.stderr);assert.equal(await readFile(path.join(root,first),'utf8'),original);assert.equal((await readdir(path.join(root,'projects/pixora/posts'))).length,2);
  result=await command('update-manual.mjs',['--from',first,'--input',path.join(root,'input.json')]);assert.notEqual(result.status,0);assert.match(result.stderr,/최신/);
  payload.manualId='github-test';payload.manualState='planned';payload.tasks[0].status='pending';payload.qaRuns=[];
  const eventFile=path.join(root,'event.json');const event=async(n)=>{await writeFile(eventFile,JSON.stringify({action:n===1?'opened':'edited',issue:{number:991,updated_at:`2026-10-30T09:4${n}:00Z`,body:issueBody(validateReport(payload,{pixora:{}}))},sender:{login:'fixture'}}));return spawnSync(process.execPath,[path.join(root,'sync-issue.mjs')],{encoding:'utf8',windowsHide:true,env:{...process.env,GITHUB_EVENT_PATH:eventFile,GITHUB_RUN_NUMBER:String(n)}});};
  result=await event(1);assert.equal(result.status,0,result.stderr);payload.manualState='active';payload.tasks[0].status='doing';result=await event(2);assert.equal(result.status,0,result.stderr);const record=JSON.parse(await readFile(path.join(root,'projects/pixora/assets/issues/TF-991.json'),'utf8'));assert.equal(record.history.length,2);assert.match(record.report.previousManual,/TF-991-v1.0.1.html$/);
 }finally{if(!root.startsWith(path.join(os.tmpdir(),'tracefolio-manual-')))throw Error('Invalid cleanup');await rm(root,{recursive:true,force:true});}
});

