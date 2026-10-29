import test from 'node:test';
import assert from 'node:assert/strict';
import {mkdtemp,mkdir,writeFile,rm} from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import {compressionProof,imageHash} from '../compression-proof.mjs';
test('optimized bytes and preserved original are both checked; tampering is rejected',async()=>{
 const root=await mkdtemp(path.join(os.tmpdir(),'tracefolio-images-'));
 try{
  await mkdir(path.join(root,'migration'));await mkdir(path.join(root,'originals/image-compression'),{recursive:true});await mkdir(path.join(root,'assets'));
  const before=Buffer.from('original-image-bytes'),after=Buffer.from('packed');
  const entry={source:'assets/image.png',backup:'originals/image-compression/image.png',beforeSha256:imageHash(before),afterSha256:imageHash(after),beforeBytes:before.length,afterBytes:after.length};
  await writeFile(path.join(root,entry.backup),before);await writeFile(path.join(root,entry.source),after);
  const manifest=path.join(root,'migration/image-compression-2026-10-30.json');await writeFile(manifest,JSON.stringify({entries:[entry],errors:[]}));
  assert.equal((await compressionProof(root)).size,1);
  await writeFile(path.join(root,entry.source),'tampered');await assert.rejects(compressionProof(root));
  await writeFile(path.join(root,entry.source),after);await writeFile(path.join(root,entry.backup),'changed');await assert.rejects(compressionProof(root));
 }finally{await rm(root,{recursive:true,force:true});}
});
