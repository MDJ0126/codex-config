import net from 'node:net';
import crypto from 'node:crypto';

// Wire format verified against the installed codex-app-tools server.mjs:
// four-byte little-endian payload length, followed by a JSON-RPC 2.0 frame.
export function appRequest(pipePath,method,params,timeoutMs=30000){
 return new Promise((resolve,reject)=>{
  const socket=net.createConnection(pipePath);let buffer=Buffer.alloc(0),settled=false;
  const finish=(err,value)=>{if(settled)return;settled=true;clearTimeout(timer);socket.destroy();err?reject(err):resolve(value);};
  const timer=setTimeout(()=>finish(Error('APP_REQUEST_TIMEOUT')),timeoutMs);
  socket.once('error',e=>finish(e));socket.once('end',()=>finish(Error('APP_PIPE_CLOSED')));
  socket.once('connect',()=>{const payload=Buffer.from(JSON.stringify({jsonrpc:'2.0',id:1,method,params}));const header=Buffer.alloc(4);header.writeUInt32LE(payload.length);socket.write(Buffer.concat([header,payload]));});
  socket.on('data',chunk=>{
   buffer=Buffer.concat([buffer,chunk]);if(buffer.length<4)return;const len=buffer.readUInt32LE(0);
   if(len>8*1024*1024){finish(Error('APP_FRAME_TOO_LARGE'));return;}if(buffer.length<len+4)return;
   try{const r=JSON.parse(buffer.subarray(4,len+4).toString());r.error?finish(Error(r.error.message)):finish(null,r.result);}catch(e){finish(e);}
  });
 });
}
export async function appTool(pipePath,callerThreadId,name,args){
 const catalog=await appRequest(pipePath,'tools/list',{threadStartKind:'all'});
 const tool=catalog.tools.find(t=>t.name===name);if(!tool)throw Error('APP_TOOL_UNAVAILABLE');
 const result=await appRequest(pipePath,'tools/call',{arguments:args,callerSource:'codex',callId:'line-'+crypto.randomUUID(),hostId:'local',namespace:tool.namespace,threadId:callerThreadId,tool:tool.name,turnId:'line-'+crypto.randomUUID()});
 if(!result.success)throw Error('APP_TOOL_FAILED');
 return result.contentItems.filter(i=>i.type==='inputText').map(i=>i.text).join('\n');
}
