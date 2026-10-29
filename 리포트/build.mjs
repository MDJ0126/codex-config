import { readdir, readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
const root = path.dirname(fileURLToPath(import.meta.url));
const esc = s => String(s).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
// Deliberately restricted Markdown: raw HTML and executable URLs are never rendered.
function inline(s) {
 const tokens=[];
 s=String(s).replace(/(!?)\[([^\]]+)\]\(([^()\s]+)\)/g, (match,image,label,url)=>{
  if(!/^(https?:\/\/|(?:\.\.\/)*(?:assets|reports|posts)\/)/i.test(url)||/[\u0000-\u0020\\]/.test(url))return match;
  const html=image?`<img class="report-image" src="${esc(url)}" alt="${esc(label)}" loading="lazy">`:`<a class="report-link" href="${esc(url)}">${esc(label)}</a>`;
  return `\u0000${tokens.push(html)-1}\u0000`;
 });
 return esc(s).replace(/`([^`]+)`/g,'<code>$1</code>').replace(/\*\*([^*]+)\*\*/g,'<strong>$1</strong>').replace(/\u0000(\d+)\u0000/g,(_,i)=>tokens[Number(i)]);
}
function markdown(s) {
  let out = '', list = false, code = false, buffer = [];
  for (const line of s.split(/\r?\n/)) {
    if (line.startsWith('```')) { if (code) {out += `<pre><code>${esc(buffer.join('\n'))}</code></pre>`;buffer=[];} code=!code;continue; }
    if(code){buffer.push(line);continue;}
    if (!line.startsWith('- ') && list) {out+='</ul>';list=false;}
    const heading = /^(#{1,3}) (.+)$/.exec(line);
    if(heading) out+=`<h${heading[1].length}>${inline(heading[2])}</h${heading[1].length}>`;
    else if(line.startsWith('- ')){if(!list){out+='<ul>';list=true;}out+=`<li>${inline(line.slice(2))}</li>`;}
    else if(line.trim()) out+=`<p>${inline(line)}</p>`;
  }
  if(list)out+='</ul>';if(code)throw new Error('Unclosed code fence');return out;
}
const shell = (title, body, prefix='') => `<!doctype html><html lang="ko"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="color-scheme" content="light"><title>${esc(title)} · 작업 기록실</title><link rel="stylesheet" href="${prefix}assets/style.css"></head><body><header><a class="brand" href="${prefix}index.html"><span class="mark">R</span> 보고서 <small>WORKSPACE</small></a><span class="edition">작업 결과 · 버전 기록</span></header>${body}<footer><span>보고서 아카이브</span><span>날짜 및 버전별 기록</span></footer></body></html>`;
await mkdir(path.join(root,'reports'),{recursive:true});
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
const types=['작업 결과','버그','수정사항','검토','설정'];
const projectIcon=(id,prefix='')=>{const p=projects[id];return p.logo?`<img class="project-logo" src="${prefix}${esc(p.logo)}" alt="${esc(p.name)} 로고">`:`<span class="project-fallback" aria-label="로고 미등록">${esc(p.name.slice(0,1))}</span>`;};
const posts=[];
for(const file of (await readdir(path.join(root,'posts'))).filter(f=>f.endsWith('.md'))){
 const raw=await readFile(path.join(root,'posts',file),'utf8');
 const match=/^---\r?\n([\s\S]*?)\r?\n---\r?\n([\s\S]*)$/.exec(raw);if(!match)throw new Error(`Missing metadata: ${file}`);
 const meta=Object.fromEntries(match[1].split(/\r?\n/).map(l=>{const i=l.indexOf(':');if(i<1)throw new Error(`Invalid metadata: ${file}`);return [l.slice(0,i).trim(),l.slice(i+1).trim()];}));
 for(const key of ['title','date','version','status','category','summary','project','type'])if(!meta[key])throw new Error(`Missing ${key}: ${file}`);
 if(!/^\d{4}-\d{2}-\d{2}T/.test(meta.date)||!Number.isFinite(Date.parse(meta.date)))throw new Error(`Invalid date: ${file}`);
 if(!projects[meta.project])throw new Error(`Unknown project: ${file}`);
 if(!types.includes(meta.type))throw new Error(`Unknown report type: ${file}`);
 const html=file.replace(/\.md$/,'.html');posts.push({...meta,file,html});
 await writeFile(path.join(root,'reports',html),shell(meta.title,`<main class="detail"><a class="back" href="../index.html">← 모든 기록</a><div class="project-heading">${projectIcon(meta.project,'../')}<div><strong>${esc(projects[meta.project].name)}</strong><div class="eyebrow">${esc(meta.type)} / ${esc(meta.version)} · ${esc(meta.category)}</div></div></div><article><div class="article-meta">${esc(meta.date)} <span class="badge">${esc(meta.status)}</span></div>${markdown(match[2])}</article><a class="source" href="../posts/${encodeURIComponent(file)}" download>↓ Markdown 원본 저장</a></main>`,'../'));
}
posts.sort((a,b)=>b.date.localeCompare(a.date)||b.version.localeCompare(a.version,undefined,{numeric:true}));
const rows=posts.map((p,i)=>`<a class="record" data-search="${esc([p.title,p.summary,p.category,p.version,projects[p.project].name,p.type].join(' ').toLowerCase())}" data-project="${esc(p.project)}" data-type="${esc(p.type)}" href="reports/${encodeURIComponent(p.html)}">${projectIcon(p.project)}<div><div class="record-meta">${esc(projects[p.project].name)} <span> / ${esc(p.type)} / ${esc(p.date.slice(0,10))} / ${esc(p.version)}</span></div><h2>${esc(p.title)}</h2><p>${esc(p.summary)}</p></div><span class="badge">${esc(p.status)}</span><span class="arrow">↗</span></a>`).join('');
const activeProjects=Object.entries(projects).filter(([id])=>posts.some(p=>p.project===id));
await writeFile(path.join(root,'index.html'),shell('모든 보고서',`<main><section class="page-heading"><div><div class="eyebrow">WORK REPORTS</div><h1>작업 보고서</h1><p>프로젝트별 결과와 버그·수정사항을 확인하세요.</p></div><span class="total">전체 <strong>${posts.length}</strong>건</span></section><section class="project-tabs" aria-label="프로젝트 선택"><button class="active" data-project-filter="전체" aria-pressed="true">전체 프로젝트 <span>${posts.length}</span></button>${activeProjects.map(([id,p])=>`<button data-project-filter="${esc(id)}" aria-pressed="false">${projectIcon(id)}${esc(p.name)}<span>${posts.filter(post=>post.project===id).length}</span></button>`).join('')}</section><section class="archive"><div class="section-top"><h2>보고서 목록</h2><span>최근 작성순 ↓</span></div><div class="tools"><div class="filters" aria-label="보고서 유형"><button class="active" data-filter="전체" aria-pressed="true">전체</button>${types.map(type=>`<button data-filter="${esc(type)}" aria-pressed="false">${esc(type)}</button>`).join('')}</div><label class="search"><span>검색</span><input id="search" type="search" placeholder="프로젝트, 제목, 버전 검색" aria-label="보고서 검색"></label></div><p id="count" class="count" aria-live="polite">${posts.length}개의 기록</p><div id="records">${rows}</div><p id="empty" hidden>일치하는 보고서가 없습니다. 검색어나 분류를 바꿔주세요.</p></section></main><script src="assets/app.js"></script>`));
console.log(`Built ${posts.length} reports across ${activeProjects.length} projects and index.html`);
