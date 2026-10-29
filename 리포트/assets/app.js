import {chooseStage} from './stage-state.mjs';
import {matchesRecord} from './filter-core.mjs';
const buttons=[...document.querySelectorAll('[data-filter]')],projectButtons=[...document.querySelectorAll('[data-project-filter]')],groupButtons=[...document.querySelectorAll('[data-group-filter]')],stageButtons=[...document.querySelectorAll('[data-stage-filter]')],records=[...document.querySelectorAll('.record')];
const filters={type:'전체',project:'전체',group:'all',stage:'all',query:''};
function select(group,button){for(const b of group){b.classList.toggle('active',b===button);b.setAttribute('aria-pressed',String(b===button));}}
function update(){filters.query=document.querySelector('#search').value;let count=0;for(const row of records){row.hidden=!matchesRecord(row.dataset,filters);if(!row.hidden)count++;}document.querySelector('#count').textContent=`${count}개의 기록`;document.querySelector('#empty').hidden=count!==0;for(const node of document.querySelectorAll('[data-stage-count]'))node.textContent=records.filter(r=>r.dataset.group==='bug'&&r.dataset.stage===node.dataset.stageCount&&matchesRecord(r.dataset,{...filters,group:'bug',stage:'all',type:'전체'})).length;}
for(const b of buttons)b.addEventListener('click',()=>{filters.type=b.dataset.filter;select(buttons,b);update();});
for(const b of projectButtons)b.addEventListener('click',()=>{filters.project=b.dataset.projectFilter;select(projectButtons,b);update();});
for(const b of groupButtons)b.addEventListener('click',()=>{filters.group=b.dataset.groupFilter;filters.stage='all';filters.type='전체';select(groupButtons,b);select(stageButtons,stageButtons.find(x=>x.dataset.stageFilter==='all'));select(buttons,buttons[0]);update();});
for(const b of stageButtons)b.addEventListener('click',()=>{Object.assign(filters,chooseStage(filters,b.dataset.stageFilter));select(groupButtons,groupButtons.find(x=>x.dataset.groupFilter===filters.group));select(buttons,buttons.find(x=>x.dataset.filter===filters.type));select(stageButtons,stageButtons.find(x=>x.dataset.stageFilter===filters.stage));update();});
document.querySelector('#search').addEventListener('input',update);
document.querySelector('#reset-filters').addEventListener('click',()=>{Object.assign(filters,{type:'전체',project:'전체',group:'all',stage:'all',query:''});document.querySelector('#search').value='';for(const list of [buttons,projectButtons,groupButtons])select(list,list[0]);select(stageButtons,stageButtons.find(x=>x.dataset.stageFilter==='all'));update();});
update();
