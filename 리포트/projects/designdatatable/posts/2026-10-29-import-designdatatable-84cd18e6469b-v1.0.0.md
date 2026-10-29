---
title: Chart preview — 2026-10-19
date: 2026-10-19T13:52:36.438Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Chart preview — 2026-10-19 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-19/ChartPreview.md
source_sha256: 27f736ca15a4f4ae85f332a80b28f996cecb1eb2104c18e6133176ac6cf1c8a6
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Basic bar (vertical/horizontal), line and single-series pie charts are displayed as simplified read-only previews in the original workbook viewer. Drawing anchors, offsets, sizes, titles, series names, literal/cached numeric values and direct one-dimensional cross-sheet references are read. Saved worksheet values take precedence over cached values. Original XML and parts remain unchanged by viewing.

Combination, 3D, stacked, logarithmic, unavailable/oversized-data or other unsupported chart types show an explicit card at the chart position. Corrupt chart parts retain an unsupported marker where possible and raise a visible sheet warning without blocking cell viewing. Chart-only worksheets are included. Preview limits are 100 charts per sheet, 12 series, 200 points and 8 MB per chart/drawing XML part. No chart editing, Excel theme/style equivalence or formula recalculation is claimed.

Entry points: View > Saved original / Chart preview, and the chart-count button in the data ribbon when charts are detected. Previews reflect the saved original, not unsaved table edits. Viewer status shows chart count and read warnings. User-facing messages and HTML guidance cover all nine supported languages.

Verification: 10,541 assertions passed. Tests cover basic/unsupported variants, Unicode titles, anchors/EMU size, current cell values, cross-sheet values, negative literal data, unavailable/bad references, combination-chart fallback, XML external-entity isolation, and all preceding preservation checks. Isolated Unity: 138 checks passed, including native chart detection, cross-sheet values, bounds and unchanged chart/summary/image parts.

Actual interactive UI appearance, scrolling, clipping and mouse operation are not visually verified. This is a simplified renderer, not a claim of Excel-identical charts. The final viewport-bound adjustment was compiled/regression-tested separately after the native run.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-19/ChartPreview.md)

- 원문 SHA-256: `27f736ca15a4f4ae85f332a80b28f996cecb1eb2104c18e6133176ac6cf1c8a6`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
