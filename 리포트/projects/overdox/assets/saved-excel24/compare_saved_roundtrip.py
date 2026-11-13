"""XLSX는 읽기만 하며 저장 전후 모든 시트의 값·수식 변경 범위를 기록한다."""
import json
from pathlib import Path
from zipfile import ZipFile
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'Docs/Verification/ExcelSavedRoundTrip'
NS = {'s': 'http://schemas.openxmlformats.org/spreadsheetml/2006/main'}


def read(file):
    with ZipFile(file) as archive:
        strings = []
        if 'xl/sharedStrings.xml' in archive.namelist():
            strings = [''.join(node.itertext()) for node in ET.fromstring(archive.read('xl/sharedStrings.xml'))]
        book = ET.fromstring(archive.read('xl/workbook.xml'))
        relations = ET.fromstring(archive.read('xl/_rels/workbook.xml.rels'))
        targets = {node.attrib['Id']: node.attrib['Target'] for node in relations}
        sheets = {}
        for sheet in book.find('s:sheets', NS):
            rid = sheet.attrib['{http://schemas.openxmlformats.org/officeDocument/2006/relationships}id']
            target = targets[rid]
            name = target.lstrip('/') if target.startswith('/') else 'xl/' + target
            xml = ET.fromstring(archive.read(name))
            cells = {}
            for cell in xml.findall('.//s:sheetData/s:row/s:c', NS):
                value = cell.findtext('s:v', default='', namespaces=NS)
                kind = cell.get('t', 'n')
                if kind == 's':
                    value = strings[int(value)]
                    kind = 'text'
                elif kind == 'inlineStr':
                    value = ''.join(cell.find('s:is', NS).itertext())
                    kind = 'text'
                elif kind == 'n' and value:
                    value = float(value)
                cells[cell.attrib['r']] = {'value': value, 'formula': cell.findtext('s:f', default='', namespaces=NS), 'type': kind}
            sheets[sheet.attrib['name']] = cells
        return sheets


before = read(ROOT / 'Assets/GameData/Workbooks/OverdoxGameData.xlsx')
after = read(OUT / 'OverdoxGameData-scale110.xlsx')
changes = []
for name in before.keys() | after.keys():
    for address in before.get(name, {}).keys() | after.get(name, {}).keys():
        old = before.get(name, {}).get(address)
        new = after.get(name, {}).get(address)
        if old != new:
            changes.append({'sheet': name, 'cell': address, 'before': old, 'after': new})
result = {'scope': 'Saved ZIP XML comparison of cell values and formulas across every sheet; style/object preservation not assessed here', 'beforeSheetCount': len(before), 'afterSheetCount': len(after), 'sameSheetNames': list(before) == list(after), 'changes': changes}
with ZipFile(ROOT / 'Assets/GameData/Workbooks/OverdoxGameData.xlsx') as original, ZipFile(OUT / 'OverdoxGameData-scale110.xlsx') as variant:
    original_names, variant_names = set(original.namelist()), set(variant.namelist())
    result['packageEntriesAdded'] = sorted(variant_names - original_names)
    result['packageEntriesRemoved'] = sorted(original_names - variant_names)
    result['packageEntriesChanged'] = sorted(name for name in original_names & variant_names if original.read(name) != variant.read(name))
    # 원본의 스타일·표·도형·유효성/뷰/행열 크기 등을 담는 비셀 XML을 직접 대조한다.
    metadata_changes = []
    for name in sorted(original_names & variant_names):
        if not name.startswith('xl/worksheets/sheet') or not name.endswith('.xml'):
            continue
        xmls = [ET.fromstring(archive.read(name)) for archive in (original, variant)]
        for xml in xmls:
            data = xml.find('s:sheetData', NS)
            if data is not None:
                for row in data:
                    for cell in row:
                        for child in list(cell):
                            if child.tag.rsplit('}', 1)[-1] in ('v', 'f', 'is'):
                                cell.remove(child)
        if ET.tostring(xmls[0]) != ET.tostring(xmls[1]):
            metadata_changes.append(name)
    result['sheetMetadataChanges'] = metadata_changes
    result['stylesByteIdentical'] = original.read('xl/styles.xml') == variant.read('xl/styles.xml')
    result['scope'] = 'All sheets: saved values/formulas, cell style IDs, row/column metadata and other non-value sheet XML; package inventory and styles bytes compared'
(OUT / 'saved-cell-comparison.json').write_text(json.dumps(result, ensure_ascii=False, indent=2), encoding='utf-8')
print(json.dumps(result, ensure_ascii=False))
