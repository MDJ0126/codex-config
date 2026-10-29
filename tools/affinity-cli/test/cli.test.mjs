import test from 'node:test';
import assert from 'node:assert/strict';
import { spawn } from 'node:child_process';
import { fileURLToPath } from 'node:url';
import { mkdtemp, writeFile, readFile, rm } from 'node:fs/promises';
import path from 'node:path';
import { tmpdir } from 'node:os';
import { mockServer, png } from './mock-server.mjs';

const cli=fileURLToPath(new URL('../bin/affinity.mjs',import.meta.url));
function run(args) {
  return new Promise((resolve,reject)=>{
    const child=spawn(process.execPath,[cli,...args],{windowsHide:true,stdio:['ignore','pipe','pipe']});
    let out='',err='';child.stdout.on('data',c=>out+=c);child.stderr.on('data',c=>err+=c);child.on('error',reject);
    child.on('close',code=>{try{resolve({code,result:JSON.parse(out),stderr:err});}catch(error){reject(new Error(`${error.message}: ${out} ${err}`));}});
  });
}
test('CLI help/version and malformed flags produce structured results', async () => {
  assert.equal((await run(['version'])).result.version,'1.0.0');
  assert.match((await run(['help'])).result.help,/Affinity CLI/);
  const invalid=await run(['job','--missing']);assert.equal(invalid.code,2);assert.equal(invalid.result.ok,false);
  assert.equal((await run(['status','--dry-run'])).code,2);
});
test('CLI reads a BOM JSON job, dry-runs without a server and refuses output overwrite', async () => {
  const dir=await mkdtemp(path.join(tmpdir(),'affinity-cli-'));
  try {
    const job=path.join(dir,'job.json'),out=path.join(dir,'result.json');
    await writeFile(job,'\ufeff'+JSON.stringify({version:1,document:{create:{width:100,height:100}},steps:[]}));
    const result=await run(['job',job,'--dry-run','--out',out]);assert.equal(result.code,0);assert.equal(result.result.dryRun,true);
    assert.equal(JSON.parse(await readFile(out,'utf8')).ok,true);
    assert.equal((await run(['job',job,'--dry-run','--out',out])).code,2);
    const server=await mockServer();
    try {
      assert.equal((await run(['status','--out',out,'--endpoint',server.endpoint])).code,2);
      assert.equal(server.calls.length,0,'existing --out must prevent all remote actions');
    } finally {await server.close();}
  } finally {await rm(dir,{recursive:true,force:true});}
});
test('CLI doctor/status/render exercise a real loopback transport', async () => {
  const server=await mockServer();const dir=await mkdtemp(path.join(tmpdir(),'affinity-render-'));
  try {
    const common=['--endpoint',server.endpoint,'--timeout','1500'];
    assert.equal((await run(['doctor',...common])).result.connected,true);
    assert.equal((await run(['status',...common])).result.application.version,'3.3.1');
    const render=await run(['render','spread','--args','{"spread_index":0}','--artifacts',dir,...common]);
    assert.equal(render.code,0);assert.equal(render.result.artifacts[0].width,1);
    assert.equal((await run(['verify',render.result.artifacts[0].path,'--width','2'])).code,4);
    const verified=await run(['verify',render.result.artifacts[0].path,'--width','1','--height','1']);
    assert.equal(verified.code,0);assert.equal(verified.result.bytes,png.length);
  } finally {await server.close();await rm(dir,{recursive:true,force:true});}
});
