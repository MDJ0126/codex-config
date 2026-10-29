import http from 'node:http';
import { once } from 'node:events';

export const png = Buffer.from('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+aU1kAAAAASUVORK5CYII=', 'base64');
export async function mockServer({ transport = 'sse', remoteError = false, hang = false, drop = false, badEndpoint = false } = {}) {
  let stream;
  const calls = [], sessions = new Map();
  const tools = [
    { name:'read_sdk_documentation_topic', inputSchema:{ type:'object', properties:{ filename:{type:'string'} }, required:['filename'], additionalProperties:false } },
    { name:'execute_script', inputSchema:{ type:'object', properties:{ script:{type:'string'} }, required:['script'], additionalProperties:false } },
    { name:'render_spread', inputSchema:{ type:'object', properties:{ spread_index:{type:'integer',minimum:0} }, required:['spread_index'], additionalProperties:false } }
  ];
  const server = http.createServer(async (req,res) => {
    if (req.method === 'GET' && req.url === '/sse') {
      res.writeHead(200, {'Content-Type':'text/event-stream'});
      stream = res;
      res.write(`: hello\r\nevent: endpoint\r\ndata: ${badEndpoint ? 'http://example.com/messages' : '/messages?session=local'}\r\n\r\n`);
      return;
    }
    if (req.method !== 'POST') { res.writeHead(404).end(); return; }
    let raw = '';
    for await (const chunk of req) raw += chunk;
    const message = JSON.parse(raw);
    calls.push(message);
    if (message.id === undefined) { res.writeHead(202).end(); return; }
    let result, error;
    if (message.method === 'initialize') {
      result = { protocolVersion:'2024-11-05', serverInfo:{name:'test-affinity',version:'3.3.1'}, capabilities:{tools:{}} };
      sessions.set('test-session',true);
    } else if (message.method === 'tools/list') result = { tools };
    else if (message.method === 'tools/call') {
      const { name, arguments:args } = message.params;
      if (hang) { if (transport === 'sse') res.writeHead(202).end(); return; }
      if (drop) { stream?.end(); res.writeHead(202).end(); return; }
      if (remoteError) result = { isError:true, content:[{type:'text',text:'test failure'}] };
      else if (name === 'read_sdk_documentation_topic') result = {content:[{type:'text',text:`Documentation: ${args.filename}`}]};
      else if (name === 'render_spread') result = {content:[{type:'image',mimeType:'image/png',data:png.toString('base64')}]};
      else if (name === 'execute_script') {
        const preamble = calls.some(c=>c.params?.name==='read_sdk_documentation_topic' && c.params.arguments.filename==='preamble');
        result = preamble ? {content:[{type:'text',text:'__AFFINITY_CLI_RESULT__'+JSON.stringify({ok:true,application:{version:'3.3.1'},documents:[]})}]}
          : {isError:true,content:[{type:'text',text:'preamble required'}]};
      }
    } else error = {code:-32601,message:'Unknown method'};
    const response = {jsonrpc:'2.0',id:message.id,...(error ? {error}:{result})};
    if (transport === 'sse') {
      res.writeHead(202).end();
      stream.write('event: message\ndata: '+JSON.stringify(response)+'\n\n');
    } else {
      // Alternate JSON and streaming response formats, as allowed by MCP.
      if (message.method === 'tools/list') res.writeHead(200,{'Content-Type':'text/event-stream','Mcp-Session-Id':'test-session'}).end('data: '+JSON.stringify(response)+'\n\n');
      else res.writeHead(200,{'Content-Type':'application/json','Mcp-Session-Id':'test-session'}).end(JSON.stringify(response));
    }
  });
  server.listen(0,'127.0.0.1'); await once(server,'listening');
  const endpoint = `http://127.0.0.1:${server.address().port}/${transport === 'sse' ? 'sse' : 'mcp'}`;
  return { endpoint, calls, async close() { stream?.end(); server.closeAllConnections(); await new Promise(r=>server.close(r)); } };
}
