import {copyButton} from './copy-link-view.mjs';
import {discussionPanel,discussionsPage} from './discussion-view.mjs';
import {manualDetail,manualLinks,manualsPage} from './manual-view.mjs';
import {validateManualLinks} from './assets/manual-contract.mjs';
import {validateDiscussionLinks} from './assets/discussion-contract.mjs';
import {historicalWorkflow} from './legacy-workflow.mjs';
import {projectMembership} from './project-tree.mjs';
import {createHash} from 'node:crypto';
import {stageIcon} from './stage-icons.mjs';
import {trackerPage} from './tracker-page.mjs';
import {formPage} from './form-page.mjs';
import {validateReport,toMarkdown,WORKFLOW} from './assets/report-contract.mjs';
import {renderMarkdown} from './render.mjs';
import {validatePost} from './standards.mjs';
import {postFiles} from './layout.mjs';
import { readdir, readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
const root = path.dirname(fileURLToPath(import.meta.url));
const esc = s => String(s).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const styleVersion=createHash('sha256').update(await readFile(path.join(root,'assets/style.css'))).update(await readFile(path.join(root,'assets/theme.css'))).update(await readFile(path.join(root,'assets/discussions.css'))).update(await readFile(path.join(root,'assets/manuals.css'))).update(await readFile(path.join(root,'assets/copy-links.css'))).digest('hex').slice(0,12);
const shell = (title, body, prefix='') => `<!doctype html><html lang="ko"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="color-scheme" content="light dark"><script src="${prefix}assets/theme-init.js"></script><title>${esc(title)} · Tracefolio</title><link rel="icon" type="image/png" href="${prefix}assets/brand/tracefolio-mark.png"><link rel="stylesheet" href="${prefix}assets/style.css?v=${styleVersion}"><link rel="stylesheet" href="${prefix}assets/theme.css?v=${styleVersion}"><link rel="stylesheet" href="${prefix}assets/discussions.css?v=${styleVersion}"><link rel="stylesheet" href="${prefix}assets/manuals.css?v=${styleVersion}"><link rel="stylesheet" href="${prefix}assets/copy-links.css?v=${styleVersion}"><script defer src="${prefix}assets/copy-links.js?v=${styleVersion}"></script><script defer src="${prefix}assets/theme.js"></script></head><body><header><a class="brand" href="${prefix}index.html"><img class="archive-brand-logo" src="${prefix}assets/brand/tracefolio-mark.png" alt="Tracefolio 로고"> Tracefolio <small>REPORTS & ISSUES</small></a><nav class="header-nav"><a href="${prefix}manuals/index.html">작업 메뉴얼</a><a href="${prefix}discussions/index.html">QA 토론</a><a href="${prefix}guide/index.html">작성 가이드</a><a class="header-write" href="${prefix}write/index.html">보고서 작성</a><button class="theme-toggle" id="theme-toggle" type="button" aria-label="화면 테마: 시스템 설정. 선택하려면 누르세요"><span class="theme-symbol" aria-hidden="true">◐</span><span id="theme-label">시스템</span></button></nav></header>${body}<div id="copy-status" role="status" aria-live="polite" hidden></div><dialog id="copy-dialog"><p>자동 복사를 사용할 수 없습니다. 주소를 선택해 직접 복사해 주세요.</p><input readonly aria-label="복사할 링크"><button type="button" data-copy-close>닫기</button></dialog><footer><span>Tracefolio</span><span>날짜 및 버전별 기록</span></footer></body></html>`;
await mkdir(path.join(root,'reports'),{recursive:true});
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
const projectGroups=JSON.parse(await readFile(path.join(root,'project-groups.json'),'utf8'));
const memberships=projectMembership(projectGroups,projects);
const types=['작업 결과','버그','수정사항','검토','설정','토론','작업 메뉴얼'];
const projectIcon=(id,prefix='')=>{const p=projects[id];return p.logo?`<img class="project-logo" src="${prefix}${esc(p.logo)}" alt="${esc(p.name)} 로고">`:`<span class="project-fallback" aria-label="로고 미등록">${esc(p.name.slice(0,1))}</span>`;};
const legacyFiles=new Set(JSON.parse(await readFile(path.join(root,'migration/editorial-2026-10-29.json'),'utf8')).map(e=>e.post));
const workflowReviews=JSON.parse(await readFile(path.join(root,'migration/workflow-review-2026-10-30.json'),'utf8'));
const posts=[];
for(const {file,projectId,postPath,reportPath} of await postFiles(root,projects)){
 const raw=await readFile(path.join(root,postPath),'utf8');
 const match=/^---\r?\n([\s\S]*?)\r?\n---\r?\n([\s\S]*)$/.exec(raw);if(!match)throw new Error(`Missing metadata: ${file}`);
 let meta=Object.fromEntries(match[1].split(/\r?\n/).map(l=>{const i=l.indexOf(':');if(i<1)throw new Error(`Invalid metadata: ${file}`);return [l.slice(0,i).trim(),l.slice(i+1).trim()];}));
 for(const key of ['title','date','version','status','category','summary','project','type'])if(!meta[key])throw new Error(`Missing ${key}: ${file}`);
 if(!/^\d{4}-\d{2}-\d{2}T/.test(meta.date)||!Number.isFinite(Date.parse(meta.date)))throw new Error(`Invalid date: ${file}`);
 if(meta.project!==projectId)throw new Error(`Project folder mismatch: ${postPath}`);
 if(!projects[meta.project])throw new Error(`Unknown project: ${file}`);
 if(!types.includes(meta.type))throw new Error(`Unknown report type: ${file}`);
 let manual;
 if(meta.format==='tracefolio-v2'){
  if(!meta.payload)throw new Error('Use the shared form/CLI; payload is required: '+postPath);
  const payloadFile=path.resolve(root,path.dirname(postPath),meta.payload),allowed=path.resolve(root,'projects',meta.project,'assets')+path.sep;
  if(!payloadFile.startsWith(allowed))throw new Error('Invalid payload path');
  const artifact=JSON.parse(await readFile(payloadFile,'utf8'));const shared=validateReport(artifact.report,projects);
  if(shared.kind==='manual')manual=shared;
  const canonical=toMarkdown(shared,{date:meta.date,version:meta.version,issueNumber:meta.issue?Number(meta.issue):undefined,issueURL:meta.issue_url,payloadPath:meta.payload});
  if(raw.replace(/\r\n/g,'\n')!==canonical)throw new Error('Markdown must be generated from the shared report contract: '+postPath);
 }
 meta=historicalWorkflow(meta,raw,postPath,workflowReviews);
 const html=file.replace(/\.md$/,'.html');posts.push({...meta,manual,file,html,postPath,reportPath,body:match[2]});
 await mkdir(path.dirname(path.join(root,reportPath)),{recursive:true});
 validatePost(meta,match[2],postPath,{legacyAllowed:legacyFiles.has(postPath)});
}
posts.sort((a,b)=>Date.parse(b.date)-Date.parse(a.date)||b.version.localeCompare(a.version,undefined,{numeric:true}));
for(const meta of posts){validateDiscussionLinks(meta,posts);validateManualLinks(meta,posts);
 const archived=meta.record==='archived';
 let articleBody=meta.body.replace(/^\s*# [^\n]+\n+/,'').trim();
 if(archived)articleBody=articleBody.replace(/^> 보관 기록입니다\.[^\n]*\n+/, '');
 const content=renderMarkdown(articleBody,{bodyMode:true});
 const date=new Intl.DateTimeFormat('ko-KR',{timeZone:'Asia/Seoul',dateStyle:'long',timeStyle:'short'}).format(new Date(meta.date));
 const postBody=`<main class="detail post-layout"><div class="post-column"><a class="back" href="../../../index.html">← 모든 기록</a><article class="post"><div class="post-project">${projectIcon(meta.project,'../../../')}<span>${memberships[meta.project]?esc(projectGroups[memberships[meta.project]].name)+' › ':''}${esc(projectGroups[memberships[meta.project]]?.labels?.[meta.project]||projects[meta.project].name)}</span><span class="post-kind">${esc(meta.type)}</span></div><div class="post-title-row"><h1>${esc(meta.title)}</h1><span class="badge" data-state="${esc(meta.state||'')}">${esc(meta.status)}</span></div><p class="post-summary">${esc(archived?'원문에 기록된 작업 내용과 당시 검증 범위를 정리한 보관 기록입니다.':meta.summary)}</p>${meta.type==='버그'?`<div class="detail-workflow">${WORKFLOW.map(([id,name])=>`<span data-stage="${id}" class="${meta.workflow===id?'current':''}">${stageIcon(id,'../../../')}${name}</span>`).join('<i>→</i>')}${!meta.workflow?'<small>과거 기록 · 단계 미지정</small>':''}</div>`:''}${meta.workflowReview?`<div class="archive-notice">과거 단계 검토: ${esc(meta.workflowReview.reason)}<br>${esc(meta.workflowReview.scope)}</div>`:''}<div class="post-byline"><time datetime="${esc(meta.date)}">${esc(date)} KST</time><span>${esc(meta.version)}</span><span>${esc(meta.category)}</span>${copyButton('','링크 복사')}</div>${archived?'<div class="archive-notice">보관 기록 · 결과와 검증 범위는 원문 작성 시점을 기준으로 읽어주세요.</div>':''}<div class="post-body">${meta.report_kind==='manual'?manualDetail(meta,posts,content.toc):content.html}</div><div class="post-actions"><a class="source" href="../posts/${encodeURIComponent(meta.file)}" download>Markdown 원본</a><a class="source" href="../../../guide/index.html">작성 기준</a></div></article>${meta.report_kind==='manual'?'':discussionPanel(meta,posts)}${manualLinks(meta,posts)}</div><aside class="post-rail"><span>이 글의 내용</span><nav aria-label="글 목차">${meta.report_kind==='manual'?'<a href="../../../manuals/index.html">작업 메뉴얼 목록</a>':'<a href="#qa-discussion">QA 토론 · 후속 글</a>'}${content.toc.filter(h=>h.depth===2).map(h=>`<a href="#${h.id}">${h.label}</a>`).join('')}</nav><p>${archived?'원문을 보존한 과거 기록입니다.':'프로젝트별 작업 결과와 근거를 기록합니다.'}</p></aside></main>`;
 await writeFile(path.join(root,meta.reportPath),shell(meta.title,postBody,'../../../'));
}
const activeProjects=Object.entries(projects).filter(([id])=>posts.some(p=>p.project===id));
await writeFile(path.join(root,'index.html'),shell('보고서 · 이슈',trackerPage(posts.filter(p=>!['discussion','manual'].includes(p.report_kind)),projects,projectIcon,projectGroups)));
await mkdir(path.join(root,'manuals'),{recursive:true});
await writeFile(path.join(root,'manuals/index.html'),shell('작업 메뉴얼',manualsPage(posts,projects,projectIcon),'../'));
await writeFile(path.join(root,'assets/manual-registry.json'),JSON.stringify(posts.filter(p=>p.manual).map(p=>({reportPath:p.reportPath,postPath:p.postPath,date:p.date,version:p.version,project:p.project,kind:'manual',manualId:p.manual.manualId,round:p.manual.round,manual:p.manual})),null,2));
await mkdir(path.join(root,'discussions'),{recursive:true});
await writeFile(path.join(root,'discussions/index.html'),shell('QA 토론',discussionsPage(posts,projects,projectIcon),'../'));
await writeFile(path.join(root,'assets/report-catalog.json'),JSON.stringify(posts.map(({title,summary,project,reportPath,date,report_kind,type,related_report,reply_to,topic,discussion_state})=>({title,summary,project,reportPath,date,kind:report_kind||(type==='버그'?'bug':'result'),relatedReport:related_report||'',replyTo:reply_to||'',topic,discussionState:discussion_state})),null,2));
await mkdir(path.join(root,'write'),{recursive:true});
await writeFile(path.join(root,'write/index.html'),shell('보고서 작성',formPage(projects,projectGroups),'../'));
await writeFile(path.join(root,'assets/project-registry.json'),JSON.stringify(projects,null,2));
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
await writeFile(path.join(root,'guide/index.html'),shell('AI 게시물 작성 가이드',`<main class="detail guide-page"><a class="back" href="../index.html">← 모든 기록</a><article><div class="eyebrow">TRACEFOLIO · AUTHORING STANDARD V2</div>${guide.html}<div class="post-actions"><a class="source" href="authoring.md" download>AI용 Markdown 가이드</a><a class="source" href="template-result.json" download>결과 JSON 템플릿</a><a class="source" href="template-bug.json" download>버그 JSON 템플릿</a><a class="source" href="template-discussion.json" download>토론 JSON 템플릿</a><a class="source" href="schema.json" download>메타데이터 스키마</a></div></article></main>`,'../'));

const workflowGuide=renderMarkdown(await readFile(path.join(root,'guide/workflow.md'),'utf8'));
await writeFile(path.join(root,'guide/workflow.html'),shell('사람·AI 공통 보고서와 5단계',`<main class="detail guide-page"><article>${workflowGuide.html}</article></main>`,'../'));

const executionGuide=renderMarkdown(await readFile(path.join(root,'guide/execution.md'),'utf8'));
await writeFile(path.join(root,'guide/execution.html'),shell('작업 시작·반복 QA 메뉴얼',`<main class="detail guide-page"><article>${executionGuide.html}<a href="template-manual.json" download>작업 메뉴얼 JSON 템플릿</a></article></main>`,'../'));
