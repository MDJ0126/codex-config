import test from 'node:test';
import assert from 'node:assert/strict';
import {validateReport,toMarkdown} from '../assets/report-contract.mjs';
import {validateManualLinks,latestTaskQA} from '../assets/manual-contract.mjs';
import {validatePost} from '../standards.mjs';
const projects={'codex-config':{name:'Codex'}};
const task=(n,status='pending')=>({id:`T${String(n).padStart(2,'0')}`,title:'작업'+n,action:'구현할 내용',expected:'확인할 완료 기준',qa:'실제 실행·기대값 비교',dependsOn:n===1?[]:[`T${String(n-1).padStart(2,'0')}`],status,revision:1,result:status==='done'?'구현 결과':''});
const base={schemaVersion:2,project:'codex-config',kind:'manual',title:'작업 메뉴얼',summary:'순차 실행과 반복 QA',outcome:'계획을 작성했습니다.',manualId:'test-plan',goal:'요청한 기능 구현',completionCriteria:'전체 작업과 최종 QA 통과',phase:'initial',round:1,manualState:'planned',tasks:[task(1),task(2)],qaRuns:[]};
const pass=(n,revision=1)=>({taskId:`T${String(n).padStart(2,'0')}`,revision,verdict:'pass',actual:'실제 기대값 일치',evidence:'실행 결과 파일',dependencies:n===1?{}:{[`T${String(n-1).padStart(2,'0')}`]:1}});
const run=(results,kind='step')=>({id:'Q01',kind,at:'2026-10-30T09:30:00Z',summary:'실제 검사 기록',results});
test('initial cap is30; followup focus cap is5 while original backlog stays intact',()=>{
 assert.equal(validateReport({...base,tasks:Array.from({length:30},(_,i)=>task(i+1))},projects).tasks.length,30);
 assert.throws(()=>validateReport({...base,tasks:Array.from({length:31},(_,i)=>task(i+1))},projects));
 const next={...base,phase:'followup',round:2,previousManual:'projects/codex-config/reports/prev.html',tasks:Array.from({length:30},(_,i)=>task(i+1)),focusTasks:['T01','T02','T03','T04','T05']};assert.equal(validateReport(next,projects).tasks.length,30);assert.throws(()=>validateReport({...next,focusTasks:[...next.focusTasks,'T06']},projects));
});
test('later tasks cannot run ahead and done requires current QA evidence',()=>{
 assert.throws(()=>validateReport({...base,manualState:'active',tasks:[task(1),task(2,'doing')]},projects));
 assert.throws(()=>validateReport({...base,manualState:'active',tasks:[task(1,'done'),task(2)]},projects));
 assert.throws(()=>validateReport({...base,manualState:'active',tasks:[task(1,'done'),task(2)],qaRuns:[run([{...pass(1),evidence:''}])]},projects));
 assert.equal(validateReport({...base,manualState:'active',tasks:[task(1,'done'),task(2,'doing')],qaRuns:[run([pass(1)])]},projects).tasks[1].status,'doing');
});
test('completed manual requires full final QA, not just step passes or partial final',()=>{
 const ready={...base,manualState:'complete',tasks:[task(1,'done'),task(2,'done')]};assert.throws(()=>validateReport({...ready,qaRuns:[run([pass(1),pass(2)])]},projects));assert.throws(()=>validateReport({...ready,qaRuns:[run([pass(1)],'final')]},projects));
 const r=validateReport({...ready,qaRuns:[run([pass(1),pass(2)],'final')]},projects);assert.equal(r.state,'complete');const md=toMarkdown(r,{date:'2026-10-30T09:30:00Z'}),[,header,body]=/^---\n([\s\S]*?)\n---\n([\s\S]*)$/.exec(md),meta=Object.fromEntries(header.trim().split('\n').map(l=>{const i=l.indexOf(':');return[l.slice(0,i),l.slice(i+1).trim()];}));validatePost(meta,body,'manual.md');
});
test('task edits and changed dependencies invalidate old QA',()=>{
 const r=validateReport({...base,manualState:'active',tasks:[{...task(1,'doing'),revision:2},task(2)],qaRuns:[run([pass(1),pass(2)])]},projects);assert.equal(latestTaskQA(r,r.tasks[0]),undefined);assert.equal(latestTaskQA(r,r.tasks[1]).verdict,'stale');
 assert.throws(()=>validateReport({...r,tasks:[{...task(1,'done'),revision:2},task(2)]},projects));
});
test('followup cannot drop remaining tasks or erase QA failures',()=>{
 const old=validateReport({...base,manualState:'active',tasks:[task(1,'qa'),task(2)],qaRuns:[run([{...pass(1),verdict:'fail',actual:'실제 불일치'}])]},projects),previous='projects/codex-config/reports/prev.html',catalog=[{project:'codex-config',kind:'manual',manual_id:old.manualId,round:1,reportPath:previous,manual:old}];
 const next=validateReport({...old,phase:'followup',round:2,focusTasks:['T01'],previousManual:previous},projects);validateManualLinks(next,catalog);
 assert.throws(()=>validateManualLinks({...next,tasks:[next.tasks[0]]},catalog));assert.throws(()=>validateManualLinks({...next,qaRuns:[]},catalog));
});
