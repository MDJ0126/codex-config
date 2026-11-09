import {readFile,writeFile} from 'node:fs/promises';
import path from 'node:path';
import {escapeHTML as esc,renderMarkdown} from './render.mjs';
import {projectMembership} from './project-tree.mjs';

export async function buildProjectGuidelines(root,projects,groups,shell){
 const membership=projectMembership(groups,projects);
 const folderIcon='<span aria-hidden="true">📁</span>';
 const item=(id,label=projects[id].name)=>`<details class="guideline-folder" data-guideline-project="${esc(id)}"><summary>${folderIcon} ${esc(label)}</summary><div class="guideline-folder-links"><a class="report-link" href="../projects/${esc(id)}/guidelines.html">전용 지침 사항</a><a href="../index.html?project=${esc(id)}">프로젝트 기록</a></div></details>`;
 const groupsHtml=Object.entries(groups).map(([id,g])=>`<details class="guideline-folder guideline-folder-group" id="group-${esc(id)}" open><summary>${folderIcon} ${esc(g.name)} <span class="guideline-folder-count">${g.projects.length}개 하위 폴더</span></summary><div class="guideline-folder-children">${g.projects.map(project=>item(project,g.labels?.[project]??projects[project].name)).join('')}</div></details>`).join('');
 const standalone=Object.keys(projects).filter(id=>!membership[id]).map(id=>item(id)).join('');
 await writeFile(path.join(root,'guide/project-instructions.html'),shell('프로젝트 전용 지침 사항',`<main class="detail guide-page"><article><h1>프로젝트 전용 지침 사항</h1><p>프로젝트 폴더를 열면 해당 프로젝트의 전용 지침과 작업 기록을 볼 수 있습니다. 모든 프로젝트에는 <a href="global-instructions.html">전역 지침 사항</a>이 함께 적용됩니다.</p>${groupsHtml?'<h2>프로젝트 묶음</h2>'+groupsHtml:''}<h2>개별 프로젝트</h2><div class="guideline-folders">${standalone}</div></article></main>`,'../'));
 for(const [id,project] of Object.entries(projects)){
  if(!/^[a-z][a-z0-9-]*$/.test(id))throw Error('Invalid guidelines project ID: '+id);
  const folder=path.join(root,'projects',id);
  const source=await readFile(path.join(folder,'guidelines.md'),'utf8');
  if(!source.trim())throw Error('Empty project guidelines: '+id);
  const content=renderMarkdown(source,{bodyMode:true});
  const title=project.name+' 전용 지침 사항';
  await writeFile(path.join(folder,'guidelines.html'),shell(title,`<main class="detail guide-page"><a class="back" href="../../guide/project-instructions.html">← 프로젝트 전용 지침 모아보기</a><article><h1>${esc(title)}</h1>${content.html}<div class="post-actions"><a class="source" href="../../index.html?project=${id}">프로젝트 기록 보기</a><a class="source" href="../../guide/global-instructions.html">전역 지침 사항</a><a class="source" href="guidelines.md" download>Markdown 원문</a></div></article></main>`,'../../'));
 }
}

export function projectGuidelinesLinks(projects,groups){
 const projectLinks=Object.entries(projects).map(([id,p])=>`<a class="report-link" data-project-guidelines-for="${esc(id)}" href="projects/${esc(id)}/guidelines.html" hidden>${esc(p.name)} 전용 지침 사항</a>`);
 const groupLinks=Object.entries(groups).map(([id,g])=>`<a class="report-link" data-project-guidelines-for="collection:${esc(id)}" href="guide/project-instructions.html#group-${esc(id)}" hidden>${esc(g.name)} 프로젝트 전용 지침 모아보기</a>`);
 return projectLinks.concat(groupLinks).join('');
}
