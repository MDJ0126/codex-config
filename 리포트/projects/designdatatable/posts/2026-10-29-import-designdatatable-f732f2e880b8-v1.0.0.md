---
title: Adopt an already displayed peek without another restore transition
date: 2026-10-26T10:33:11.372Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Adopt an already displayed peek without another restore transition — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-adoption-stability/Verification.md
source_sha256: f0174cc3101afb2bc54ca71b9bed9db67532b2b0ac51c698b3edadf087e91262
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The user confirmed the previous click-restoration fix keeps the window visible, but reported flicker. The inspected code still sent a WINDOWPOS transition in PeekSession.Commit, called ShowWindow(SW_SHOW) because the logical entry remained minimized during peek, then ran managed Focus, peer-by-peer group restacking and another native activation. These redundant operations are concrete source findings; their exact pixel-level contribution was not measured.

Changes: Commit now removes the temporary input subclass without repositioning, hiding or showing the existing window. The lease coordinator returns whether a live peek was adopted. Both immediate and deferred card-click paths carry this flag into Restore. An adopted window skips ShowWindow and group restacking. Native foreground activation is requested only if it is not already foreground; ordinary restore and its peer-order behavior are unchanged. Managed Focus is a fallback if the minimal native activation fails. Hover delay, card click target coverage and mouse-leave persistence are retained.

Validation: final .NET build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors; 77 hover/ownership checks, 153 overflow and 92 status-layout checks passed (322 total). The deferred-adoption regression verifies the adoption result survives cleared temporary ownership, so queued restoration need not re-show the same window. git diff --check passed.

Actual Windows/Unity rendering, native focus/keyboard behavior and click-time flicker elimination remain unverified for this patch. A manual retry question was sent. No computer-use automation, screenshot capture, native mouse injection or foreground-window manipulation was executed by tools. Documentation descriptions remain accurate; capture/source-hash verification continues to report pending/stale captures rather than passing visual QA. Historical records and screenshot hashes were preserved.

XERJ: project MCP search for ActivateDocument, then current source inspection of commit, deferred restore and activation paths.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-adoption-stability/Verification.md)

- 원문 SHA-256: `f0174cc3101afb2bc54ca71b9bed9db67532b2b0ac51c698b3edadf087e91262`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
