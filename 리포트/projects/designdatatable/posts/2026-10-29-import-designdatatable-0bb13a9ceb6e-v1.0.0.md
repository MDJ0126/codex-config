---
title: Bitmap-only custom-card fade
date: 2026-10-26T11:57:32.232Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Bitmap-only custom-card fade — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-bitmap-fade/Verification.md
source_sha256: 7eef213dd538bc25d5939462befa08394a1679b213213b9d82fc03de674fbc48
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The requested 120ms fade-in is restored without SetAlpha or SetLayeredWindowAttributes on Unity/document windows. A temporary, ownerless STATIC bitmap overlay is created with WS_EX_LAYERED, WS_EX_NOACTIVATE, WS_EX_TOOLWINDOW and WS_EX_TRANSPARENT. PrintWindow captures only the already prepared custom-card HWND. UpdateLayeredWindow changes SourceConstantAlpha on that separate overlay. Once fully opaque, the real prepared card is shown before the bitmap overlay is disposed. All bitmap/DC/window resources are released on completion, cancellation, retarget or domain reload. Failed overlay preparation falls back to showing the original card.

The normal card design and border-only hover remain. Immediate visible retargets discard the old overlay and show the new original card without starting another fade. No document-window show/minimize/group code was changed by this implementation; the previous selected-only restore/owner-detachment fixes remain.

Validation: build passed with zero warnings/errors. Hover suite passed 85 assertions and taskbar overflow suite passed 153 assertions. Covered fade endpoints/midpoint, Win32 blend ABI, initial dwell, immediate retarget, card-only peek, selected-only raise ordering, restore hit areas and existing taskbar behavior. Source search confirms there are no SetAlpha/SetLayeredWindowAttributes calls in the window implementation.

Actual overlay bitmap capture, composited fade appearance and native per-window behavior remain unverified without user testing. No computer use was performed. No native screenshot was recaptured; historical image/source hashes were preserved. Nine localized descriptions were regenerated; documentation verification found only existing pending/stale screenshot requirements.

Primary API references consulted:
https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-updatelayeredwindow
https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-blendfunction

XERJ located WorkbookHoverPreview; current code and native thumbnail implementation were read before editing.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-bitmap-fade/Verification.md)

- 원문 SHA-256: `7eef213dd538bc25d5939462befa08394a1679b213213b9d82fc03de674fbc48`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
