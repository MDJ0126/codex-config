import {matchesRecord} from './filter-core.mjs';
export function trackerCounts(rows,filters={}) {
 // Navigation and stage cards share project/search scope, independently of the selected tab.
 const scoped=rows.filter(row=>matchesRecord(row,{...filters,group:'all',stage:'all',type:'전체'}));
 const bugs=scoped.filter(row=>row.group==='bug');
 const stages={new:0,review:0,in_progress:0,resolved:0,closed:0,unclassified:0};
 for(const row of bugs)stages[row.stage in stages?row.stage:'unclassified']++;
 return {all:scoped.length,result:scoped.length-bugs.length,bug:bugs.length,stages,active:stages.new+stages.review+stages.in_progress};
}
