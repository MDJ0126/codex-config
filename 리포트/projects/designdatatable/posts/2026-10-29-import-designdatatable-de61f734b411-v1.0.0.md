---
title: Create menu and curved outline coverage
date: 2026-10-26T07:00:38.337Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Create menu and curved outline coverage — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/create-and-curves/Verification.md
source_sha256: d561535f63bc617e6636a403b1a65deb4da13cb896bbace4589dfca2142578a0
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Implemented `Assets > Create > Design Data Table` using Unity's Project-window name editing. The selected Assets folder receives a new XLSX with Id/int header rows. Creation does not register the sheet or overwrite an existing file. The same source-creation method invoked by the menu action was exercised by the headless verification runner.

Changed thin-outline scan conversion from 2x2 to 8x8 coverage sampling. Existing 2x2 solid-fill sampling, path geometry, dash patterns, cap/join rules and source workbook XML remain unchanged. Thin curved outlines previously had only five coverage alpha levels; the new setting provides 65. The user confirmed the reported dots were black marks on curved boundaries, not selection handles.

An independent analytic annulus sampled at 64x64 per pixel was used as the numerical reference. Total absolute alpha error over the same 32x32 image fell from 2492.25 for 2x2 coverage to 345.42 for 8x8 coverage. This proves the coverage accuracy improvement; it does not certify that every reported visual mark is gone in the running Unity UI.

Final build: success with zero warnings and zero errors. Executed checks:

| Verification.dll argument | Successful assertions |
| --- | ---: |
| `--geometry-catalog` | 1331 |
| `--source-creation` | 4 |
| `--outline-preview-only` | 60 |
| `--outline-styles-only` | 59 |

Source creation checks actually wrote and reopened a new XLSX, verified Id/int, accepted a filename with spaces, rejected existing destinations without changing bytes, rejected outside-Assets paths and refused non-XLSX extensions. Outputs are isolated in Tools/Verification/Temp/SourceCreation. No generated scripts or table registrations were created.

The user explicitly stopped computer use to use their computer. No computer-use calls were made after that instruction. Actual menu invocation, final curved-outline appearance and native screenshot recapture therefore remain unverified. No focus activation, keyboard/mouse input or app restart was performed afterward. The temporary Assets probe and request marker are absent.

Nine language guide descriptions were updated and regenerated. Historical images and source hashes were preserved. Twelve affected/new captures are explicitly pending in ScreenshotManifest.json. VerifyLocalizedDocs.py reports those pending captures and stale renderer hashes as errors in LocalizedDocs.json; documentation is not reported as fully current or passing.

XERJ MCP index designdatatable-code-v3-docs was used for menu, renderer and object-handle searches, with candidates read from current source. rg was used when symbol retrieval was incomplete. No Git commit or push was performed.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/create-and-curves/Verification.md)

- 원문 SHA-256: `d561535f63bc617e6636a403b1a65deb4da13cb896bbace4589dfca2142578a0`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
