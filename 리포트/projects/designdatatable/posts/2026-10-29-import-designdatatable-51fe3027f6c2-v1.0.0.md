---
title: Bottom status reservation and subtle worksheet curves
date: 2026-10-26T09:46:03.674Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Bottom status reservation and subtle worksheet curves — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/status-strip-curves/Verification.md
source_sha256: 88ca97c9327d4f4c70b7f3d7aaedd1709112654bcf8558f437ecf47c8971c303
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The console-message row above the document shelf is Unity's native status row. Saved project layout evidence shows MainView.bottomViewHeight=20 while the AppStatusBar view persisted height=48 (the native 20 plus our 28). Old attach logic used the current view height as originalHeight and added another 28. Reloading that expanded rectangle produces 76, leaving a 28px spare area in the wrapped native row. See SavedLayoutEvidence.json; the user layout itself was only read, not edited.

Fix: use the parent MainView configured native height, reserve exactly native+28, preserve the bottom edge, and release exactly the native row. Clipping confines original content to its row. When no documents remain, recognized orphan DDT height increments are repaired without changing unrelated heights. Repeated-reservation and detach/reattach checks verify idempotence and recovery.

Both WorkbookViewer and DataTableEditor worksheet lists now use small 5px rounded corners and inset selection lines. Order, sizing, scrolling, selection/context actions, source names and semantic colors are retained. Existing 2px/3px selection-line thicknesses are retained in their respective editors. No changes were made to workbook content.

Validation: final .NET build against Unity 6000.0.83f1 libraries passed with zero warnings/errors. Status geometry: 92 assertions; existing hover: 54; overflow: 153; total 299. git diff --check passed. All nine guides have the new localized bottom-strip note; English remains default.

Not verified: actual Unity rendering, black-gap disappearance, native console/progress interaction and rounded tab appearance. Computer use and UI capture remain prohibited by the user. Screenshot/source hashes were preserved; documentation verification continues to fail for pending/stale captures rather than claiming visual verification. No runtime probe was installed.

XERJ: project MCP search for WorkbookMinimizedShelf, followed by current source and saved-layout inspection. Unity reference source was consulted only as API/layout context; implementation is project-authored.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/status-strip-curves/Verification.md)

- 원문 SHA-256: `88ca97c9327d4f4c70b7f3d7aaedd1709112654bcf8558f437ecf47c8971c303`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
