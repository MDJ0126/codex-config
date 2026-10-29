---
title: DrawingML geometry verification — 2026-10-26
date: 2026-10-26T06:38:23.858Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: DrawingML geometry verification — 2026-10-26 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/shape-catalog/Verification.md
source_sha256: 5fa39547e98297261d1cba19e06c94d4cf646f0825545a278d0e88c60d0859b0
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Implemented the complete 187-entry DrawingML preset geometry catalog and a shared custom-path evaluator. Excel XLSX and Google Sheets XLSX exports pass through the same workbook reader. The catalog fixture is `Assets/ShapeCatalogVerification.xlsx` (187 presets and five custom-path examples across nine sheets).

## Executed checks

Final `dotnet build Tools/Verification/Verification.csproj --no-restore -v minimal`: success, zero warnings and zero errors.

| Verification.dll argument | Successful assertions |
| --- | ---: |
| `--geometry-catalog` | 1330 |
| `--shape-gradient-only` | 107 |
| `--outline-preview-only` | 60 |
| `--outline-styles-only` | 59 |
| `--reported-shapes C:/Users/ehdwn/Downloads/test.xlsx` | 6 |
| `--external-ui-contract` | 36 |

The geometry suite evaluates every preset at six aspect ratios (1122 evaluations), checks custom cubic/quadratic curves and elliptical arcs, holes, winding, transparency, overshooting curves, malformed commands and formula arity. Integration imports 192 objects and saves/reopens them after style and position edits while preserving preset/custom geometry XML.

## Native Unity observations

Unity 6000.0.83f1, English UI, Windows. All eight preset-gallery sheets and the custom-path sheet were inspected in the running editor. Representative current gallery, custom-path and insertion-list screens are included in the guide. Curved-outline artifacts observed during development were corrected by rasterizing the union of outline polygons. Initial screenshots are retained separately.

Twenty guide screenshots were actually recaptured and individually inspected. Each capture has adjacent metadata containing its UTC capture time and contemporaneous source hashes. The manifest records the subset of relevant hashes and final JPEG hashes. Previous documentation images and manifest remain under `historical-docs/`. `BuildLocalizedDocs.py` and `VerifyLocalizedDocs.py` succeeded for nine languages with no errors; `LocalizedDocs.json` contains the report. Only the English Unity layout was visually reviewed.

The original external test.xlsx remained open with its existing unsaved state. Its two shapes and chart render, registration is disabled, the title indicates external origin, and hovering the disabled control or shape displays custom help. Actual shelf minimize/restore was exercised. After closing only clean verification fixtures, clicking the original workbook's top-right Settings displayed preferences over the retained workbook without Unity main covering it (`settings-owner-preserved.png`). No source workbook was saved or discarded and no workspace setting was changed.

The development-only probe was removed from Assets after review; its reproducible source remains in Tools/Verification/Unity. Tool windows were manually positioned for documentation captures, so those captures do not certify automatic multi-monitor placement.

## Limits of this evidence

This verifies geometry support, not complete Excel/Google product compatibility or pixel identity. No full native Excel/Google round-trip visual comparison was executed. SmartArt, WordArt text warping, 3D/effects, all image encodings and unsupported fill/style variants remain separate capabilities. The custom-path examples are representative, not an exhaustive corpus of every externally produced custom drawing. Rasterization approximates curves and enforces resource budgets.

Preset XML provenance, SHA256 and Apache licensing are recorded in Assets/DesignDataTable/ThirdParty/DrawingML. Bundled circular-arrow definitions contain a redundant formula operand; only the bundled erratum is normalized. Imported custom formulas retain strict arity checks.

XERJ was used with index `designdatatable-code-v3-docs` to locate drawing, object editing and window code; candidates were checked against current files. The empty default index and failing local script status were bypassed with the explicit functioning MCP index, with rg used for out-of-scope files.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/shape-catalog/Verification.md)

- 원문 SHA-256: `5fa39547e98297261d1cba19e06c94d4cf646f0825545a278d0e88c60d0859b0`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
