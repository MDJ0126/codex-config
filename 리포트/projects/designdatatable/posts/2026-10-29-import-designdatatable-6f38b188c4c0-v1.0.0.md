---
title: Option window redesign
date: 2026-10-28T17:44:10.991Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Option window redesign — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-29/options-redesign/Results.md
source_sha256: 5bda5dfcb9efdfad58619d132b5a76a8e7dd0f537aa298db21a8bf67091e06f9
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Implemented a dialog-only visual style: white headers with a thin document accent, quieter typography, rounded outlined fields/buttons, consistent category navigation and spacing. The workbook ribbon retains its original theme. Applied to the common workbook tool dialogs and independent find/replace, paste-special, column filtering, diagnostics, formula help, registration, legacy selection/value windows and preferences.

Preferences: one native-language card picker instead of duplicate selection controls; compact palette swatches; advanced display-color controls collapsed by default; searchable registered tables with collapsible binary details; readable two-column shortcut reference with localized separator handling and measured row heights.

Shape fill: display Solid / Linear gradient / Pattern controls separately, load existing gradient stops and pattern colors, and show one applicable Apply action. Conditional formatting: advanced font/number/protection options use a disclosure; hidden values remain in the draft; empty disabled rule-management controls are omitted. Registration copy is shortened without claiming existing Ids will be regenerated. Type-picker navigation buttons use explicit compact padding so the arrow remains visible.

Verification:
- Final .NET/editor build passed: zero warnings/errors.
- 27 native option render states passed, including five format categories, three paint modes, advanced sections, table search/details/empty state, Korean appearance and Japanese number-format layouts. Native data-preservation checks passed for existing multistop gradients, view-only paint switching and folded formatting payloads.
- 15 current guide views and six option-gallery examples were recaptured and visually reviewed with real Unity HostViews. Four existing game captures remain valid. Current documentation: nine languages, 25 referenced images, zero missing-link/image/source-hash errors. Unused historical captures are retained.
- 119 headless browser checks passed for tabs, keyboard navigation, languages, mobile overflow, anchors and image decoding. Six gallery examples are collapsed by default to keep the main guide minimal.
- Existing taskbar hover (151) and game-cell validation (331) regression assertions passed.
- English/Korean PDF guides regenerated and rendered with Poppler for visual review.

No physical desktop input or foreground app control was used. Actual mouse/keyboard interaction with the live user windows remains unverified. Independent native-speaker review of all translations was not performed. Intermediate compiler/render-publisher failures remain in the logs; they are not counted as passes.

XERJ: failed because its node was not running; rg was used.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-29/options-redesign/Results.md)

- 원문 SHA-256: `5bda5dfcb9efdfad58619d132b5a76a8e7dd0f537aa298db21a8bf67091e06f9`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
