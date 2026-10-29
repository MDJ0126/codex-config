import test from 'node:test';
import assert from 'node:assert/strict';
import { McpClient, localUrl, readSse } from '../src/transport.mjs';
import { Affinity } from '../src/affinity.mjs';
import { mockServer } from './mock-server.mjs';

for (const transport of ['sse','http']) test(`${transport}: handshake, discovery, preamble and script execution`, async () => {
  const server = await mockServer({transport});
  const client = new McpClient({endpoint:server.endpoint,transport,timeout:1500});
  try {
    assert.equal((await client.connect()).serverInfo.version,'3.3.1');
    const affinity = new Affinity(client);
    assert.equal((await affinity.tools()).length,3);
    await affinity.script('console.log("안녕하세요");');
    await affinity.script('console.log("again");');
    assert.equal(server.calls.filter(c=>c.params?.name==='read_sdk_documentation_topic').length,1);
    const names=server.calls.filter(c=>c.method==='tools/call').map(c=>c.params.name);
    assert.deepEqual(names,['read_sdk_documentation_topic','execute_script','execute_script']);
    if (transport === 'http') assert.equal(client.session,'test-session');
  } finally { await client.close(); await server.close(); }
});

test('rejects malformed arguments before calling the server', async () => {
  const server = await mockServer();
  const client = new McpClient({endpoint:server.endpoint,timeout:1500});
  try {
    await client.connect(); const affinity = new Affinity(client);
    await assert.rejects(affinity.call('render_spread',{spread_index:-1}),{code:'INPUT'});
    await assert.rejects(affinity.call('render_spread',{invented:0}),{code:'INPUT'});
    assert.equal(server.calls.filter(c=>c.method==='tools/call').length,0);
  } finally { await client.close(); await server.close(); }
});

test('timeout never retries a tool and reports unknown outcome', async () => {
  const server = await mockServer({hang:true});
  const client = new McpClient({endpoint:server.endpoint,timeout:150});
  try {
    await client.connect();
    await assert.rejects(client.request('tools/call',{name:'modify',arguments:{}}),error=>error.code==='TIMEOUT' && error.details.outcome.includes('unknown'));
    assert.equal(server.calls.filter(c=>c.method==='tools/call').length,1);
  } finally { await client.close(); await server.close(); }
});

test('connection loss rejects an in-flight call without retry', async () => {
  const server = await mockServer({drop:true});
  const client = new McpClient({endpoint:server.endpoint,timeout:500});
  try {
    await client.connect();
    await assert.rejects(client.request('tools/call',{}),{code:'CONNECTION'});
    assert.equal(server.calls.filter(c=>c.method==='tools/call').length,1);
  } finally { await client.close(); await server.close(); }
});

test('native tool errors are not reported as success', async () => {
  const server = await mockServer({remoteError:true});
  const client = new McpClient({endpoint:server.endpoint,timeout:500});
  try { await client.connect(); await assert.rejects(new Affinity(client).call('render_spread',{spread_index:0}),{code:'REMOTE'}); }
  finally { await client.close(); await server.close(); }
});

test('remote and cross-origin endpoints are refused', async () => {
  for (const url of ['https://localhost:6767/sse','http://example.com/sse','http://user:pass@localhost/sse','file:///tmp/foo']) assert.throws(()=>localUrl(url),{code:'CONFIG'});
  const server=await mockServer({badEndpoint:true}); const client=new McpClient({endpoint:server.endpoint,timeout:500});
  try { await assert.rejects(client.connect(),{code:'CONFIG'}); }
  finally { await client.close(); await server.close(); }
});

test('SSE parser handles split UTF-8, CRLF, comments and multiline events', async () => {
  const bytes=new TextEncoder().encode(': comment\r\nevent: message\r\ndata: 안녕\r\ndata: world\r\n\r\n');
  const stream=new ReadableStream({start(controller){for(const byte of bytes)controller.enqueue(Uint8Array.of(byte));controller.close();}});
  const messages=[]; await readSse(stream,event=>messages.push(event));
  assert.deepEqual(messages,[{event:'message',data:'안녕\nworld'}]);
});

test('SSE oversized events fail rather than consuming unbounded memory', async () => {
  const stream=new ReadableStream({start(c){c.enqueue(new TextEncoder().encode('data: too long'));c.close();}});
  await assert.rejects(readSse(stream,()=>{},4),{code:'PROTOCOL'});
});
