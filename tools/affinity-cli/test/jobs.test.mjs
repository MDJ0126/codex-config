import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, writeFile, rm } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import path from 'node:path';
import vm from 'node:vm';
import { prepareJob, scriptForJob, parseJobResult, affinityRuntime } from '../src/jobs.mjs';
import { validate } from '../src/schema.mjs';
import { imageInfo, saveImages, inspectFile } from '../src/files.mjs';
import { png } from './mock-server.mjs';

const base = () => ({version:1,document:{create:{width:800,height:600}},steps:[]});
test('job rejects unknown operations, invalid geometry and ambiguous document targets', async () => {
  await assert.rejects(prepareJob({...base(),steps:[{op:'delete-all'}]}),{code:'INPUT'});
  await assert.rejects(prepareJob({...base(),document:{create:{width:0,height:600}}}),{code:'INPUT'});
  await assert.rejects(prepareJob({...base(),document:{id:'x',open:'x'}}),{code:'INPUT'});
  await assert.rejects(prepareJob({...base(),steps:[{op:'text',text:'x',x:0,y:0,width:5,size:12,font:'X',fill:'#000000'}]}),{code:'INPUT'});
});

test('output preflight preserves existing files and resolves paths relative to the job', async () => {
  const dir=await mkdtemp(path.join(tmpdir(),'affinity-job-'));
  try {
    await writeFile(path.join(dir,'exists.af'),'original');
    await assert.rejects(prepareJob({...base(),steps:[{op:'save',path:'exists.af'}]},dir),{code:'INPUT'});
    const job=await prepareJob({...base(),steps:[{op:'save',path:'exists.af',overwrite:true}]},dir);
    assert.equal(job.steps[0].path,path.join(dir,'exists.af'));
    await assert.rejects(prepareJob({...base(),steps:[{op:'save',path:'new.af'},{op:'save',path:'new.af'}]},dir),{code:'INPUT'});
    await assert.rejects(prepareJob({...base(),steps:[{op:'image',x:0,y:0,width:10,height:10,path:'missing.png'}]},dir),{code:'INPUT'});
  } finally { await rm(dir,{recursive:true,force:true}); }
});

test('hostile text remains data when compiling a native job', () => {
  const job={...base(),steps:[{op:'text',x:0,y:0,text:'\");globalThis.injected=true;//\u2028한글',size:24,font:'Test',fill:'#123456'}]};
  let log;
  const context={console:{log:x=>{log=x;}},require:()=>{throw new Error('SDK unavailable');}};
  // Replace the runtime function with an observer, not a fake design renderer.
  const script=scriptForJob(job).replace(affinityRuntime.toString(),'function(data){ return {ok:true,text:data.steps[0].text}; }');
  vm.runInNewContext(script,context);
  assert.equal(context.injected,undefined);
  assert.equal(parseJobResult({content:[{type:'text',text:log}]}).text,job.steps[0].text);
});

test('partial job errors and absent result markers cannot be passed as success', () => {
  assert.throws(()=>parseJobResult({content:[{type:'text',text:'normal log'}]}),{code:'PROTOCOL'});
  assert.throws(()=>parseJobResult({content:[{type:'text',text:'__AFFINITY_CLI_RESULT__{"ok":false,"error":"save failed","completed":[{"op":"shape"}]}'}]}),error=>error.code==='JOB' && error.details.completed.length===1);
  assert.equal(parseJobResult({content:[{type:'text',text:'log\n__AFFINITY_CLI_RESULT__{"ok":true,"text":"brace } and \\\" quote"}\nend'}]}).ok,true);
  assert.equal(parseJobResult({content:[{type:'text',text:JSON.stringify({logs:['__AFFINITY_CLI_RESULT__{"ok":true,"text":"한글"}']})}]}).text,'한글');
});

test('PNG artifacts are saved, hashed, verified and removed from JSON base64 output', async () => {
  const dir=await mkdtemp(path.join(tmpdir(),'affinity-image-'));
  try {
    assert.equal(imageInfo(png).width,1);
    const raw={content:[{type:'image',mimeType:'image/png',data:png.toString('base64')}]};
    const saved=await saveImages(raw,dir);
    assert.equal(saved.artifacts.length,1);
    assert.equal(saved.result.content[0].data,undefined);
    assert.equal((await inspectFile(saved.artifacts[0].path)).sha256,saved.artifacts[0].sha256);
    const second=await saveImages(raw,dir);
    assert.notEqual(second.artifacts[0].path,saved.artifacts[0].path);
    const omitted=await saveImages(raw);
    assert.match(omitted.result.content[0].data,/omitted/);
  } finally { await rm(dir,{recursive:true,force:true}); }
});

test('schema checks nested tool arguments and alternatives', () => {
  assert.throws(()=>validate({type:'object',required:['n'],properties:{n:{type:'integer',minimum:0}}},{n:0.5}),{code:'INPUT'});
  assert.throws(()=>validate({oneOf:[{type:'number'},{type:'integer'}]},1),{code:'INPUT'});
  validate({anyOf:[{type:'string'},{type:'null'}]},null);
});
