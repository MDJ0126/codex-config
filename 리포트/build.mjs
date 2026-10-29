import { readdir, readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
const root = path.dirname(fileURLToPath(import.meta.url));
const esc = s => String(s).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
// Deliberately restricted Markdown: raw HTML and executable URLs are never rendered.
function inline(s) { return esc(s).replace(/`([^`]+)`/g,'<code>$1</code>').replace(/\*\*([^*]+)\*\*/g,'<strong>$1</strong>'); }
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
const shell = (title, body, prefix='') => `<!doctype html><html lang="ko"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="color-scheme" content="light"><title>${esc(title)} · 작업 기록실</title><link rel="stylesheet" href="${prefix}assets/style.css"></head><body><header><a class="brand" href="${prefix}index.html"><span class="mark">R</span> 작업 기록실 <small>REPORT ARCHIVE</small></a><span class="edition">PERSONAL WORKSPACE / 2026</span></header>${body}<footer><span>기록은 남기고, 다음 작업으로.</span><span>MARKDOWN → HTML · VERSIONED ARCHIVE</span></footer></body></html>`;
await mkdir(path.join(root,'reports'),{recursive:true});
const posts=[];
for(const file of (await readdir(path.join(root,'posts'))).filter(f=>f.endsWith('.md'))){
 const raw=await readFile(path.join(root,'posts',file),'utf8');
 const match=/^---\r?\n([\s\S]*?)\r?\n---\r?\n([\s\S]*)$/.exec(raw);if(!match)throw new Error(`Missing metadata: ${file}`);
 const meta=Object.fromEntries(match[1].split(/\r?\n/).map(l=>{const i=l.indexOf(':');if(i<1)throw new Error(`Invalid metadata: ${file}`);return [l.slice(0,i).trim(),l.slice(i+1).trim()];}));
 for(const key of ['title','date','version','status','category','summary'])if(!meta[key])throw new Error(`Missing ${key}: ${file}`);
 if(!/^\d{4}-\d{2}-\d{2}T/.test(meta.date)||!Number.isFinite(Date.parse(meta.date)))throw new Error(`Invalid date: ${file}`);
 const html=file.replace(/\.md$/,'.html');posts.push({...meta,file,html});
 await writeFile(path.join(root,'reports',html),shell(meta.title,`<main class="detail"><a class="back" href="../index.html">← 모든 기록</a><div class="eyebrow">${esc(meta.category)} / ${esc(meta.version)}</div><article><div class="article-meta">${esc(meta.date)} <span class="badge">${esc(meta.status)}</span></div>${markdown(match[2])}</article><a class="source" href="../posts/${encodeURIComponent(file)}" download>↓ Markdown 원본 저장</a></main>`,'../'));
}
posts.sort((a,b)=>b.date.localeCompare(a.date)||b.version.localeCompare(a.version,undefined,{numeric:true}));
const rows=posts.map((p,i)=>`<a class="record" data-search="${esc([p.title,p.summary,p.category,p.version].join(' ').toLowerCase())}" data-category="${esc(p.category)}" href="reports/${encodeURIComponent(p.html)}"><span class="number">${String(posts.length-i).padStart(2,'0')}</span><div><div class="record-meta">${esc(p.category)} <span> / ${esc(p.date.slice(0,10))} / ${esc(p.version)}</span></div><h2>${esc(p.title)}</h2><p>${esc(p.summary)}</p></div><span class="badge">${esc(p.status)}</span><span class="arrow">↗</span></a>`).join('');
await writeFile(path.join(root,'index.html'),shell('모든 보고서',`<main><section class="hero"><div><div class="eyebrow"><span class="dot"></span> THE WORK, DOCUMENTED</div><h1>작업의 끝을,<br><em>다음의 시작으로.</em></h1><p>결과와 근거, 그리고 변화의 기록.<br>마크다운으로 쓰고, 하나의 공간에서 이어갑니다.</p></div><aside><span class="aside-label">ARCHIVE NOTES</span><strong>${String(posts.length).padStart(2,'0')}</strong><span>개의 보고서 · 날짜와 버전으로 보관</span><div class="mini-line"></div><small>원본은 Markdown, 읽기는 HTML.<br>이전 기록도 언제든 다시 열어보세요.</small></aside></section><section class="archive"><div class="section-top"><h2>모든 기록 <span>${posts.length}</span></h2><span>최근 작성순 ↓</span></div><div class="tools"><div class="filters"><button class="active" data-filter="전체" aria-pressed="true">전체</button>${[...new Set(posts.map(p=>p.category))].map(c=>`<button data-filter="${esc(c)}" aria-pressed="false">${esc(c)}</button>`).join('')}</div><label class="search"><span>검색</span><input id="search" type="search" placeholder="제목, 내용, 버전 검색" aria-label="보고서 검색"></label></div><p id="count" class="count" aria-live="polite">${posts.length}개의 기록</p><div id="records">${rows}</div><p id="empty" hidden>일치하는 보고서가 없습니다. 검색어나 분류를 바꿔주세요.</p></section><section class="note"><span>01 — WRITE & KEEP</span><p>모든 게시물은 <code>리포트/posts/</code>에 저장됩니다.<br>새 버전을 추가하면 이전 기록과 함께 목록에 쌓입니다.</p></section></main><script src="assets/app.js"></script>`));
console.log(`Built ${posts.length} reports and index.html`);
