---
title: Workbook object preservation — 2026-10-19
date: 2026-10-19T13:43:34.641Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Workbook object preservation — 2026-10-19 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-19/WorkbookPreservation.md
source_sha256: 66bcb8b9635d236cf48722f4f6440ca03ce3aa9fd63093b8aaa247538b2407a7
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Existing XLSX saves now retain the original worksheet structure and replace the editable data portion. Drawing connections, charts, images, relationships, unchanged rich text, cell style indices, print settings and other worksheet elements are retained. Unchanged package parts, including the summary worksheet and chart/media parts, are copied byte-for-byte. Original shared strings and styles remain available. Single-sheet saves also use this preservation path.

Safeguards:
- Chart/drawing/table/external-reference workbooks and worksheets with merges, conditional formatting, validations or hyperlinks reject changes to row count, column count, names or types. This includes automatic registration that inserts Id/schema rows. Prepare the native Id/name/type layout in Excel first; the registration popup recognizes an existing native layout without adding another Id column.
- Digital signatures, pivot/slicer parts, macros, ActiveX and embedded objects block original saves pending verified handling.
- Object-containing workbooks receive a copy in Tools/WorkbookBackups before replacement.
- Writes use a temporary package and atomic replacement; bounded retries handle replacement failures without falling back to destructive overwrite.
- Protection messages are translated in all nine UI languages. Nine HTML guides include the safety limits.

Verification: 10,343 assertions passed. Single-sheet and multi-sheet fixtures each contain a chart and image in the data sheet; the multi-sheet fixture also has a chart, image and summary formula on a second sheet. Package comparison verifies original chart/media/relationship parts, drawing references, summary worksheet, print settings, styles and rich text. Independent openpyxl reading confirms all charts/images remain readable, chart ranges remain unchanged, the summary formula is preserved and edited B3 equals 99. Rejected structural/pivot saves leave the complete original file byte-identical.

Actual Microsoft Excel rendering, chart-cache refresh and every third-party workbook feature have not been verified. Chart drawing/editing is not implemented. The tests establish preservation for the documented fixtures; they do not certify arbitrary Excel documents. See workbook-preservation/independent-reader.json and unity-session.jsonl for execution evidence.

Final isolated Unity run: 135 checks passed, including five workbook-object checks and all three sample scenes. The first attempt failed in an existing JSON/TSV/XML export replacement; after adding bounded atomic-replace retries, the complete fresh run passed. The failed execution remains in history.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-19/WorkbookPreservation.md)

- 원문 SHA-256: `66bcb8b9635d236cf48722f4f6440ca03ce3aa9fd63093b8aaa247538b2407a7`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
