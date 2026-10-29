import {readFile,access} from 'node:fs/promises';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import {createHash} from 'node:crypto';
import assert from 'node:assert/strict';
const root=path.dirname(fileURLToPath(import.meta.url));
const manifest=JSON.parse(await readFile(path.join(root,'migration/2026-10-29-manifest.json'),'utf8'));
const hash=b=>createHash('sha256').update(b).digest('hex');
let originals=0;
for(const e of manifest.entries){
 assert.equal(hash(await readFile(path.join(root,e.destination))),e.publishedSha256,e.destination);
 if(e.isReport){assert.equal(hash(await readFile(path.join(root,e.originalBackup))),e.sha256,e.originalBackup);await access(path.join(root,e.post));originals++;}
}
console.log(`PASS: ${originals} original report hashes, ${manifest.entries.length} published dependency hashes and imported posts verified. Original test results were not rerun.`);
