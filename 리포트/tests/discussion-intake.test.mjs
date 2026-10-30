import test from 'node:test';
import assert from 'node:assert/strict';
import {mkdtemp,readFile,writeFile,mkdir,cp,rm,readdir} from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import {spawnSync} from 'node:child_process';
import {issueBody,validateReport} from '../assets/report-contract.mjs';
test('GitHub discussion intake preserves edits, decisions and both Markdown revisions',async()=>{
 const root=await mkdtemp(path.join(os.tmpdir(),'tracefolio-discussion-'));
 try{
  for(const file of ['sync-issue.mjs','discussion-data.mjs','layout.mjs','assets/report-contract.mjs','assets/discussion-contract.mjs']){await mkdir(path.dirname(path.join(root,file)),{recursive:true});await cp(new URL('../'+file,import.meta.url),path.join(root,file));}
  await writeFile(path.join(root,'projects.json'),JSON.stringify({pixora:{name:'Pixora'}}));await mkdir(path.join(root,'projects/pixora/posts'),{recursive:true});await writeFile(path.join(root,'projects/pixora/posts/qa.md'),'---\ntitle: QA\nproject: pixora\ntype: 작업 결과\ndate: 2026-10-29T00:00:00Z\n---\n');
  const payload={schemaVersion:2,project:'pixora',kind:'discussion',title:'QA 후속 논의',summary:'격리 접수 검사',outcome:'관찰한 차이를 논의합니다.',topic:'qa',discussionState:'open',relatedReport:'projects/pixora/reports/qa.html'};
  const run=async(action,at,revision)=>{const event={action,issue:{number:424242,body:issueBody(validateReport(payload,{pixora:{}})),updated_at:at,user:{login:'fixture'}},sender:{login:'fixture'}};const file=path.join(root,'event.json');await writeFile(file,JSON.stringify(event));return spawnSync(process.execPath,[path.join(root,'sync-issue.mjs')],{env:{...process.env,GITHUB_EVENT_PATH:file,GITHUB_RUN_NUMBER:String(revision)},encoding:'utf8',windowsHide:true});};
  let result=await run('opened','2026-10-30T00:00:00Z',1);assert.equal(result.status,0,result.stderr);
  payload.discussionState='accepted';payload.decision='QA 근거를 검토해 채택, 다음 수정에서 구현합니다.';result=await run('edited','2026-10-30T00:01:00Z',2);assert.equal(result.status,0,result.stderr);
  const record=JSON.parse(await readFile(path.join(root,'projects/pixora/assets/issues/TF-424242.json'),'utf8'));assert.equal(record.history.length,2);assert.equal(record.history[1].stage,'accepted');assert.equal(record.report.discussionState,'accepted');assert.equal((await readdir(path.join(root,'projects/pixora/posts'))).length,3);
  result=await run('edited','2026-10-30T00:01:00Z',3);assert.equal(result.status,0);assert.match(result.stdout,/stale/);
  payload.relatedReport='projects/pixora/reports/missing.html';result=await run('edited','2026-10-30T00:02:00Z',4);assert.notEqual(result.status,0);assert.equal((await readdir(path.join(root,'projects/pixora/posts'))).length,3);
 }finally{if(!root.startsWith(path.join(os.tmpdir(),'tracefolio-discussion-')))throw Error('Invalid test cleanup');await rm(root,{recursive:true,force:true});}
});
