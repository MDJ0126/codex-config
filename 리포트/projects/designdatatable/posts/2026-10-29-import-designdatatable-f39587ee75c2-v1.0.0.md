---
title: Game Data pill action
date: 2026-10-26T09:32:45.598Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Game Data pill action — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/game-data-pill/Verification.md
source_sha256: 26b7864994264ba8c2f70b9e7e649143fd65b822c8143182db6da3e4cb09406f
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Changed the far-right orange rectangular tab to a fully rounded 24px-high pill, inset 3px vertically. The label is 게임 데이터로 전환하기 in Korean and Switch to Game Data in default English. All nine supported languages have localized action labels. Width follows the bold measured label plus 32px padding; the left tab strip continues to scroll. The rectangular selected underline was removed. The existing current-sheet-as-game-data custom help and Game Data tool-panel behavior remain intact.

Validation: build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors; 80 existing ribbon artwork/layout assertions passed. The new translation row has all nine values and the exact Korean/English labels. git diff --check passed. Nine guide descriptions updated.

Actual Unity appearance, translated layouts and screenshot recapture were not verified because the user prohibited computer use. Historical capture/source hashes were preserved; localized-doc verification intentionally remains failing for pending/stale UI captures. No new tests or runtime probes were added for this reversible UI change.

XERJ: project MCP search for SpreadsheetRibbon, followed by current-source inspection.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/game-data-pill/Verification.md)

- 원문 SHA-256: `26b7864994264ba8c2f70b9e7e649143fd65b822c8143182db6da3e4cb09406f`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
