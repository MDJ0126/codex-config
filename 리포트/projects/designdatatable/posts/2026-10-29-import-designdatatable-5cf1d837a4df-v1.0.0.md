---
title: Spreadsheet Play-mode brightness
date: 2026-10-28T14:55:48.602Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Spreadsheet Play-mode brightness — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-28/play-mode-brightness/Results.md
source_sha256: 946e80c8b7fa320c584bc593e23bb70902c5d943f4890580a18c7a88b064fba1
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

- Reproduced native Play-mode GUI.color = (0.8, 0.8, 0.8, 1), GUI.enabled = true.
- White cell pixels before fix: RGB (204, 204, 204).
- WorkbookViewer temporarily sets GUI.color to white only while drawing its contents and restores the previous value in finally.
- White cell pixels after fix in actual Play mode: RGB (255, 255, 255).
- Final Edit and Play captures have identical white background, header and type-row pixel colors at recorded sample points.
- Successful Play capture used WorkbookViewer.cs identical to production source.
- .NET build: 0 errors, 0 warnings. No input or saving policy changes.
- Actual non-presented Unity 6000.0.83f1 HostViews rendered and images visually inspected. No desktop input or preference changes.
- Nine language guides rebuilt and new screenshot manifest checks pass. Full documentation verification retains historical unrelated stale images and pending physical Create-menu capture; no overall pass claimed.
- Unity 6000.0 reference source confirms IMGUIContainer applies playModeTintColor to GUI.color: https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Modules/UIElements/Core/IMGUIContainer.cs
- XERJ not running; current source inspected using rg after status check.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-28/play-mode-brightness/Results.md)

- 원문 SHA-256: `946e80c8b7fa320c584bc593e23bb70902c5d943f4890580a18c7a88b064fba1`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
