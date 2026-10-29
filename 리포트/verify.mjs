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
const elements=[...index.matchAll(/data-search="([^"]*)" data-category="([^"]*)"/g)].map(m=>({dataset:{search:m[1],category:m[2]},hidden:false}));
const buttons=[...index.matchAll(/data-filter="([^"]*)"/g)].map(m=>({dataset:{filter:m[1]},classList:{toggle(){}},setAttribute(){},addEventListener(event,fn){this.click=fn;}}));
const search={value:'',addEventListener(event,fn){this.input=fn;}};const count={};const empty={};
const document={querySelectorAll:s=>s==='[data-filter]'?buttons:elements,querySelector:s=>({'#search':search,'#count':count,'#empty':empty}[s])};
vm.runInNewContext(await readFile(path.join(root,'assets/app.js'),'utf8'),{document});
search.value='없는검색결과';search.input();assert.ok(elements.every(e=>e.hidden));assert.equal(empty.hidden,false);
search.value='';search.input();assert.equal(elements.filter(e=>!e.hidden).length,posts.length);
const design=buttons.find(b=>b.dataset.filter==='디자인');design.click();assert.equal(elements.filter(e=>!e.hidden).length,2);
search.value='v1.1.0';search.input();assert.equal(elements.filter(e=>!e.hidden).length,1);
buttons[0].click();search.value='';search.input();assert.equal(elements.filter(e=>!e.hidden).length,posts.length);
assert.ok(index.indexOf('report-archive')<index.indexOf('design-example'));
console.log(`PASS: ${reports.length} Markdown/HTML pairs, ${links} local links, date order, empty search, reset, category and combined version search. Browser layout not tested.`);
