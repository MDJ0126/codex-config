import {activityCalendar} from './activity-core.mjs';
export function createActivityGrass(section,onSelect){
 const grid=section.querySelector('.activity-grid'),months=section.querySelector('.activity-months'),detail=section.querySelector('[data-activity-detail]');
 let signature='',selected='';
 section.querySelector('[data-clear-date]').addEventListener('click',()=>onSelect(''));
 return function update(rows,date=''){
  selected=date;const data=activityCalendar(rows),key=JSON.stringify(data);
  if(key!==signature){signature=key;grid.replaceChildren();months.replaceChildren();let lastMonth='';
   data.days.forEach((day,i)=>{if(i%7===0){const label=document.createElement('span'),month=day.date.slice(0,7);label.textContent=month!==lastMonth?Number(day.date.slice(5,7))+'월':'';lastMonth=month;months.append(label);}
    const cell=document.createElement('button');cell.type='button';cell.className='activity-day';cell.dataset.date=day.date;cell.dataset.level=day.level;cell.disabled=!day.inRange;cell.title=day.date+' · '+day.count+'건';cell.setAttribute('aria-label',cell.title);cell.addEventListener('click',()=>onSelect(selected===day.date?'':day.date));grid.append(cell);
   });
   section.querySelector('[data-activity-total]').textContent=data.total+'건';section.querySelector('[data-activity-range]').textContent=`최근 1년 · 활동 ${data.activeDays}일 · KST`;
   section.querySelector('.activity-scroll').scrollLeft=section.querySelector('.activity-scroll').scrollWidth;
  }
  for(const cell of grid.children)cell.setAttribute('aria-pressed',String(cell.dataset.date===date));
  detail.textContent=date?`${date} · ${data.days.find(d=>d.date===date)?.count||0}건`:'날짜를 누르면 해당 글을 볼 수 있습니다.';
  section.querySelector('[data-clear-date]').hidden=!date;
 };
}
