import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import {fileURLToPath} from 'node:url';
const [action='observe',argumentJson='{}']=process.argv.slice(2);
const root=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'../..');
const directory=path.join(root,'.utmp/Participation');
const id=crypto.randomUUID();
fs.mkdirSync(path.join(directory,'requests'),{recursive:true});
const pending=path.join(directory,'requests',id+'.tmp'),request=path.join(directory,'requests',id+'.json');
fs.writeFileSync(pending,JSON.stringify({...JSON.parse(argumentJson),id,action}));fs.renameSync(pending,request);
const result=path.join(directory,'results',id+'.json');
const started=Date.now();
while(!fs.existsSync(result)) {
  if(Date.now()-started>15000) {
    fs.rmSync(request,{force:true});
    throw new Error('No live isolated participation session. The timed-out request was removed.');
  }
  await new Promise(resolve=>setTimeout(resolve,100));
}
const value=JSON.parse(fs.readFileSync(result,'utf8'));process.stdout.write(JSON.stringify(value,null,2)+'\n');
if(!value.success) process.exitCode=1;
