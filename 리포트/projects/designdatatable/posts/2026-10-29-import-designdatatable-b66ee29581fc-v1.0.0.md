---
title: Minimal two-tab documentation and animation preference
date: 2026-10-28T17:14:02.996Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Minimal two-tab documentation and animation preference — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-29/minimal-guide/Results.md
source_sha256: f9624d4f0301bc12c4aaa250feac2d98ee71573931c9ce545688fd3f0b037a73
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

## Implemented
- A persisted ON-by-default Window animations option in Settings > Appearance, localized in all nine UI languages. Reset restores ON. OFF bypasses minimize/restore/maximize motion and preview fade/rise/hover transitions while preserving hover delays. Toggling OFF completes/cancels active captured surfaces safely on their next update.
- Replaced append-only user-guide generation with one concise source of truth: seven green spreadsheet chapters and seven orange game-data chapters. Features use short Feature/Description tables. Conversion has four steps and a typed XLSX example; Summary, local/global Enums, validation, Play read-only, localization and runtime API are covered.
- 19 newly rendered native Unity screenshots: 15 workbook/popup/preferences views and four actual Play-mode games. Games alternate 2D/3D and are paired with their real source tables. Native English UI only; non-English Unity layouts were not visually certified.
- Nine offline HTML guides and entry pages, keyboard-accessible tabs, language switching, direct chapter links, mobile layout, and refreshed English/Korean 11-page PDFs with two section colors and page numbers.

## Verification
- Production/editor C# build: 0 warnings, 0 errors.
- Six unique native preference checks passed, including migration from an old preference file without the animations field, persistent OFF/ON, Reset, and disabled restore/maximize dispatch.
- 151 taskbar hover and 153 overflow assertions passed.
- 113 headless Microsoft Edge checks passed across nine languages: tabs, color, keyboard access, anchors, language links, image decode, mobile overflow, no JS errors. Isolated profiles; no user browser or desktop input.
- Four actual Unity Play captures completed without recorded Error/Exception; representative 2D/3D scene bytes and relevant runtime source hashes match the main workspace.
- Current guide verification: nine languages, 19 referenced screenshots, zero link/image/source-hash errors. The 93 unused older capture records and pending historical notes are retained; they are not certified current or silently re-hashed.
- English and Korean PDFs each have 11 pages; all pages rendered with Poppler, contact sheets and representative full-size pages visually reviewed. Runtime code and animation-setting text extracted successfully.

## Limits
Physical pointer/keyboard interaction with live document animation settings was not injected; native callbacks, persistence, guarded dispatch and rendered UI were tested. Actual desktop minimize/maximize feel remains unverified. Independent native-speaker review of translations was not performed. Initial headless launch/profile and a Japanese mobile heading overflow were recorded and resolved before the passing run.

XERJ failed: node not running. rg was used for current source checks.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-29/minimal-guide/Results.md)

- 원문 SHA-256: `f9624d4f0301bc12c4aaa250feac2d98ee71573931c9ce545688fd3f0b037a73`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
