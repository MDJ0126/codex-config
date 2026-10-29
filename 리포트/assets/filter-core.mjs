export function matchesRecord(row,filters={}){
 const {project='전체',type='전체',group='all',stage='all',query=''}=filters;
 return(project==='전체'||row.project===project)&&(type==='전체'||row.type===type)&&(group==='all'||row.group===group)&&(stage==='all'||row.stage===stage)&&row.search.includes(query.trim().toLowerCase());
}
