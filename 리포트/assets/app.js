const buttons=[...document.querySelectorAll('[data-filter]')];
const projectButtons=[...document.querySelectorAll('[data-project-filter]')];
const records=[...document.querySelectorAll('.record')];
let type='전체',project='전체';
function update(){const query=document.querySelector('#search').value.trim().toLowerCase();let count=0;for(const row of records){const show=(type==='전체'||row.dataset.type===type)&&(project==='전체'||row.dataset.project===project)&&row.dataset.search.includes(query);row.hidden=!show;if(show)count++;}document.querySelector('#count').textContent=`${count}개의 기록`;document.querySelector('#empty').hidden=count!==0;}
function select(group,button){for(const b of group){b.classList.toggle('active',b===button);b.setAttribute('aria-pressed',String(b===button));}}
for(const button of buttons)button.addEventListener('click',()=>{type=button.dataset.filter;select(buttons,button);update();});
for(const button of projectButtons)button.addEventListener('click',()=>{project=button.dataset.projectFilter;select(projectButtons,button);update();});
document.querySelector('#search').addEventListener('input',update);
