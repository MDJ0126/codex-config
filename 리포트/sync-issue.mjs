import {readFile,writeFile,mkdir} from 'node:fs/promises';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import {validateReport,parseIssueBody,toMarkdown} from './assets/report-contract.mjs';
import {validateDiscussionLinks} from './assets/discussion-contract.mjs';
import {readCatalog} from './discussion-data.mjs';
import {validateManualLinks} from './assets/manual-contract.mjs';
const root=path.dirname(fileURLToPath(import.meta.url));
const event=JSON.parse(await readFile(process.env.GITHUB_EVENT_PATH,'utf8'));
const issue=event.issue;if(!issue||!Number.isSafeInteger(issue.number))throw new Error('Invalid issue event');
const raw=parseIssueBody(issue.body||'');if(!raw){console.log('Not a Tracefolio report; no files changed.');process.exit(0);}
const projects=JSON.parse(await readFile(path.join(root,'projects.json'),'utf8'));
if(raw.kind==='bug'){if(event.action==='closed')raw.workflow='closed';else if(event.action==='reopened')raw.workflow='review';}
const report=validateReport(raw,projects),number=issue.number;
if(report.kind==='manual'){try{const old=JSON.parse(await readFile(path.join(root,'projects',report.project,'assets/issues',`TF-${number}.json`),'utf8'));report.previousManual=old.post.replace('/posts/','/reports/').replace(/\.md$/,'.html');}catch(e){if(e.code!=='ENOENT')throw e;}}
const catalog=await readCatalog(root,projects);validateDiscussionLinks(report,catalog);validateManualLinks(report,catalog);
const version=`v1.0.${Number(process.env.GITHUB_RUN_NUMBER||1)}`;
if(!/^v1\.0\.\d+$/.test(version))throw new Error('Invalid revision');
const date=issue.updated_at;if(!Number.isFinite(Date.parse(date)))throw new Error('Invalid issue date');
const destination=path.join(root,'projects',report.project,'assets/issues',`TF-${number}.json`);
let previous;try{previous=JSON.parse(await readFile(destination,'utf8'));}catch(e){if(e.code!=='ENOENT')throw e;}
if(previous&&Date.parse(previous.updatedAt)>=Date.parse(date)){console.log('Already synchronized or stale event; no files changed.');process.exit(0);}
const history=[...(previous?.history||[]),{at:date,action:event.action,stage:report.workflow||report.discussionState||report.manualState||report.state,actor:event.sender?.login||issue.user?.login||'unknown'}];
const canonicalURL=`https://github.com/MDJ0126/codex-config/issues/${number}`;
const post=path.join(root,'projects',report.project,'posts',`${date.slice(0,10)}-TF-${number}-${version}.md`);
const payloadPath=`../assets/issues/TF-${number}-${version}.json`;
await mkdir(path.dirname(destination),{recursive:true});await mkdir(path.dirname(post),{recursive:true});
await writeFile(destination,JSON.stringify({report,issue:number,url:canonicalURL,updatedAt:date,history,post:path.relative(root,post).split(path.sep).join('/')},null,2)+'\n');
await writeFile(path.resolve(path.dirname(post),payloadPath),JSON.stringify({report},null,2)+'\n',{flag:'wx'});
await writeFile(post,toMarkdown(report,{date,version,issueNumber:number,issueURL:canonicalURL,payloadPath}),{flag:'wx'});
console.log(`Synchronized TF-${number} to project ${report.project}; revision ${version}.`);
