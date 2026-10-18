import fs from'node:fs';import path from'node:path';import os from'node:os';import{fileURLToPath}from'node:url';import{appTool}from'./app-client.mjs';
function normalized(value){let p=value||'';if(p.startsWith('\\\\?\\'))p=p.slice(4);return path.win32.normalize(p).replace(/[\\/]+$/,'').toLowerCase();}
export function projectFor(thread,projects){
 if(thread.projectId){const p=projects.find(p=>p.projectId===thread.projectId);return p?.label||'프로젝트명 미확인';}
 const cwd=normalized(thread.cwd);if(!cwd)return null;
 const matches=projects.filter(p=>p.projectKind==='local'&&p.path&&(!p.hostId||p.hostId==='local')).filter(p=>{const root=normalized(p.path);return cwd===root||cwd.startsWith(root+'\\');}).sort((a,b)=>normalized(b.path).length-normalized(a.path).length);
 return matches[0]?.label||null;
}
async function main(){
 const home=process.env.CODEX_HOME||path.join(os.homedir(),'.codex');let context={pipePath:process.env.CODEX_APP_TOOLS_PIPE_PATH,callerThreadId:process.env.CODEX_THREAD_ID};
 if(!context.pipePath)context=JSON.parse(fs.readFileSync(path.join(home,'line-bridge','app-context.json'),'utf8').replace(/^\uFEFF/,''));
 const id=process.argv[2];if(!id||!context.pipePath)throw Error('Metadata connection unavailable');
 const threadResult=JSON.parse(await appTool(context.pipePath,context.callerThreadId,'read_thread',{threadId:id,turnLimit:1,includeOutputs:false}));
 const projectResult=JSON.parse(await appTool(context.pipePath,context.callerThreadId,'list_projects',{}));
 const listed=JSON.parse(await appTool(context.pipePath,context.callerThreadId,'list_threads',{limit:200}));
 const summary=[...(listed.threads||[]),...(listed.pinnedThreads||[])].find(t=>t.id===id);
 const thread={...threadResult.thread,...summary};
 console.log(JSON.stringify({title:thread.title,projectName:projectFor(thread,projectResult.projects||[]),resolved:true}));
}
if(process.argv[1]&&path.resolve(process.argv[1])===fileURLToPath(import.meta.url))main().catch(()=>{console.error('THREAD_METADATA_UNAVAILABLE');process.exitCode=1;});
