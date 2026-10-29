import {matchesRecord} from './assets/filter-core.mjs';
import {postFiles} from './layout.mjs';
import assert from 'node:assert/strict';
import { readFile, readdir, access } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
import vm from 'node:vm';
const root=path.dirname(fileURLToPath(import.meta.url));
const index=await readFile(path.join(root,'index.html'),'utf8');
const registry=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
const entries=await postFiles(root,registry);
const reports=entries.map(e=>e.reportPath);
const posts=entries.map(e=>e.postPath);
assert.equal(posts.length,reports.length);
let links=0;
for(const file of ['index.html','write/index.html','guide/index.html',...reports,...(await readdir(path.join(root,'reports'))).filter(f=>f.endsWith('.html')).map(f=>`reports/${f}`)]){
 const html=await readFile(path.join(root,file),'utf8');
 assert.match(html,/<html lang="ko">/);assert.match(html,/name="viewport"/);
 for(const match of html.matchAll(/(?:href|src)="([^"]+)"/g)){
  const target=decodeURIComponent(match[1]).split(/[?#]/)[0];if(!target||/^[a-z]+:/.test(target))continue;
  const resolved=path.resolve(path.dirname(path.join(root,file)),target);
  assert.ok(resolved.startsWith(root+path.sep));await access(resolved);links++;
 }
}
const elements=[...index.matchAll(/data-search="([^"]*)" data-project="([^"]*)" data-type="([^"]*)" data-group="([^"]*)" data-stage="([^"]*)" data-collection="([^"]*)"/g)].map(m=>({search:m[1],project:m[2],type:m[3],group:m[4],stage:m[5],collection:m[6]}));
assert.ok(elements.length>0);
const projectGroups=JSON.parse(await readFile(path.join(root,'project-groups.json'),'utf8'));
for(const [id,g]of Object.entries(projectGroups)){const expected=elements.filter(e=>g.projects.includes(e.project));assert.ok(expected.length>0);assert.deepEqual(elements.filter(e=>matchesRecord(e,{project:'collection:'+id})),expected);for(const group of ['result','bug'])assert.equal(elements.filter(e=>matchesRecord(e,{project:'collection:'+id,group})).length,expected.filter(e=>e.group===group).length);}
assert.equal(elements.filter(e=>matchesRecord(e,{query:'없는검색결과'})).length,0);
for(const project of Object.keys(registry))assert.equal(elements.filter(e=>matchesRecord(e,{project})).length,elements.filter(e=>e.project===project).length);
for(const group of ['result','bug'])assert.equal(elements.filter(e=>matchesRecord(e,{group})).length,elements.filter(e=>e.group===group).length);
assert.equal(elements.filter(e=>matchesRecord(e,{project:'pixora',type:'수정사항',query:'v1.1.2'})).length,1);
for(const stage of ['new','review','in_progress','resolved','closed','unclassified'])assert.equal(elements.filter(e=>matchesRecord(e,{group:'bug',stage})).length,elements.filter(e=>e.group==='bug'&&e.stage===stage).length);
assert.ok(!index.includes('design-example'));
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
for(const p of Object.values(projects)){if(p.logo){const data=await readFile(path.join(root,p.logo));assert.equal(data.subarray(1,4).toString(),'PNG');assert.ok(data.length>100);}}
console.log(`PASS: ${reports.length} Markdown/HTML pairs, ${links} local links, project-scoped folders, legacy redirect targets, all project/type filters, combined project+type+version search, empty/reset, example removal and PNG logos. Browser layout not tested.`);
