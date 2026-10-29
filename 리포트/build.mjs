import {renderMarkdown} from './render.mjs';
import {validatePost} from './standards.mjs';
import {postFiles} from './layout.mjs';
import { readdir, readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
const root = path.dirname(fileURLToPath(import.meta.url));
const esc = s => String(s).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const shell = (title, body, prefix='') => `<!doctype html><html lang="ko"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="color-scheme" content="light"><title>${esc(title)} · Tracefolio</title><link rel="icon" type="image/png" href="${prefix}assets/brand/tracefolio-mark.png"><link rel="stylesheet" href="${prefix}assets/style.css"></head><body><header><a class="brand" href="${prefix}index.html"><img class="archive-brand-logo" src="${prefix}assets/brand/tracefolio-mark.png" alt="Tracefolio 로고"> Tracefolio <small>REPORTS & ISSUES</small></a><nav class="header-nav"><a href="${prefix}guide/index.html">작성 가이드</a><span class="edition">작업 결과 · 버전 기록</span></nav></header>${body}<footer><span>Tracefolio</span><span>날짜 및 버전별 기록</span></footer></body></html>`;
await mkdir(path.join(root,'reports'),{recursive:true});
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
const types=['작업 결과','버그','수정사항','검토','설정'];
const projectIcon=(id,prefix='')=>{const p=projects[id];return p.logo?`<img class="project-logo" src="${prefix}${esc(p.logo)}" alt="${esc(p.name)} 로고">`:`<span class="project-fallback" aria-label="로고 미등록">${esc(p.name.slice(0,1))}</span>`;};
const legacyFiles=new Set(JSON.parse(await readFile(path.join(root,'migration/editorial-2026-10-29.json'),'utf8')).map(e=>e.post));
const posts=[];
for(const {file,projectId,postPath,reportPath} of await postFiles(root,projects)){
 const raw=await readFile(path.join(root,postPath),'utf8');
 const match=/^---\r?\n([\s\S]*?)\r?\n---\r?\n([\s\S]*)$/.exec(raw);if(!match)throw new Error(`Missing metadata: ${file}`);
 const meta=Object.fromEntries(match[1].split(/\r?\n/).map(l=>{const i=l.indexOf(':');if(i<1)throw new Error(`Invalid metadata: ${file}`);return [l.slice(0,i).trim(),l.slice(i+1).trim()];}));
 for(const key of ['title','date','version','status','category','summary','project','type'])if(!meta[key])throw new Error(`Missing ${key}: ${file}`);
 if(!/^\d{4}-\d{2}-\d{2}T/.test(meta.date)||!Number.isFinite(Date.parse(meta.date)))throw new Error(`Invalid date: ${file}`);
 if(meta.project!==projectId)throw new Error(`Project folder mismatch: ${postPath}`);
 if(!projects[meta.project])throw new Error(`Unknown project: ${file}`);
 if(!types.includes(meta.type))throw new Error(`Unknown report type: ${file}`);
 const html=file.replace(/\.md$/,'.html');posts.push({...meta,file,html,postPath,reportPath});
 await mkdir(path.dirname(path.join(root,reportPath)),{recursive:true});
 validatePost(meta,match[2],postPath,{legacyAllowed:legacyFiles.has(postPath)});
 const archived=meta.record==='archived';
 let articleBody=match[2].replace(/^\s*# [^\n]+\n+/,'').trim();
 if(archived)articleBody=articleBody.replace(/^> 보관 기록입니다\.[^\n]*\n+/, '');
 const content=renderMarkdown(articleBody,{bodyMode:true});
 const date=new Intl.DateTimeFormat('ko-KR',{timeZone:'Asia/Seoul',dateStyle:'long',timeStyle:'short'}).format(new Date(meta.date));
 const postBody=`<main class="detail post-layout"><div class="post-column"><a class="back" href="../../../index.html">← 모든 기록</a><article class="post"><div class="post-project">${projectIcon(meta.project,'../../../')}<span>${esc(projects[meta.project].name)}</span><span class="post-kind">${esc(meta.type)}</span></div><div class="post-title-row"><h1>${esc(meta.title)}</h1><span class="badge" data-state="${esc(meta.state||'')}">${esc(meta.status)}</span></div><p class="post-summary">${esc(archived?'원문에 기록된 작업 내용과 당시 검증 범위를 정리한 보관 기록입니다.':meta.summary)}</p><div class="post-byline"><time datetime="${esc(meta.date)}">${esc(date)} KST</time><span>${esc(meta.version)}</span><span>${esc(meta.category)}</span></div>${archived?'<div class="archive-notice">보관 기록 · 결과와 검증 범위는 원문 작성 시점을 기준으로 읽어주세요.</div>':''}<div class="post-body">${content.html}</div><div class="post-actions"><a class="source" href="../posts/${encodeURIComponent(file)}" download>Markdown 원본</a><a class="source" href="../../../guide/index.html">작성 기준</a></div></article></div><aside class="post-rail"><span>이 글의 내용</span><nav aria-label="글 목차">${content.toc.filter(h=>h.depth===2).map(h=>`<a href="#${h.id}">${h.label}</a>`).join('')}</nav><p>${archived?'원문을 보존한 과거 기록입니다.':'프로젝트별 작업 결과와 근거를 기록합니다.'}</p></aside></main>`;
 await writeFile(path.join(root,reportPath),shell(meta.title,postBody,'../../../'));
}
posts.sort((a,b)=>Date.parse(b.date)-Date.parse(a.date)||b.version.localeCompare(a.version,undefined,{numeric:true}));
const rows=posts.map((p,i)=>`<a class="record" data-search="${esc([p.title,p.summary,p.category,p.version,projects[p.project].name,p.type].join(' ').toLowerCase())}" data-project="${esc(p.project)}" data-type="${esc(p.type)}" href="${p.reportPath.split('/').map(encodeURIComponent).join('/')}">${projectIcon(p.project)}<div><div class="record-meta">${esc(projects[p.project].name)} <span> / ${esc(p.type)} / ${esc(p.date.slice(0,10))} / ${esc(p.version)}</span></div><h2>${esc(p.title)}</h2><p>${esc(p.summary)}</p></div><span class="badge">${esc(p.status)}</span><span class="arrow">↗</span></a>`).join('');
const activeProjects=Object.entries(projects).filter(([id])=>posts.some(p=>p.project===id));
await writeFile(path.join(root,'index.html'),shell('모든 보고서',`<main><section class="page-heading"><div><div class="eyebrow">PROJECT JOURNAL</div><h1>프로젝트 기록</h1><p>작업 결과, 발견한 문제, 해결한 내용을 프로젝트별로 확인하세요.</p></div><span class="total">전체 <strong>${posts.length}</strong>건</span></section><section class="project-tabs" aria-label="프로젝트 선택"><button class="active" data-project-filter="전체" aria-pressed="true">전체 프로젝트 <span>${posts.length}</span></button>${activeProjects.map(([id,p])=>`<button data-project-filter="${esc(id)}" aria-pressed="false">${projectIcon(id)}${esc(p.name)}<span>${posts.filter(post=>post.project===id).length}</span></button>`).join('')}</section><section class="archive"><div class="section-top"><h2>보고서 목록</h2><span>최근 작성순 ↓</span></div><div class="tools"><div class="filters" aria-label="보고서 유형"><button class="active" data-filter="전체" aria-pressed="true">전체</button>${types.map(type=>`<button data-filter="${esc(type)}" aria-pressed="false">${esc(type)}</button>`).join('')}</div><label class="search"><span>검색</span><input id="search" type="search" placeholder="프로젝트, 제목, 버전 검색" aria-label="보고서 검색"></label></div><p id="count" class="count" aria-live="polite">${posts.length}개의 기록</p><div id="records">${rows}</div><p id="empty" hidden>일치하는 보고서가 없습니다. 검색어나 분류를 바꿔주세요.</p></section></main><script src="assets/app.js"></script>`));
console.log(`Built ${posts.length} reports across ${activeProjects.length} projects and index.html`);

// Compatibility routes retain previously shared report URLs without duplicate content.
const redirects=JSON.parse(await readFile(path.join(root,'migration/layout-redirects.json'),'utf8'));
let aliasCount=0;
for(const [old,target] of Object.entries(redirects)){
 if(old.startsWith('assets/imports/')&&old.endsWith('.md')){const destination=path.join(root,old);const relative=path.relative(path.dirname(destination),path.join(root,target)).split(path.sep).join('/');await mkdir(path.dirname(destination),{recursive:true});await writeFile(destination,`# Tracefolio · 보고서 이동\n\n[프로젝트 폴더의 원문 열기](${relative})\n`);continue;}
 if(!/\.html$/.test(old))continue;
 const destination=path.join(root,old);const relative=path.relative(path.dirname(destination),path.join(root,target)).split(path.sep).map(encodeURIComponent).join('/');
 await mkdir(path.dirname(destination),{recursive:true});
 await writeFile(destination,`<!doctype html><html lang="ko"><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta http-equiv="refresh" content="0;url=${esc(relative)}"><title>Tracefolio · 보고서 이동</title><link rel="canonical" href="${esc(relative)}"><p>프로젝트 폴더로 이동했습니다. <a href="${esc(relative)}">보고서 열기</a></p></html>`);aliasCount++;
}
console.log(`Generated ${aliasCount} compatibility routes`);

const guideSource=await readFile(path.join(root,'guide/authoring.md'),'utf8');
const guide=renderMarkdown(guideSource);
await writeFile(path.join(root,'guide/index.html'),shell('AI 게시물 작성 가이드',`<main class="detail guide-page"><a class="back" href="../index.html">← 모든 기록</a><article><div class="eyebrow">TRACEFOLIO · AUTHORING STANDARD V1</div>${guide.html}<div class="post-actions"><a class="source" href="authoring.md" download>AI용 Markdown 가이드</a><a class="source" href="template-report.md" download>결과 보고 템플릿</a><a class="source" href="template-bug.md" download>버그 보고 템플릿</a><a class="source" href="schema.json" download>메타데이터 스키마</a></div></article></main>`,'../'));
