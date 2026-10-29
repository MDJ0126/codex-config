import {readFile,writeFile,mkdir} from 'node:fs/promises';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import {validateReport,toMarkdown} from './assets/report-contract.mjs';
const root=path.dirname(fileURLToPath(import.meta.url));
const args=process.argv.slice(2),index=args.indexOf('--input');if(index<0||!args[index+1])throw new Error('Usage: node 리포트/create-report.mjs --input report.json [--dry-run]');
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
const report=validateReport(JSON.parse(await readFile(path.resolve(args[index+1]),'utf8')),projects);
const date=new Date().toISOString(),file=`${date.slice(0,10)}-${report.kind}-${date.replace(/\D/g,'')}-v1.0.0.md`;
const destination=path.join(root,'projects',report.project,'posts',file),payloadPath=`../assets/submissions/${file.replace(/\.md$/,'.json')}`,markdown=toMarkdown(report,{date,payloadPath});
if(args.includes('--dry-run'))console.log(markdown);else{const payload=path.resolve(path.dirname(destination),payloadPath);await mkdir(path.dirname(payload),{recursive:true});await writeFile(payload,JSON.stringify({report},null,2)+'\n',{flag:'wx'});await mkdir(path.dirname(destination),{recursive:true});await writeFile(destination,markdown,{flag:'wx'});console.log(destination);}
