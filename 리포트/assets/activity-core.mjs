const DAY=86400000;
export function kstDate(value){const n=Date.parse(value);return Number.isFinite(n)?new Date(n+9*3600000).toISOString().slice(0,10):'';}
export function activityCalendar(rows,today=kstDate(new Date().toISOString())){
 const end=Date.parse(today+'T00:00:00Z');if(!Number.isFinite(end))throw Error('Invalid activity end date');
 const first=end-364*DAY,start=first-new Date(first).getUTCDay()*DAY,counts=new Map();
 for(const row of rows){const date=row.date;if(date>=new Date(first).toISOString().slice(0,10)&&date<=today)counts.set(date,(counts.get(date)||0)+1);}
 const gridEnd=end+(6-new Date(end).getUTCDay())*DAY;
 const days=[];for(let n=start;n<=gridEnd;n+=DAY){const date=new Date(n).toISOString().slice(0,10),count=counts.get(date)||0;days.push({date,count,inRange:n>=first&&n<=end,level:count===0?0:count<=2?1:count<=5?2:count<=9?3:4});}
 return{days,total:[...counts.values()].reduce((a,b)=>a+b,0),activeDays:counts.size,today};
}
