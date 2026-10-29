---
title: Commit restoration when clicking a preview card
date: 2026-10-26T10:03:00.813Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Commit restoration when clicking a preview card — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-click-restore/Verification.md
source_sha256: b2bad257bd7803e9cbf58b0c2027d9d5ee277da9bd5d50e698831206407ea3f1
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The user reported that clicking the custom card while full-size peek was shown did not keep the document restored.

Source findings: the native pointer path handled restore only over the taskbar entry. Card IMGUI restore handled the thumbnail image only; title and margins had no restore action. In addition, temporary ownership was not relinquished until after Commit returned, leaving cleanup order dependent on nested native callbacks.

Fix: hit-test the entire card except the close button. Handle an exposed native card click on mouse-down before focus-loss/hover cleanup, with a whole-card IMGUI fallback. The GUI fallback commits the native peek immediately, deferring only document activation/host closure until IMGUI unwinds. A shared lease owner detaches before Commit/Dispose, preventing reentrant cleanup or later mouse leave from hiding a committed document. Native PeekSession's existing commit removes input interception and retains visibility. Close-button behavior remains the normal document-close/save-confirmation path. ESC still cancels.

Validation: final .NET build against Unity 6000.0.83f1 libraries passed with zero warnings/errors. 76 hover checks cover title/image/margins, close exclusion, outside clicks, negative-coordinate monitors, reentrant commit/cancel and later cancellation after commit, alongside existing timing/geometry/ABI checks. Existing overflow: 153 checks; status layout: 92; total 321. git diff --check passed. Nine localized guides updated.

Limits: current native click behavior has not been manually confirmed. A retry question is pending. No computer-use automation, screen capture, native click injection or window activation was executed by tools. Documentation verification remains failing for pending/stale captures, with historical screenshot/source hashes preserved. Historical read-only runtime evidence from taskbar-hover-fix confirmed the native card was the pointer root while hovered; this supports the exposed-card guard, but is not a test of this new click fix.

Handoff: the user explicitly requested starting the agreed work in the existing Unity 에셋 페이지 작업 session. A message was sent to thread 01a17245-169b-7bc0-92bb-1576f0d86493, preserving the computer-use prohibition and warning that actual click restoration must be checked before sales filming. wait_threads returned active/inProgress, and the destination assistant acknowledged starting environment/material inspection and capture planning. No new thread or subagent was created, and no navigation/focus change was requested.

XERJ: project MCP search for WorkbookHoverPreview, followed by current-source and historical runtime-record inspection.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-click-restore/Verification.md)

- 원문 SHA-256: `b2bad257bd7803e9cbf58b0c2027d9d5ee277da9bd5d50e698831206407ea3f1`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
