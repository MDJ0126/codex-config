import {readdir} from 'node:fs/promises';
import path from 'node:path';
export async function postFiles(root,projects){
 const posts=[];
 for(const entry of await readdir(path.join(root,'projects'),{withFileTypes:true})){if(entry.isDirectory()&&!projects[entry.name])throw new Error(`Unregistered project folder: ${entry.name}`);}
 for(const projectId of Object.keys(projects)){
  const folder=`projects/${projectId}/posts`;
  let entries;try{entries=await readdir(path.join(root,folder));}catch(e){if(e.code==='ENOENT')continue;throw e;}
  for(const file of entries.filter(f=>f.endsWith('.md'))){posts.push({file,projectId,postPath:`${folder}/${file}`,reportPath:`projects/${projectId}/reports/${file.replace(/\.md$/,'.html')}`});}
 }
 return posts;
}
