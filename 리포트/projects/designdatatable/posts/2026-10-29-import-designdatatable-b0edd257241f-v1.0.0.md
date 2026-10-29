---
title: Distinct sample workbooks
date: 2026-10-26T11:24:54.539Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Distinct sample workbooks — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/sample-diversity/Verification.md
source_sha256: be6eaa331c56ca6929aaf4b3a39d59c2ae6b5ab28556ffbae6843c36ac52bd55
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The four published samples now use purpose-specific working sheets instead of the same Guide template.

- ClosedShapeGradientsUi: Data (13 editable gradient shapes) opens first; Palette contains swatches and angle experiments.
- EmptyStringCoercion: Coercion lab compares 5 kinds of input using arithmetic, SUM and LEN, with optional game-data field examples.
- GeneralSections: Format workshop compares 7 game values across General and fixed-precision sections, plus percentage, multiplier and text formats.
- LocaleDefaultCalendar: Release calendar provides 7 milestones, inclusive duration formulas, a leap-day release, date filters and multiple date displays.

Original regression cells and gradients are retained in Data. Original Assets/Editor fixtures are unchanged. ChartGallery and ShapeCatalog have the same hashes as the earlier reviewed samples.

Validation: .NET build passed with zero warnings and errors. The production reader/edit session passed 699 sample assertions, including input edits, recalculation, save/reopen and object preservation. ZIP/XML relationship/style/error audit passed for all 6 published files. All affected sheet previews were inspected: Artifact Tool previews for layout and production reader/raster previews for actual formatted values and drawing paths.

Artifact Tool renders empty-string arithmetic and General/text sections differently from the target engine. Its previews do not establish target calculation behavior. The intentional empty-string arithmetic error cache is set during packaging and confirmed by target-engine checks. Production previews confirm #VALUE!, parentheses, zero sections and Item: Rare.

No Unity UI automation or capture was performed, as requested. Native on-screen appearance remains unverified. This task changes workbook content and the sample README, not application UI source or guide screenshots.

Commands:
- BuildStoreSamples.mjs --focused-redesign
- PackageStoreSamples.py
- dotnet build Tools/Verification/Verification.csproj --no-restore -v quiet
- dotnet Tools/Verification/bin/Debug/net9.0/Verification.dll --store-samples
- ReviewStoreSamples.py

XERJ MCP search located WorkbookFormatting; current source was read to validate the result. The separate local CLI status reported its node stopped; the available MCP index was reachable and used. Searches outside the Assets index used direct reads/rg.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/sample-diversity/Verification.md)

- 원문 SHA-256: `be6eaa331c56ca6929aaf4b3a39d59c2ae6b5ab28556ffbae6843c36ac52bd55`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
