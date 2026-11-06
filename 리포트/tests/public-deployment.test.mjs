import test from 'node:test';
import assert from 'node:assert/strict';
import {verifyResponse} from '../verify-public.mjs';
const response=(status,text)=>({ok:status===200,status,arrayBuffer:async()=>Buffer.from(text)});
test('public verification accepts exact packaged bytes',async()=>{assert.equal((await verifyResponse('https://example.test/',Buffer.from('expected'),{request:async()=>response(200,'expected'),attempts:1})).toString(),'expected');});
test('HTTP 200 with stale content fails',async()=>{await assert.rejects(verifyResponse('https://example.test/',Buffer.from('expected'),{request:async()=>response(200,'stale'),attempts:1}),/differ/);});
test('404 is never reported as published',async()=>{await assert.rejects(verifyResponse('https://example.test/',Buffer.from('expected'),{request:async()=>response(404,'missing'),attempts:1}),/HTTP 404/);});
test('propagation delay retries and then requires exact bytes',async()=>{let called=0;await verifyResponse('https://example.test/',Buffer.from('expected'),{request:async()=>++called===1?response(404,'missing'):response(200,'expected'),attempts:2,delay:0});assert.equal(called,2);});
