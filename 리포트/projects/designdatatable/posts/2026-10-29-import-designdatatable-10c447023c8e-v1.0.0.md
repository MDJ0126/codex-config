---
title: ClosedShapeGradientsUi outline comparison
date: 2026-10-26T11:29:11.572Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 검토
category: 이전 기록
summary: ClosedShapeGradientsUi outline comparison — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/gradient-outline-analysis/Verification.md
source_sha256: 4787d392db80db1f7b1a67ff00a134220225a410ad4ecf74215e6632ac09a494
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Read-only analysis. No workbook or product source was changed.

Original: Assets/Editor/StoreAuditSamples/ClosedShapeGradientsUi.xlsx
Published: Assets/DesignDataTable/Sample/StoreAuditSamples/ClosedShapeGradientsUi.xlsx

Both packages have 13 shapes. Neither package contains a:ln, a:lnRef, xdr:style or a theme part. Canonical XML of all 13 xdr:spPr elements matches between original and published copies. Packaging changed anchors/labels, not shape properties.

WorkbookDrawings.cs:119 sets NoLine only when a:ln/a:noFill exists. With no a:ln, NoLine becomes false, Line remains empty and LineWidth defaults to 9525/9525 = 1 pixel. WorkbookDrawingGUI.cs resolves an empty line color to the editor accent and draws a stroke whenever NoLine is false. The prior production-raster preview independently shows these outlines.

The supplied Google Sheets screenshot has no visible outlines. For this sample, borderless display matches the supplied reference and no authored outline exists in the package. The added stroke is the current renderer fallback, not a difference introduced in the distributed workbook. This does not establish that every missing a:ln should mean no line: inherited shape styles must be resolved where present. This sample has none.

Official structure references:
https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.drawing.outline?view=openxml-3.0.1
https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.drawing.linereference?view=openxml-3.0.1

Excel desktop rendering was not run. No computer use was performed. XML/source and prior target-raster execution establish the fallback mechanism; the supplied screenshot establishes the reference appearance.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/gradient-outline-analysis/Verification.md)

- 원문 SHA-256: `4787d392db80db1f7b1a67ff00a134220225a410ad4ecf74215e6632ac09a494`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
