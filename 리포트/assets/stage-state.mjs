// Clearing a stage also clears the bug filters introduced by selecting it.
export function chooseStage(filters,requested){
 const stage=requested==='all'||filters.stage===requested?'all':requested;
 return {...filters,stage,group:stage==='all'?'all':'bug',type:stage==='all'?'전체':'버그'};
}
