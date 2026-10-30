import {readFile} from 'node:fs/promises';
import path from 'node:path';
import {postFiles} from './layout.mjs';
export async function readCatalog(root,projects){
 const items=[];for(const p of await postFiles(root,projects)){const raw=await readFile(path.join(root,p.postPath),'utf8'),header=raw.match(/^---\r?\n([\s\S]*?)\r?\n---/);if(!header)throw new Error('Missing report metadata');const meta=Object.fromEntries(header[1].split(/\r?\n/).map(l=>{const i=l.indexOf(':');return[l.slice(0,i).trim(),l.slice(i+1).trim()];}));let manual;if(meta.report_kind==='manual'&&meta.payload){const payload=path.resolve(root,path.dirname(p.postPath),meta.payload),allowed=path.resolve(root,'projects',meta.project,'assets')+path.sep;if(!payload.startsWith(allowed))throw new Error('Invalid manual payload');manual=JSON.parse(await readFile(payload,'utf8')).report;}items.push({...meta,...p,manual,kind:meta.report_kind|| (meta.type==='버그'?'bug':'result')});}return items;
}
