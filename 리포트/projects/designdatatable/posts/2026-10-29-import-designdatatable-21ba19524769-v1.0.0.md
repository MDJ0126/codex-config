---
title: Game Data tab label follows registration state
date: 2026-10-26T15:48:27.219Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Game Data tab label follows registration state — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/game-data-tab-state/Verification.md
source_sha256: 4a6f7d62b17ed604e2ef34b007a29507dfd8eb394e9d181a5cd09708d4b31726
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Ordinary sheets retain “Switch to Game Data”. Already registered sheets use the existing localized “Game Data” label and “Registered · Edit game data” help text. The same registered-state flag controls the button title and custom help.

WorkbookViewer supplies IsGameData, which resolves the active sheet's registration. DataTableEditor supplies tableInfo != null. Both shared-ribbon callers now pass that state; selecting a different sheet refreshes it through the existing GUI presentation state. The pill style, tab index, registration command and editing behavior are unchanged. Existing nine-language strings are reused.

All nine guide descriptions were regenerated. Native UI screenshots/visual interaction were not captured under the user's existing computer-use prohibition. Historical screenshot/source hashes were preserved. Documentation verification reports only the pending/stale native capture backlog.

XERJ found the ribbon.gameaction rendering site, validated against current source and both actual call sites.

Build passed with zero warnings/errors. All 82 ribbon artwork/layout assertions passed, including ordinary-vs-registered tab label selection. Actual Unity visual confirmation remains pending.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/game-data-tab-state/Verification.md)

- 원문 SHA-256: `4a6f7d62b17ed604e2ef34b007a29507dfd8eb394e9d181a5cd09708d4b31726`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
