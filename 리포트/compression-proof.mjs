import {readFile} from 'node:fs/promises';
import {createHash} from 'node:crypto';
import path from 'node:path';
import assert from 'node:assert/strict';
export const imageHash=b=>createHash('sha256').update(b).digest('hex');
export async function compressionProof(root){
 let manifest;try{manifest=JSON.parse(await readFile(path.join(root,'migration/image-compression-2026-10-30.json'),'utf8'));}catch(e){if(e.code==='ENOENT')return new Map();throw e;}
 assert.equal(manifest.errors.length,0,'Image compression has unresolved failures');
 const map=new Map();
 for(const entry of manifest.entries){
  assert.ok(!map.has(entry.source),'Duplicate compression entry');
  for(const name of ['source','backup']){const absolute=path.resolve(root,entry[name]);assert.ok(absolute.startsWith(path.resolve(root)+path.sep),'Unsafe image path');}
  assert.match(entry.backup,/(?:^|\/)originals\/image-compression\//,'Image original must be preserved outside Pages');
  const before=await readFile(path.join(root,entry.backup)),after=await readFile(path.join(root,entry.source));
  assert.equal(imageHash(before),entry.beforeSha256,entry.backup);assert.equal(imageHash(after),entry.afterSha256,entry.source);
  assert.equal(before.length,entry.beforeBytes);assert.equal(after.length,entry.afterBytes);assert.ok(after.length<before.length);
  map.set(entry.source,entry);
 }
 return map;
}
