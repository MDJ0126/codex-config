import fs from 'node:fs';
import path from 'node:path';
import {spawnSync} from 'node:child_process';
import {fileURLToPath} from 'node:url';
import {validateObject} from './contract.mjs';

const root=path.resolve(path.dirname(fileURLToPath(import.meta.url)),'../..');
const entry={command:process.execPath,args:[path.join(root,'Tools/AgentBridge/mcp.mjs')]};
const client=process.argv[2]||'print';
if(client==='print') {
  process.stdout.write(JSON.stringify({mcpServers:{fantasya:entry}},null,2)+'\n');
} else if(client==='cursor' || client==='claude') {
  const file=client==='cursor'?path.join(root,'.cursor/mcp.json'):path.join(root,'.mcp.json');
  const original=fs.existsSync(file)?fs.readFileSync(file,'utf8'):null;
  const config=original?JSON.parse(original):{};validateObject(config);
  if(config.mcpServers && (typeof config.mcpServers!=='object' || Array.isArray(config.mcpServers))) throw new Error('Invalid existing mcpServers; unchanged.');
  config.mcpServers ||= {};
  if(config.mcpServers.fantasya && JSON.stringify(config.mcpServers.fantasya)!==JSON.stringify(entry)) throw new Error('A different Fantasya entry already exists; unchanged.');
  config.mcpServers.fantasya=entry;
  fs.mkdirSync(path.dirname(file),{recursive:true});
  if(original && !fs.existsSync(file+'.before-fantasya')) fs.writeFileSync(file+'.before-fantasya',original);
  const temporary=file+'.fantasya-tmp';fs.writeFileSync(temporary,JSON.stringify(config,null,2)+'\n');fs.renameSync(temporary,file);
  process.stdout.write(client+' project MCP entry configured; other entries preserved.\n');
} else if(client==='codex') {
  const existing=spawnSync('codex',['mcp','get','fantasya','--json'],{encoding:'utf8',windowsHide:true});
  if(existing.status===0) {
    const result=JSON.parse(existing.stdout);
    const text=JSON.stringify(result);
    if(!text.includes(path.join(root,'Tools/AgentBridge/mcp.mjs').replaceAll('\\','\\\\')) && !text.includes(path.join(root,'Tools/AgentBridge/mcp.mjs'))) throw new Error('A different Codex Fantasya entry exists; unchanged.');
    process.stdout.write('Codex Fantasya MCP entry already configured.\n');
  } else {
    const result=spawnSync('codex',['mcp','add','fantasya','--',entry.command,...entry.args],{encoding:'utf8',windowsHide:true});
    if(result.error) throw result.error;
    if(result.status!==0) throw new Error(result.stderr||'Codex MCP configuration failed.');
    process.stdout.write(result.stdout);
  }
} else {
  throw new Error('Choose codex, claude, cursor, or print.');
}
