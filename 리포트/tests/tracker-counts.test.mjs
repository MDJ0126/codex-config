import test from 'node:test';
import assert from 'node:assert/strict';
import {readFile} from 'node:fs/promises';
import {trackerCounts} from '../assets/tracker-counts.mjs';
import {matchesRecord} from '../assets/filter-core.mjs';
import {historicalWorkflow} from '../legacy-workflow.mjs';
const root=new URL('../',import.meta.url);
test('all 38 historical stage decisions retain source bytes and exact evidence',async()=>{
 const decisions=JSON.parse(await readFile(new URL('migration/workflow-review-2026-10-30.json',root),'utf8'));
 assert.equal(decisions.length,38);
 assert.equal(new Set(decisions.map(d=>d.post)).size,38);
 for(const d of decisions){const raw=await readFile(new URL(d.post,root),'utf8');const meta=historicalWorkflow({type:'버그'},raw,d.post,decisions);assert.equal(meta.workflow,d.workflow);assert.throws(()=>historicalWorkflow({type:'버그'},raw+'changed',d.post,decisions));}
});
test('project, collection and search scope keep bug total equal to every stage',()=>{
 const rows=[{project:'pixora',collection:'products',group:'bug',type:'버그',stage:'review',search:'pixora 검색'}, {project:'turnroom',collection:'products',group:'bug',type:'버그',stage:'resolved',search:'turnroom 검색'}, {project:'codex-config',collection:'',group:'bug',type:'버그',stage:'closed',search:'codex'}, {project:'pixora',collection:'products',group:'result',type:'수정사항',stage:'none',search:'pixora 검색'}];
 for(const project of ['전체','pixora','collection:products','codex-config'])for(const query of ['','검색','no match'])for(const group of ['all','result','bug']){
  const filters={project,query,group,stage:'resolved',type:'설정'};
  const counts=trackerCounts(rows,filters);
  assert.equal(counts.bug,Object.values(counts.stages).reduce((a,b)=>a+b,0));
  assert.equal(counts.all,counts.result+counts.bug);
  for(const [stage,count]of Object.entries(counts.stages))assert.equal(count,rows.filter(r=>matchesRecord(r,{...filters,group:'bug',stage,type:'전체'})).length);
 }
});
test('review needed records remain counted instead of silently disappearing',()=>{
 const c=trackerCounts([{group:'bug',stage:'unclassified',search:''}]);assert.equal(c.bug,1);assert.equal(c.stages.unclassified,1);assert.equal(c.active,0);
});
