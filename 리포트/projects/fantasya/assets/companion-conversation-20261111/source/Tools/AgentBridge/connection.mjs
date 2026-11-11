import fs from 'node:fs';
import path from 'node:path';
import {spawn} from 'node:child_process';
import crypto from 'node:crypto';
import {fileURLToPath} from 'node:url';

export const directory=path.resolve(process.env.FANTASYA_AGENT_DATA_DIR||path.join(path.dirname(fileURLToPath(import.meta.url)),'../../.fantasya'));
export async function readConnection() {
  const connection=JSON.parse(fs.readFileSync(path.join(directory,'connection.json'),'utf8'));
  const url=new URL(connection.url);
  if(url.protocol!=='http:' || url.hostname!=='127.0.0.1' || url.pathname!=='/' || !/^[a-f0-9]{64}$/.test(connection.token)) throw new Error('Invalid local bridge connection manifest.');
  return connection;
}
async function health(connection) {
  const result=await fetch(connection.url+'/health',{headers:{Authorization:'Bearer '+connection.token},signal:AbortSignal.timeout(1000)});
  if(!result.ok) throw new Error('Local bridge is not ready.');
}
export async function connect({autostart=true}={}) {
  try {const connection=await readConnection();await health(connection);return connection;}catch(error){if(!autostart) throw error;}
  const child=spawn(process.execPath,[fileURLToPath(new URL('server.mjs',import.meta.url))],{detached:true,windowsHide:true,stdio:'ignore',env:{...process.env,FANTASYA_AGENT_DATA_DIR:directory}});child.unref();
  for(let attempt=0;attempt<40;attempt++) {
    await new Promise(resolve=>setTimeout(resolve,125));
    try {const connection=await readConnection();await health(connection);return connection;}catch{}
  }
  throw new Error('Cannot start local companion bridge. Run node Tools/AgentBridge/server.mjs and inspect its error.');
}
export async function call(tool,args,clientId,connection) {
  connection ||= await connect();
  const result=await fetch(connection.url+'/rpc',{method:'POST',headers:{Authorization:'Bearer '+connection.token,'Content-Type':'application/json','X-Fantasya-Client':clientId},body:JSON.stringify({tool,arguments:args}),signal:AbortSignal.timeout(70000)});
  const body=await result.json();if(!result.ok) throw new Error(body.error||'Bridge request failed.');return body;
}
export function persistentClient(label='codex') {
  if(!/^[a-z0-9_-]{1,40}$/i.test(label)) throw new Error('Invalid client label.');
  fs.mkdirSync(path.join(directory,'clients'),{recursive:true});
  const file=path.join(directory,'clients',label+'.json');
  if(fs.existsSync(file)) return JSON.parse(fs.readFileSync(file,'utf8')).id;
  const id=crypto.randomUUID();fs.writeFileSync(file,JSON.stringify({id}),{mode:0o600});return id;
}
