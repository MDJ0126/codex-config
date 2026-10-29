---
title: Minimize motion
date: 2026-10-28T16:15:13.166Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Minimize motion — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-29/minimize-motion/Results.md
source_sha256: 26d1201562e29b9e8d01f4a3f1e441af5c91afca778a64f65cf8d6e4e8690797
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

A captured workbook surface shrinks/moves to the cached real taskbar-entry anchor over 0.24 s, fading during the final 35%. Overflow entries use the overflow-control anchor. Only the source document is hidden; original placement and edit state are retained. Restore cancels the surface before normal existing restoration. Hover previews wait until the flight ends. Compile, close and shutdown clean up the bitmap surface. Missing snapshot/target or unsupported native rendering falls back to the existing immediate minimize.

54 native checks passed: initial/final bounds, monotonic size, opacity, actual UpdateLayeredWindow geometry, bitmap readbacks at five time points, no black frames, cancellation, GDI cleanup, unchanged source window geometry/styles and unchanged foreground. No visible window was presented to the user's desktop. The actual desktop minimize-button-to-taskbar and restoration/focus gestures were not exercised; these remain a limitation of this screenless verification. Existing taskbar/hover regressions were run separately.

The guide image is an actual native renderer frame using a workbook screenshot; offscreen test geometry is synthetic. It is not a full desktop animation recording. Nine language guide text records this distinction. Historical stale screenshots remain outside this task; no overall documentation verification pass is claimed.

XERJ server was not running; rg was used after the status check.

Regression results: 101 taskbar-hover assertions and 153 overflow/list assertions passed. These headless regressions do not claim physical desktop interaction.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-29/minimize-motion/Results.md)

- 원문 SHA-256: `26d1201562e29b9e8d01f4a3f1e441af5c91afca778a64f65cf8d6e4e8690797`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
