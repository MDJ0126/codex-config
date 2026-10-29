---
title: Workbook taskbar thumbnail and temporary peek
date: 2026-10-26T08:14:21.093Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Workbook taskbar thumbnail and temporary peek — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/taskbar-hover/Verification.md
source_sha256: 9e18157dc2eb4939a8366df81a105e9b88607aca526c057368f6dab0607076bc
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Implementation: a custom card appears after 350 ms, with a one-second non-activating document peek. Leaving both tab and card restores visibility, input handling and stacking. Clicking commits the restoration without hide/show flicker. ESC, drag, context actions, focus change, close, assembly reload and editor shutdown cancel temporary state. Both visible entries and the overflow list are supported. DWM supplies live thumbnails for visible windows; a bounded pre-minimize snapshot supplies hidden-window thumbnails. A snapshot capture failure falls back to the document name until live rendering is available. This uses the project document shelf; it does not modify the Windows taskbar.

Validation: final .NET verification build passed with zero warnings/errors against installed Unity 6000.0.83f1 assemblies. 45 hover-policy, placement, installed-API, input-filter and interop checks passed. Existing taskbar overflow regression: 153 assertions passed. git diff --check passed. All nine guides contain one localized hover section; English remains default.

Not verified: actual Unity/Windows visual appearance, native thumbnail capture/DWM output, native click-through, focus/stack restoration, mixed-DPI behavior and UI screenshot recapture. The user prohibited computer use, so no actual native interaction was exercised. Localized-doc verification remains failing for 16 pending captures and 17 stale source hashes (including pre-existing pending work); no other documentation errors. Historical screenshot/source hashes were preserved. This is a source change, not a freshly exported Unity package.

Implementation API references: [DWM thumbnail properties](https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ns-dwmapi-dwm_thumbnail_properties), [PrintWindow](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-printwindow), [SetWindowSubclass](https://learn.microsoft.com/en-us/windows/win32/api/commctrl/nf-commctrl-setwindowsubclass), [WM_NCHITTEST](https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest). These documents informed the implementation; reading them is not native execution verification.

XERJ: used explicit project MCP index designdatatable-code-v3-docs for workbook taskbar symbols, then validated current sources. New implementation sources inspected directly.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/taskbar-hover/Verification.md)

- 원문 SHA-256: `9e18157dc2eb4939a8366df81a105e9b88607aca526c057368f6dab0607076bc`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
