// 원본을 덮어쓰지 않고 실제 파일 왕복 검증용 사본 한 개만 생성한다.
import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import { FileBlob, SpreadsheetFile } from '@oai/artifact-tool';

const root = path.resolve(import.meta.dirname, '../..');
const source = path.join(root, 'Assets/GameData/Workbooks/OverdoxGameData.xlsx');
const output = path.join(root, 'Docs/Verification/ExcelSavedRoundTrip');
const hash = async file => crypto.createHash('sha256').update(await fs.readFile(file)).digest('hex');
const originalHash = await hash(source);
const workbook = await SpreadsheetFile.importXlsx(await FileBlob.load(source));
const gear = workbook.worksheets.getItem('Gear');
if (Number(gear.getRange('A39').values[0][0]) !== 10750 || Number(gear.getRange('AX39').values[0][0]) !== 1) {
  throw new Error('원본 검증 조건 불일치: expected weapon10750, scale1');
}
gear.getRange('AX39').values = [[1.1]];
workbook.recalculate();
const actual = {
  base: Number(gear.getRange('H39').values[0][0]),
  catalog: Number(workbook.worksheets.getItem('Weapons').getRange('G39').values[0][0]),
  preview: Number(workbook.worksheets.getItem('Balance').getRange('H39').values[0][0]),
};
if (actual.base !== 132 || actual.catalog !== 132 || actual.preview !== 215) { throw new Error(JSON.stringify(actual)); }
const preview = await workbook.render({ sheetName: 'Balance', range: 'A36:J41', scale: 1, format: 'png' });
await fs.writeFile(path.join(output, 'balance-changed.png'), new Uint8Array(await preview.arrayBuffer()));
const destination = path.join(output, 'OverdoxGameData-scale110.xlsx');
await (await SpreadsheetFile.exportXlsx(workbook)).save(destination);
const saved = await SpreadsheetFile.importXlsx(await FileBlob.load(destination));
const savedValues = {
  scale: saved.worksheets.getItem('Gear').getRange('AX39').values[0][0],
  base: saved.worksheets.getItem('Gear').getRange('H39').values[0][0],
  preview: saved.worksheets.getItem('Balance').getRange('H39').values[0][0],
};
if (Number(savedValues.scale) !== 1.1 || Number(savedValues.base) !== 132 || Number(savedValues.preview) !== 215) {
  throw new Error(`Saved workbook mismatch: ${JSON.stringify(savedValues)}`);
}
if (await hash(source) !== originalHash) { throw new Error('Production workbook changed'); }
const result = { recordedAt: new Date().toISOString(), originalHash, variantHash: await hash(destination), weaponId: 10750, changedCell: 'Gear!AX39', before: 1, after: 1.1, actual, savedValues, sourceUnchanged: true, scope: 'Artifact import, one-cell edit, recalculate, export and saved-file reimport; Unity compiler and Windows runtime not yet verified' };
await fs.writeFile(path.join(output, 'saved-file-verification.json'), JSON.stringify(result, null, 2));
console.log(JSON.stringify(result));
