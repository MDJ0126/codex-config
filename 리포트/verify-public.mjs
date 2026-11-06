import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import { fileURLToPath } from 'node:url';
const digest=b=>crypto.createHash('sha256').update(b).digest('hex');
export async function verifyResponse(url,expected,{request=fetch,attempts=12,delay=5000}={}){
 let last;
 for(let n=0;n<attempts;n++){
  try{const response=await request(url,{headers:{'cache-control':'no-cache'}});if(!response.ok)throw new Error(`HTTP ${response.status}`);const body=Buffer.from(await response.arrayBuffer());if(digest(body)!==digest(expected))throw new Error('published bytes differ from packaged bytes');return body;}
  catch(error){last=error;if(n+1<attempts)await new Promise(resolve=>setTimeout(resolve,delay));}
 }
 throw new Error(`${url}: ${last.message}`);
}
export async function verifyPublic({root=path.dirname(fileURLToPath(import.meta.url)),base='https://mdj0126.github.io/codex-config/',request=fetch}={}){
 const site=path.join(root,'.site'),catalog=JSON.parse(await fs.readFile(path.join(root,'assets/report-catalog.json'),'utf8'));
 const reports=catalog.slice(0,5).map(r=>r.reportPath),targets=new Set(['index.html',...reports]);
 for(const report of reports){const html=await fs.readFile(path.join(site,report),'utf8');for(const match of html.matchAll(/(?:href|src)=["']([^"']+)["']/g)){
  const url=new URL(match[1].replaceAll('&amp;','&'),new URL(report,base)),prefix=new URL(base);
  if(url.origin!==prefix.origin||!url.pathname.startsWith(prefix.pathname))continue;
  let relative=decodeURIComponent(url.pathname.slice(prefix.pathname.length));if(!relative||relative.endsWith('/'))relative+='index.html';
  if(/\.(?:html|md|css|js|png|webp|jpg|jpeg|gif|svg|woff2?)$/i.test(relative))targets.add(relative);
 }}
 for(const relative of targets){const target=path.resolve(site,relative);if(!target.startsWith(path.resolve(site)+path.sep))throw new Error('public target escaped package');
  const expected=await fs.readFile(target),url=new URL(relative,base);url.searchParams.set('verify',process.env.GITHUB_SHA||String(Date.now()));await verifyResponse(url.href,expected,{request});
 }
 console.log(`PASS: ${targets.size} public reports/assets returned HTTP 200 and exact packaged SHA-256 bytes. Browser rendering not tested.`);
 return targets.size;
}
if(process.argv[1]&&path.resolve(process.argv[1])===fileURLToPath(import.meta.url))await verifyPublic();
