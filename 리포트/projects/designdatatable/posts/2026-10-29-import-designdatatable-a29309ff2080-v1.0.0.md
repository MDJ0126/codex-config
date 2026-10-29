---
title: Reviewed distribution examples — 2026-10-26
date: 2026-10-26T07:52:31.320Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Reviewed distribution examples — 2026-10-26 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/store-samples/Verification.md
source_sha256: 3f40ae4645e8c83663b5910cae78c84783c38a787f2d32a321e8127ddb0f8170
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

## Callout lines outside the body

The first gallery row contains accent/border callout presets. Their bundled DrawingML definitions intentionally place leader lines beyond the body rectangle. For example, accentBorderCallout1 has default x adjustments -8333 and -38333 (relative to 100000), with an endpoint at y=112500. The 150x95 body therefore has a complete path bounding box wider/taller than 150x95. This is valid geometry.

The original compact gallery placed neighboring leader lines across adjacent example areas. The fixture builder now uses actual evaluated path bounds to position each example inside a 243x150 tile, with a separate 32-pixel label row. Shape geometry and adjustment definitions remain unchanged. Every one of the 187 presets and five custom examples was checked for complete-path containment; callouts are not clipped to hide their leaders.

## Individual file review

| Published file | Findings and changes | Review |
| --- | --- | --- |
| ClosedShapeGradientsUi.xlsx | The original drawing-only fixture had narrow text wrapping, especially on arrows. Added a Guide, enlarged/centered shapes, and placed readable names below them. All 13 geometry and gradient definitions remain. | Guide preview, production gradient/path raster preview, import/recalculate/edit/save/reopen. |
| EmptyStringCoercion.xlsx | Error results and blank-looking formulas were unexplained. Added a Guide listing the intentional #VALUE! cases and blank-versus-empty-text behavior. Original inputs/formulas remain. | Guide preview, target calculation/display results, error whitelist, save/reopen. |
| GeneralSections.xlsx | Two unexplained cells were not a useful standalone example. Added a Guide explaining numeric storage, red negative sections and TEXT. Kept original -12.5 and formula. | Verified (12.5), red number-color directive, positive input change to 4.25, saved TEXT result. |
| LocaleDefaultCalendar.xlsx | Date serials had no display format and the second data row was hidden by a retained filter. Added Guide, yyyy-mm-dd display and cleared initial row filtering. Both dates are visible. | Independently verified serials as 2024-02-03 and 2567-02-03; target formatter and row state checked. |
| ShapeCatalog.xlsx | Added the complete catalog to the distribution and fixed tile spacing to account for external leader lines. | All nine sheets and 192 drawings inspected through production geometry output; bounds and save/reopen checked. |
| ChartGallery.xlsx | Added seven native editable chart types with source tables beside them: column, line, area, pie, doughnut, radar and scatter. Explicit scatter axis format avoids milli-prefix labels. | All seven rendered; target parser supports every chart, series and source binding. Changing Column!B6 to 1500 updates the chart and survives save/reopen. |

All six files contain 24 worksheets. Each sheet was inspected through software rendering and/or the target reader/raster output. These are not native Unity screenshots. The original four files in Assets/Editor/StoreAuditSamples were preserved; original and published checksums are recorded in PublishedSamples.json.

## Executed verification

- Final build: zero warnings, zero errors.
- `--store-samples`: 692 successful assertions, all six files.
- `--geometry-catalog`: 1331 successful assertions, 1122 preset/aspect-ratio evaluations.
- ReviewStoreSamples.py: six package/XML audits, no errors. Internal relationships, cell/style indices, documented error cells and original source checksums checked.
- Charts are source-cell references, not static images. A synthetic-data note is included.
- BuildLocalizedDocs.py updated the nine language guides with links and descriptions of the six examples.

The artifact import renderer calculates empty-text arithmetic and General-section TEXT differently from the target application. It was used to inspect the authored Guide sheets and native chart layouts. Formula correctness and final cell previews use the actual application calculation/display code; original cached results and formulas were not overwritten with the artifact renderer's results. PackageAudit.json records this limitation.

## Distribution archive

The examples reside inside Assets/DesignDataTable/Sample/StoreAuditSamples, the existing package export scope. The reviewed archive is output/release/DesignDataTable-samples-reviewed-20261026.unitypackage.

A separate hidden Unity export was attempted in an owned temporary project. Licensing IPC reconnects failed and the log reported the missing com.unity.editor.headless entitlement. Only that owned batch-mode process was stopped after verifying its exact project path; the user's editor and licensing clients were not stopped. No computer-use tools were called during this task.

The deliverable was assembled from source using the standard GUID/asset/asset.meta/pathname Unity package structure, matching an existing native-export archive layout. VerifyRelease.py passed: 457 unique GUID entries, exact source and metadata byte matches, all expected source assets present, external UGUI GUID dependencies resolved, no development caches or overlong paths. The JSON records the archive SHA256.

Native Unity re-import and interactive UI are **not verified**. ScreenshotManifest.json retains historical hashes and 13 pending captures; VerifyLocalizedDocs.py consequently reports the pending/stale screenshots, not a documentation pass. No user screen capture was taken and no source hash was rewritten to imply recapture.

XERJ MCP index designdatatable-code-v3-docs was used for package/menu, drawing, calculation and formatting code discovery, with current source reads and rg fallbacks. No Git commit or remote push was performed.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/store-samples/Verification.md)

- 원문 SHA-256: `3f40ae4645e8c83663b5910cae78c84783c38a787f2d32a321e8127ddb0f8170`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
