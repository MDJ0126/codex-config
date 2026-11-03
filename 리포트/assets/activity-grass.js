import {activityCalendar} from './activity-core.mjs';
export function createActivityGrass(section,onSelect){
 const grid=section.querySelector('.activity-grid'),months=section.querySelector('.activity-months'),detail=section.querySelector('[data-activity-detail]');
 const popup=document.createElement('div');popup.id='activity-tooltip';popup.className='activity-tooltip';popup.setAttribute('role','region');popup.setAttribute('aria-label','날짜별 보고서 미리보기');popup.hidden=true;document.body.append(popup);
 let selected='',lastRows=null,calendar=null,current=null,closeTimer,dayMap=new Map();
 function cancelClose(){clearTimeout(closeTimer);}
 function hide(){cancelClose();popup.hidden=true;current?.cell.setAttribute('aria-expanded','false');current=null;}
 function scheduleClose(){cancelClose();closeTimer=setTimeout(()=>{if(!popup.matches(':hover')&&!popup.contains(document.activeElement)&&!current?.cell.matches(':hover')&&document.activeElement!==current?.cell)hide();},140);}
 function show(entry){cancelClose();if(current===entry&&!popup.hidden)return;current?.cell.setAttribute('aria-expanded','false');const viewKey=innerWidth+':'+innerHeight+':'+scrollX+':'+scrollY+':'+section.querySelector('.activity-scroll').scrollLeft;if(entry.viewKey!==viewKey){entry.rect=entry.cell.getBoundingClientRect();entry.viewKey=viewKey;}const box=entry.rect;current=entry;popup.replaceChildren(entry.panel);popup.hidden=false;entry.cell.setAttribute('aria-expanded','true');const width=Math.min(350,innerWidth-16);if(entry.width!==width){entry.height=popup.offsetHeight;entry.width=width;}popup.style.left=Math.max(8,Math.min(innerWidth-width-8,box.left+box.width/2-width/2))+'px';popup.style.top=Math.max(8,Math.min(innerHeight-entry.height-8,box.top>=entry.height+12?box.top-entry.height:box.bottom))+'px';}
 popup.addEventListener('mouseenter',cancelClose);popup.addEventListener('mouseleave',scheduleClose);popup.addEventListener('focusin',cancelClose);popup.addEventListener('focusout',scheduleClose);
 document.addEventListener('keydown',e=>{if(e.key==='Escape'&&!popup.hidden){const cell=current?.cell,inside=popup.contains(document.activeElement);hide();if(inside){cell?.focus();hide();}}});
 document.addEventListener('pointerdown',e=>{if(!popup.contains(e.target)&&!grid.contains(e.target))hide();});
 window.addEventListener('scroll',e=>{if(popup.contains(e.target))return;const entry=current;if(entry&&(entry.cell.matches(':hover')||popup.matches(':hover')||popup.contains(document.activeElement))){entry.viewKey=null;current=null;show(entry);}else hide();},true);window.addEventListener('resize',hide);
 section.querySelector('[data-clear-date]').addEventListener('click',()=>onSelect(''));
 return function update(rows,date=''){
  selected=date;
  if(!lastRows||rows.length!==lastRows.length||rows.some((row,i)=>row!==lastRows[i])){
   lastRows=rows.slice();calendar=activityCalendar(rows);hide();dayMap=new Map();grid.replaceChildren();months.replaceChildren();const byDate=new Map();for(const row of rows){if(!byDate.has(row.date))byDate.set(row.date,[]);byDate.get(row.date).push(row);}let lastMonth='';
   for(const [i,day]of calendar.days.entries()){
    if(i%7===0){const label=document.createElement('span'),month=day.date.slice(0,7);label.textContent=month!==lastMonth?Number(day.date.slice(5,7))+'월':'';lastMonth=month;months.append(label);}
    const cell=document.createElement('button'),items=byDate.get(day.date)||[];cell.type='button';cell.className='activity-day';cell.dataset.date=day.date;cell.dataset.level=day.level;cell.dataset.count=day.count;cell.disabled=!day.inRange;cell.setAttribute('aria-label',day.date+' · '+day.count+'건'+(items.length?' · '+items.slice(0,5).map(r=>r.title).join(', '):''));cell.setAttribute('aria-controls',popup.id);cell.setAttribute('aria-expanded','false');
    const panel=document.createElement('div'),heading=document.createElement('strong');heading.textContent=day.date+' · '+day.count+'건';panel.append(heading);
    for(const item of items.slice(0,5)){const link=document.createElement('a');link.className='activity-report-link';link.textContent=item.title||'제목 없음';link.href=item.href;panel.append(link);}
    if(items.length>5){const more=document.createElement('button');more.className='activity-more';more.textContent='외 '+(items.length-5)+'건 · 그날 보고서 전체 보기';more.addEventListener('click',()=>{onSelect(day.date);hide();});panel.append(more);}
    if(!items.length){const empty=document.createElement('div');empty.textContent='작성된 보고서가 없습니다.';panel.append(empty);}
    const entry={cell,panel,count:day.count,width:0,height:0};dayMap.set(day.date,entry);cell.addEventListener('mouseenter',()=>show(entry));cell.addEventListener('mouseleave',scheduleClose);cell.addEventListener('focus',()=>show(entry));cell.addEventListener('blur',scheduleClose);cell.addEventListener('click',()=>onSelect(selected===day.date?'':day.date));grid.append(cell);
   }
   section.querySelector('[data-activity-total]').textContent=calendar.total+'건';section.querySelector('[data-activity-range]').textContent=`최근 1년 · 활동 ${calendar.activeDays}일 · KST`;section.querySelector('.activity-scroll').scrollLeft=section.querySelector('.activity-scroll').scrollWidth;
  }
  for(const cell of grid.children)cell.setAttribute('aria-pressed',String(cell.dataset.date===date));
  detail.textContent=date?`${date} · ${dayMap.get(date)?.count||0}건`:'날짜를 누르면 해당 글을 볼 수 있습니다.';section.querySelector('[data-clear-date]').hidden=!date;
 };
}
