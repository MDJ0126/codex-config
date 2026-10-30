import {readFile,writeFile,mkdir} from 'node:fs/promises';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import {validateReport,toMarkdown} from './assets/report-contract.mjs';
import {validateManualLinks} from './assets/manual-contract.mjs';
import {readCatalog} from './discussion-data.mjs';
const root=path.dirname(fileURLToPath(import.meta.url)),args=process.argv.slice(2),arg=name=>args[args.indexOf(name)+1];
if(!args.includes('--from')||!args.includes('--input'))throw new Error('Usage: node 리포트/update-manual.mjs --from projects/<project>/posts/<file>.md --input updated.json [--dry-run]');
const catalog=await readCatalog(root,JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'))),previous=catalog.find(p=>p.postPath===arg('--from')||p.reportPath===arg('--from'));if(previous?.kind!=='manual')throw new Error('갱신할 기존 메뉴얼을 선택하세요.');
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8')),input=JSON.parse(await readFile(path.resolve(arg('--input')),'utf8'));
if(input.project!==previous.project||input.manualId!==previous.manual_id)throw new Error('같은 프로젝트·메뉴얼 ID를 유지하세요.');
const latest=catalog.filter(p=>p.project===previous.project&&p.manual_id===previous.manual_id).sort((a,b)=>Date.parse(b.date)-Date.parse(a.date))[0];if(latest.reportPath!==previous.reportPath)throw new Error('최신 버전에서 갱신하세요. 이전 원본을 덮어쓰지 않습니다.');
input.previousManual=previous.reportPath;const report=validateReport(input,projects);validateManualLinks(report,catalog);
const date=new Date().toISOString(),patch=Number(previous.version.match(/^v1\.0\.(\d+)$/)?.[1]??0)+1,version=`v1.0.${patch}`,file=`${date.slice(0,10)}-manual-${report.manualId}-${date.replace(/\D/g,'')}-${version}.md`,post=`projects/${report.project}/posts/${file}`,payloadPath=`../assets/submissions/${file.replace(/\.md$/,'.json')}`,markdown=toMarkdown(report,{date,version,payloadPath});
if(args.includes('--dry-run'))console.log(markdown);else{const payload=path.resolve(root,path.dirname(post),payloadPath);await mkdir(path.dirname(payload),{recursive:true});await writeFile(payload,JSON.stringify({report},null,2)+'\n',{flag:'wx'});await writeFile(path.join(root,post),markdown,{flag:'wx'});console.log(path.join(root,post));}
