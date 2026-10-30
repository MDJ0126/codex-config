import test from 'node:test';
import assert from 'node:assert/strict';
import {validateReport,toMarkdown,parseIssueBody,issueBody} from '../assets/report-contract.mjs';
import {validateDiscussionLinks} from '../assets/discussion-contract.mjs';
import {validatePost} from '../standards.mjs';
import {currentDiscussions} from '../discussion-view.mjs';
const projects={pixora:{name:'Pixora'},'codex-config':{name:'Codex'}};
const qa='projects/pixora/reports/qa.html',thread='projects/pixora/reports/thread.html';
const base={schemaVersion:2,project:'pixora',kind:'discussion',title:'QA 후속 논의',summary:'동일 입력의 차이 검토',outcome:'차이를 확인했습니다.',topic:'qa',discussionState:'open',relatedReport:qa};
test('human and AI discussion share validation, markdown sections and issue round trip',()=>{
 const r=validateReport(base,projects);assert.equal(r.type,'토론');assert.equal(r.state,'progress');assert.equal(r.discussionState,'open');assert.deepEqual(validateReport(parseIssueBody(issueBody(r)),projects),r);
 const md=toMarkdown(r,{date:'2026-10-30T07:00:00Z'}),[,header,body]=md.split('---'),meta=Object.fromEntries(header.trim().split('\n').map(l=>{const i=l.indexOf(':');return[l.slice(0,i),l.slice(i+1).trim()];}));validatePost(meta,body,'discussion.md');assert.match(md,/## QA 근거와 맥락/);assert.doesNotMatch(md,/workflow:/);
});
test('QA reference, decisions and implemented evidence are enforced',()=>{
 assert.throws(()=>validateReport({...base,relatedReport:''},projects));
 for(const discussionState of ['accepted','deferred','implemented'])assert.throws(()=>validateReport({...base,discussionState},projects));
 assert.throws(()=>validateReport({...base,discussionState:'implemented',decision:'반영'},projects));
 assert.throws(()=>validateReport({...base,decision:'ghp_'+'a'.repeat(30)},projects));
 assert.equal(validateReport({...base,discussionState:'implemented',decision:'반영',verification:'실행 근거'},projects).state,'complete');
 assert.equal(validateReport({...base,topic:'feature',relatedReport:''},projects).topic,'feature');
 for(const relatedReport of ['https://example.com','projects/other/reports/qa.html','projects/pixora/reports/../qa.html','projects/pixora/reports/qa.html\nproject: other'])assert.throws(()=>validateReport({...base,relatedReport},projects));
});
test('newest issue discussion is listed once while older versions remain available',()=>{
 const old={report_kind:'discussion',issue:'12',date:'2026-10-29T00:00:00Z',version:'v1.0.1'},latest={...old,date:'2026-10-30T00:00:00Z',version:'v1.0.2'};assert.deepEqual(currentDiscussions([old,latest]),[latest]);
});
test('report and reply relationships reject missing, wrong-project and mismatched roots',()=>{
 const catalog=[{project:'pixora',kind:'result',reportPath:qa},{project:'pixora',kind:'discussion',reportPath:thread,relatedReport:qa}];
 validateDiscussionLinks(validateReport({...base,replyTo:thread},projects),catalog);
 assert.throws(()=>validateDiscussionLinks(base,[]));assert.throws(()=>validateDiscussionLinks({...base,relatedReport:thread},catalog));assert.throws(()=>validateDiscussionLinks({...base,replyTo:qa},catalog));assert.throws(()=>validateDiscussionLinks({...base,replyTo:thread,relatedReport:''},catalog));
});
