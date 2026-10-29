---
title: Five independent game scenes with distinct artwork
date: 2026-10-27T14:55:55.697Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Five independent game scenes with distinct artwork — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/mini-games/Verification.md
source_sha256: 0b66dc19c289d4d92f074e88440b2df0b06da6a15255f7898c6e26f5bb95967d
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Open an `Example - ...` scene under `Assets/DesignDataTable/Sample/Scenes` and press Play. Each starts independently. The hub is optional. Coin Dash uses pixel dungeon sprites, Meteor Dodge illustrated spacecraft, Brick Breaker pastel puzzle art, Road Dash top-down racing assets and Memory Match animal illustrations. The existing Neon Runner scene filename is retained for stable references.

51 selected sprites from five official Kenney CC0 packs are included with original licenses and a file/source/hash manifest. Four backdrops are sample compositions. The models remain separate from rendering and use the real encrypted MiniGameBalance table.

All five new games and the original survival/shop scenes use ArcadeText localization. Its 35 stable Enum IDs run down column A, with nine languages across row 1. Every game has a language picker and AUTO follows the device language. Original UI Text components refresh on LanguageChanged; source data and number formatting are retained.

36 gameplay/sample assertions and 43 localization assertions passed. Five new saved scenes rendered in native Unity Play mode. Both original scenes verified actual live UI Text against all nine source languages. English new-game and Korean/French original-scene captures were visually inspected. Simulation was slowed only to stabilize captures; Memory Match reveals use model input for artwork inspection. The Windows build and native player checks are recorded in Result.json and FinalPlayerBuild.txt.

The long French restart label found during actual player testing now uses measured text width to fit the button. Final captures and build follow that correction. Temporary development probes are archived here and removed from Assets. Existing Build Settings and user workbook changes are preserved. Automatically generated ProjectSettings files are backed up under the ignored verification Temp directory before restoration.

Nine guide editions include current game images and language instructions. Older common editor screenshots still have stale source hashes; LocalizedDocs.json reports those failures honestly. Current game screenshots are not implicated in those failures.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/mini-games/Verification.md)

- 원문 SHA-256: `0b66dc19c289d4d92f074e88440b2df0b06da6a15255f7898c6e26f5bb95967d`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
