---
title: Rise with thumbnail fade
date: 2026-10-26T12:24:44.399Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Rise with thumbnail fade — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-rise/Verification.md
source_sha256: 4f881358bb338bbe17dc7a3eeb36986f59263a1538edb2aff057a9ef4ed0e0bf
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

New preview cards now rise from 8 logical pixels below the final position while fading over the existing 120ms duration. Both alpha and Y position use the same smoothstep progress and the same UpdateLayeredWindow call. The DPI-scaled overlay offset reaches exactly zero before the real card is exposed at its existing final bounds. No document HWND or document group is moved. Existing visible-card retargets remain immediate and do not replay the entrance.

Validation: build passed with zero warnings/errors. 98 hover and 153 taskbar assertions passed, including start/midpoint/end position, no overshoot, fade, right-click menu suppression and independent document raise ordering. All nine guide descriptions were regenerated. Documentation checks report only pending/stale native screenshot captures. Actual native visual appearance remains pending user verification; no computer use or screen capture was performed and historical hashes were preserved.

XERJ located WorkbookPreviewAnimation; the actual overlay update path was inspected before editing.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-rise/Verification.md)

- 원문 SHA-256: `4f881358bb338bbe17dc7a3eeb36986f59263a1538edb2aff057a9ef4ed0e0bf`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
