import {activityCalendar} from './activity-core.mjs';
export function createActivityGrass(section,onSelect){
 const grid=section.querySelector('.activity-grid'),months=section.querySelector('.activity-months'),detail=section.querySelector('[data-activity-detail]');
 let signature='',selected='';
 const tooltip=document.createElement('div');tooltip.id='activity-tooltip';tooltip.className='activity-tooltip';tooltip.setAttribute('role','tooltip');tooltip.hidden=true;document.body.append(tooltip);
 function hide(){tooltip.hidden=true;}
 function show(cell,day,titles){tooltip.replaceChildren();const heading=document.createElement('strong');heading.textContent=day.date+' · '+day.count+'건';tooltip.append(heading);for(const title of titles.slice(0,5)){const line=document.createElement('div');line.textContent='• '+title;tooltip.append(line);}if(titles.length>5){const more=document.createElement('small');more.textContent='외 '+(titles.length-5)+'건 · 클릭해서 전체 보기';tooltip.append(more);}if(!titles.length){const empty=document.createElement('div');empty.textContent='작성된 보고서가 없습니다.';tooltip.append(empty);}tooltip.hidden=false;const box=cell.getBoundingClientRect(),tip=tooltip.getBoundingClientRect();tooltip.style.left=Math.max(8,Math.min(innerWidth-tip.width-8,box.left+box.width/2-tip.width/2))+'px';tooltip.style.top=(box.top>=tip.height+12?box.top-tip.height-8:box.bottom+8)+'px';}
 window.addEventListener('scroll',hide,true);window.addEventListener('resize',hide);section.addEventListener('keydown',e=>{if(e.key==='Escape')hide();});
 section.querySelector('[data-clear-date]').addEventListener('click',()=>onSelect(''));
 return function update(rows,date=''){
  selected=date;const data=activityCalendar(rows),titlesByDate=new Map();for(const row of rows){if(!titlesByDate.has(row.date))titlesByDate.set(row.date,[]);titlesByDate.get(row.date).push(row.title||'제목 없음');}const key=JSON.stringify([data,[...titlesByDate]]);
  if(key!==signature){signature=key;hide();grid.replaceChildren();months.replaceChildren();let lastMonth='';
   data.days.forEach((day,i)=>{if(i%7===0){const label=document.createElement('span'),month=day.date.slice(0,7);label.textContent=month!==lastMonth?Number(day.date.slice(5,7))+'월':'';lastMonth=month;months.append(label);}
    const cell=document.createElement('button');cell.type='button';cell.className='activity-day';cell.dataset.date=day.date;cell.dataset.level=day.level;cell.disabled=!day.inRange;const titles=titlesByDate.get(day.date)||[];cell.dataset.count=day.count;cell.setAttribute('aria-label',day.date+' · '+day.count+'건'+(titles.length?' · '+titles.slice(0,5).join(', '):''));cell.setAttribute('aria-describedby',tooltip.id);cell.addEventListener('mouseenter',()=>show(cell,day,titles));cell.addEventListener('mouseleave',hide);cell.addEventListener('focus',()=>show(cell,day,titles));cell.addEventListener('blur',hide);cell.addEventListener('click',()=>onSelect(selected===day.date?'':day.date));grid.append(cell);
   });
   section.querySelector('[data-activity-total]').textContent=data.total+'건';section.querySelector('[data-activity-range]').textContent=`최근 1년 · 활동 ${data.activeDays}일 · KST`;
   section.querySelector('.activity-scroll').scrollLeft=section.querySelector('.activity-scroll').scrollWidth;
  }
  for(const cell of grid.children)cell.setAttribute('aria-pressed',String(cell.dataset.date===date));
  detail.textContent=date?`${date} · ${data.days.find(d=>d.date===date)?.count||0}건`:'날짜를 누르면 해당 글을 볼 수 있습니다.';
  section.querySelector('[data-clear-date]').hidden=!date;
 };
}
