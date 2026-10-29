---
title: Reverse import verification — 2026-10-19
date: 2026-10-19T09:56:49.010Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Reverse import verification — 2026-10-19 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-19/DataImport.md
source_sha256: 48d40518593f13cd94367fffebe8a3f43bfcf2fdeb72ffe637c3ac0ac7927f91
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Implemented `TableImport`: exported XLSX/native CSV, TSV, typed JSON/XML and generated ScriptableObject can populate the current table through Import. Input files are read only; the current table name and registered destination sheet are retained. Data is validated before a single undoable replacement. Saving remains explicit.

JSON/XML require the Design Data Table export schema, not arbitrary third-party layouts. TSV uses a column-name header and infers types; it cannot recover metadata absent from TSV. JSON/XML/TSV/ScriptableObject restore calculated values rather than original formulas. Generated ScriptableObject assets must be inside this project's Assets folder. Imported data is not automatically registered as a new source.

- Full C# regression: **10,247 assertions passed**, compiling all project C# against Unity assemblies.
- Real isolated Unity: **130 checks passed**, including generated ScriptableObject binary roundtrip for all nine supported types and three sample scenes in Play Mode.
- JSON/XML roundtrips preserve typed binary values and column order, including Int64.MaxValue and UTC DateTime.
- TSV preserves quoted multiline Unicode text and exact Int64 values.
- Invalid schemas, duplicate JSON keys, trailing JSON and XML external entities are rejected.
- Actual menu/file-picker mouse operation remains unverified. Native validation used Unity APIs in an isolated project; the open original editor was not controlled.
- Evidence: `data-import/unity-session.jsonl`.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-19/DataImport.md)

- 원문 SHA-256: `48d40518593f13cd94367fffebe8a3f43bfcf2fdeb72ffe637c3ac0ac7927f91`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
