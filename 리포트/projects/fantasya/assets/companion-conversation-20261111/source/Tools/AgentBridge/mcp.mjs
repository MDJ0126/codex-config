import readline from 'node:readline';
import crypto from 'node:crypto';
import {TOOLS} from './contract.mjs';
import {call,connect} from './connection.mjs';
const clientId=crypto.randomUUID();
const versions=['2025-11-25','2025-06-18','2025-03-26','2024-11-05'];
const input=readline.createInterface({input:process.stdin,crlfDelay:Infinity});
const send=value=>process.stdout.write(JSON.stringify(value)+'\n');
let initialized=false;
input.on('line',async line=>{
  if(!line.trim()) return;
  let request;
  try {if(Buffer.byteLength(line)>65536) throw new Error('oversized');request=JSON.parse(line);}catch{send({jsonrpc:'2.0',id:null,error:{code:-32700,message:'Invalid JSON-RPC message.'}});return;}
  if(request.id===undefined) return;
  try {
    if(request.jsonrpc!=='2.0') throw new Error('Invalid JSON-RPC version.');
    let result;
    if(request.method==='initialize') {
      initialized=true;
      result={protocolVersion:versions.includes(request.params?.protocolVersion)?request.params.protocolVersion:'2025-06-18',capabilities:{tools:{listChanged:false}},serverInfo:{name:'fantasya-companions',version:'0.1.0'},instructions:'Fantasya companions share persistent identity and memory between game and external chat. Observe before acting; claim before speaking as or controlling a companion. Player promises override autonomous choices. Queued commands require Unity receipts. Never invent completed game events.'};
    } else if(!initialized) throw new Error('Initialize the MCP connection first.');
    else if(request.method==='ping') result={};
    else if(request.method==='tools/list') result={tools:TOOLS};
    else if(request.method==='tools/call') {
      try {
        const connection=await connect();
        const value=await call(request.params.name,request.params.arguments||{},clientId,connection);
        result={content:[{type:'text',text:JSON.stringify(value)}],isError:false};
      } catch(error) {result={content:[{type:'text',text:error.message}],isError:true};}
    } else {send({jsonrpc:'2.0',id:request.id,error:{code:-32601,message:'Method not found.'}});return;}
    send({jsonrpc:'2.0',id:request.id,result});
  } catch(error){send({jsonrpc:'2.0',id:request.id,error:{code:-32602,message:error.message}});}
});
