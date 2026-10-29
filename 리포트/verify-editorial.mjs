import {readFile} from 'node:fs/promises';
import {createHash} from 'node:crypto';
import path from 'node:path';
import {fileURLToPath} from 'node:url';
import assert from 'node:assert/strict';
const root=path.dirname(fileURLToPath(import.meta.url));
const hash=b=>createHash('sha256').update(b).digest('hex');
const records=JSON.parse(await readFile(path.join(root,'migration/editorial-2026-10-29.json'),'utf8'));
for(const e of records){assert.equal(hash(await readFile(path.join(root,e.backup))),e.beforeSha256,e.backup);assert.equal(hash(await readFile(path.join(root,e.post))),e.afterSha256,e.post);}
const codeassist=await readFile(path.join(root,'projects/codeassist/reports/2026-10-29-import-codeassist-eff852ba2638-v1.0.0.html'),'utf8');
for(const marker of ['<table>','247','108','미측정','원문과 편집 이력'])assert.ok(codeassist.includes(marker),marker);
assert.ok(!codeassist.includes('<script>'));
console.log(`PASS: ${records.length} editorial originals and formatted post hashes; representative original table, figures and limitations preserved.`);
