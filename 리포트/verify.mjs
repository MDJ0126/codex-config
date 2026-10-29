import assert from 'node:assert/strict';
import { readFile, readdir, access } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
import vm from 'node:vm';
const root=path.dirname(fileURLToPath(import.meta.url));
const index=await readFile(path.join(root,'index.html'),'utf8');
const reports=(await readdir(path.join(root,'reports'))).filter(f=>f.endsWith('.html'));
const posts=(await readdir(path.join(root,'posts'))).filter(f=>f.endsWith('.md'));
assert.equal(posts.length,reports.length);
let links=0;
for(const file of ['index.html',...reports.map(f=>`reports/${f}`)]){
 const html=await readFile(path.join(root,file),'utf8');
 assert.match(html,/<html lang="ko">/);assert.match(html,/name="viewport"/);
 for(const match of html.matchAll(/(?:href|src)="([^"]+)"/g)){
  const target=decodeURIComponent(match[1]);if(/^[a-z]+:/.test(target))continue;
  const resolved=path.resolve(path.dirname(path.join(root,file)),target);
  assert.ok(resolved.startsWith(root+path.sep));await access(resolved);links++;
 }
}
const elements=[...index.matchAll(/data-search="([^"]*)" data-project="([^"]*)" data-type="([^"]*)"/g)].map(m=>({dataset:{search:m[1],project:m[2],type:m[3]},hidden:false}));
const buttons=[...index.matchAll(/data-filter="([^"]*)"/g)].map(m=>({dataset:{filter:m[1]},classList:{toggle(){}},setAttribute(){},addEventListener(event,fn){this.click=fn;}}));
const projectButtons=[...index.matchAll(/data-project-filter="([^"]*)"/g)].map(m=>({dataset:{projectFilter:m[1]},classList:{toggle(){}},setAttribute(){},addEventListener(event,fn){this.click=fn;}}));
const search={value:'',addEventListener(event,fn){this.input=fn;}};const count={};const empty={};
const document={querySelectorAll:s=>s==='[data-filter]'?buttons:s==='[data-project-filter]'?projectButtons:elements,querySelector:s=>({'#search':search,'#count':count,'#empty':empty}[s])};
vm.runInNewContext(await readFile(path.join(root,'assets/app.js'),'utf8'),{document});
search.value='없는검색결과';search.input();assert.ok(elements.every(e=>e.hidden));assert.equal(empty.hidden,false);
search.value='';search.input();assert.equal(elements.filter(e=>!e.hidden).length,posts.length);

const visible=()=>elements.filter(e=>!e.hidden);
for(const b of projectButtons){b.click();assert.equal(visible().length,elements.filter(e=>b.dataset.projectFilter==='전체'||e.dataset.project===b.dataset.projectFilter).length);}
projectButtons[0].click();
for(const b of buttons){b.click();assert.equal(visible().length,elements.filter(e=>b.dataset.filter==='전체'||e.dataset.type===b.dataset.filter).length);}
const pixora=projectButtons.find(b=>b.dataset.projectFilter==='pixora');pixora.click();buttons.find(b=>b.dataset.filter==='수정사항').click();search.value='v1.1.2';search.input();assert.equal(visible().length,1);assert.equal(visible()[0].dataset.project,'pixora');assert.equal(visible()[0].dataset.type,'수정사항');
projectButtons[0].click();buttons[0].click();search.value='';search.input();assert.equal(visible().length,posts.length);
assert.ok(!index.includes('design-example'));
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
for(const p of Object.values(projects)){if(p.logo){const data=await readFile(path.join(root,p.logo));assert.equal(data.subarray(1,4).toString(),'PNG');assert.ok(data.length>100);}}
console.log(`PASS: ${reports.length} Markdown/HTML pairs, ${links} local links, all project/type filters, combined project+type+version search, empty/reset, example removal and PNG logos. Browser layout not tested.`);
