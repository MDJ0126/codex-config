---
title: Column-based local Enums
date: 2026-10-28T14:08:37.474Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Column-based local Enums — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-28/column-local-enums/Results.md
source_sha256: f96c9d0c04b029184b356821428745e1141603c9460d290cfc5b5cbd012162cf
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

- Each column defines one local Enum. Headers are type names; members start on row 2 as Name = integer with optional // descriptions.
- Explicit numbers, names, descriptions and all game-table values preserved in native migration.
- EnumGear and EnumEnemies runtime binaries remained byte-identical.
- Both tables compile using the same shared local Element C# type and load data in Unity.
- Definition navigation selects the relevant column. Inspector and type-picker links retained.
- Add creates a column; member or type removal warns dependent values; Undo restores validity.
- Old sheet-per-type and EnumId-grouped formats remain readable and convertible.
- Global Enum providers unchanged.
- 428 automated assertions passed; .NET build: 0 errors, 0 warnings.
- Final Unity 6000.0.83f1 UI renders visually inspected: table, selected column, Inspector and type picker. English only; no physical keyboard/mouse input.
- Native sandbox Editor sources match production byte for byte.
- XERJ unavailable: server startup refused a locked data directory. Searches continued with rg; no lock deleted.
- Historical screenshot source mismatches and pending physical Create-menu capture remain outside this change; full documentation verification not claimed.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-28/column-local-enums/Results.md)

- 원문 SHA-256: `f96c9d0c04b029184b356821428745e1141603c9460d290cfc5b5cbd012162cf`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
