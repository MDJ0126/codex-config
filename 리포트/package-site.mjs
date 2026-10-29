import { cp, mkdir, writeFile, rm, lstat } from 'node:fs/promises';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
const root=path.dirname(fileURLToPath(import.meta.url));
const output=path.join(root,'.site');
if(path.relative(root,output)!=='.site')throw new Error('Invalid output path');
try{if((await lstat(output)).isSymbolicLink())throw new Error('Output must not be a symlink');}catch(e){if(e.code!=='ENOENT')throw e;}
await rm(output,{recursive:true,force:true});
await mkdir(output,{recursive:true});
for(const entry of ['index.html','assets','reports'])await cp(path.join(root,entry),path.join(output,entry),{recursive:true});
const projects=JSON.parse(await (await import('node:fs/promises')).readFile(path.join(root,'projects.json'),'utf8'));
for(const id of Object.keys(projects)){for(const entry of ['posts','reports','assets']){const from=path.join(root,'projects',id,entry);try{await cp(from,path.join(output,'projects',id,entry),{recursive:true});}catch(e){if(e.code!=='ENOENT')throw e;}}}
await writeFile(path.join(output,'.nojekyll'),'');
await writeFile(path.join(output,'404.html'),'<!doctype html><html lang="ko"><meta charset="utf-8"><title>보고서를 찾을 수 없습니다</title><h1>보고서를 찾을 수 없습니다</h1><a href="/codex-config/">보고서 목록</a></html>');
console.log('Packaged report site in 리포트/.site; configuration, tools, migration manifests and original backups are excluded.');
