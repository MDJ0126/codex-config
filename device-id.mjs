import fs from'node:fs';import path from'node:path';import os from'node:os';import crypto from'node:crypto';
export function deviceId(){const root=path.join(process.env.CODEX_HOME||path.join(os.homedir(),'.codex'),'line-bridge');fs.mkdirSync(root,{recursive:true});const file=path.join(root,'device-id.txt');if(!fs.existsSync(file)){try{fs.writeFileSync(file,crypto.randomUUID(),{flag:'wx'});}catch(e){if(e.code!=='EEXIST')throw e;}}return fs.readFileSync(file,'utf8').trim();}
if(process.argv[1]?.endsWith('device-id.mjs'))console.log(deviceId());
