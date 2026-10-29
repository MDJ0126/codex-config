import {escapeHTML as esc} from './render.mjs';
export function projectMembership(groups,projects){
 const membership={};
 for(const [id,group] of Object.entries(groups)){
  if(!/^[a-z][a-z0-9-]*$/.test(id)||!group.name||!Array.isArray(group.projects)||!group.projects.length)throw Error('Invalid project group: '+id);
  for(const project of group.projects){if(!Object.hasOwn(projects,project)||membership[project])throw Error('Unknown or duplicate child project: '+project);membership[project]=id;}
 }
 return membership;
}
export function projectTree(posts,projects,groups,icon){
 const membership=projectMembership(groups,projects),count=id=>posts.filter(p=>p.project===id).length;
 const item=id=>`<button data-project-filter="${esc(id)}" aria-pressed="false">${icon(id)}<span>${esc(projects[id].name)}</span><b>${count(id)}</b></button>`;
 const roots=Object.keys(projects).filter(id=>!membership[id]).map(item).join('');
 const branches=Object.entries(groups).map(([id,g])=>`<section class="project-collection"><div class="collection-head"><button data-project-filter="collection:${esc(id)}" data-project-members="${g.projects.join(' ')}" aria-pressed="false"><span class="collection-symbol" aria-hidden="true">▦</span><span>${esc(g.name)}</span><b>${g.projects.reduce((n,p)=>n+count(p),0)}</b></button><button class="collection-toggle" data-collection-toggle="${esc(id)}" aria-expanded="true" aria-controls="children-${esc(id)}" aria-label="${esc(g.name)} 소항목 접기·펼치기">⌄</button></div><div class="project-children" id="children-${esc(id)}">${g.projects.map(item).join('')}</div></section>`).join('');
 return roots+branches;
}
export function projectOptions(projects,groups){
 const membership=projectMembership(groups,projects),option=id=>`<option value="${esc(id)}">${esc(projects[id].name)}</option>`;
 return Object.keys(projects).filter(id=>!membership[id]).map(option).join('')+Object.values(groups).map(g=>`<optgroup label="${esc(g.name)} · 소항목">${g.projects.map(option).join('')}</optgroup>`).join('');
}
