---
title: Full document preview fade without movement
date: 2026-10-26T12:30:48.345Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Full document preview fade without movement — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/document-peek-fade/Verification.md
source_sha256: 203e1703317067c8a2feae28e31e0272aa97821f633194b1e8bea76b72ee4fa7
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The full-size temporary document preview now uses the same separate bitmap-overlay fade at the document's existing bounds, with an explicit zero Y offset at creation and every update. The custom thumbnail card retains its existing 8px rise. No native document alpha/layered style is changed.

The source bitmap uses the latest visible document capture or the saved pre-minimize snapshot when hidden. Newly retained snapshots are bounded to 1920x1200 instead of 320x200 to reduce enlargement artifacts in the full-size preview. Existing smaller snapshots remain valid until refreshed by a subsequent minimize.

The overlay is ordered below the custom card and declines activation/input. The real source is revealed at its unchanged position only after the fade reaches opacity. Clicking during the fade reveals and adopts it immediately. Canceling before reveal disposes only the overlay; canceling after reveal restores original visibility/z-order. The fade uses EditorApplication.update and unsubscribes on cancel/commit/finish/reload. The card live thumbnail is refreshed when the source is revealed.

No computer use was performed. Actual native visual appearance remains pending user confirmation. Nine guide descriptions were regenerated; historical screenshot/source hashes were preserved. Documentation errors are limited to existing pending/stale native screenshots.

XERJ located PeekSession; its actual subclass, visibility, commit and cancel paths were read before editing.

Validation: build passed with zero warnings/errors. 101 hover and 153 taskbar assertions passed, including pre-reveal cancellation vs post-reveal restoration and commit placement behavior. Native fade appearance/position still requires user confirmation.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/document-peek-fade/Verification.md)

- 원문 SHA-256: `203e1703317067c8a2feae28e31e0272aa97821f633194b1e8bea76b72ee4fa7`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
