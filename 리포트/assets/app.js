const buttons=[...document.querySelectorAll('[data-filter]')];
const records=[...document.querySelectorAll('.record')];
let category='전체';
function update(){const query=document.querySelector('#search').value.trim().toLowerCase();let count=0;for(const row of records){const show=(category==='전체'||row.dataset.category===category)&&row.dataset.search.includes(query);row.hidden=!show;if(show)count++;}document.querySelector('#count').textContent=`${count}개의 기록`;document.querySelector('#empty').hidden=count!==0;}
for(const button of buttons)button.addEventListener('click',()=>{category=button.dataset.filter;for(const b of buttons){b.classList.toggle('active',b===button);b.setAttribute('aria-pressed',String(b===button));}update();});
document.querySelector('#search').addEventListener('input',update);
