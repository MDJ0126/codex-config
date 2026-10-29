---
title: Custom preview hover and fade
date: 2026-10-26T11:40:39.908Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Custom preview hover and fade — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-hover-fade/Verification.md
source_sha256: c56ca558458cda817c375332315d67af2a4f2da6e8f203ba75388ab159d69dc7
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The custom thumbnail card now interpolates its background, outer border and image outline on pointer hover (90ms). Native pointer state is supplied by the existing shelf poll, so the visual does not depend on Unity being foreground. The close button keeps its red hover cue and existing hit test.

A newly displayed card uses a 120ms smoothstep alpha fade. Only the custom card HWND is passed to PreviewOpacity; document peek/restore HWNDs are not changed. Windows SetLayeredWindowAttributes with LWA_ALPHA provides whole-host alpha. The style bit is retained rather than toggled every frame. Alpha is prepared while hidden before the initial show. A visible reused host retains its animation state, so immediate retargeting does not fade out/restart. A hidden reused host starts a fresh fade. If native alpha setup fails, the code requests full opacity rather than leaving a hidden interactive card.

Build passed with zero warnings/errors. 85 hover checks passed, covering fade endpoints/intermediate stages, existing timing/retarget behavior, click restoration, placement and native layout contracts. These are headless checks; actual native transparency, compositor/DWM interaction and hover appearance remain unverified. No computer use was performed. The existing screenshot recapture backlog remains open; historical source/image hashes were preserved. All nine language descriptions were regenerated; documentation verification reported only pending/stale screenshot requirements.

Native API reference consulted:
https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setlayeredwindowattributes

XERJ located WorkbookHoverPreview; current source and native preview implementation were inspected before editing.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-hover-fade/Verification.md)

- 원문 SHA-256: `c56ca558458cda817c375332315d67af2a4f2da6e8f203ba75388ab159d69dc7`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
