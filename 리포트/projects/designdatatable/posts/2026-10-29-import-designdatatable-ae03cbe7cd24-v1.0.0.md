---
title: Restore preview card chrome
date: 2026-10-26T11:46:53.885Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Restore preview card chrome — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-chrome-restore/Verification.md
source_sha256: dcd2a949a489257e0117c5e8527dfa0ea8862d6355528dafd9b2406ff79f85e6
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

User observation: after the previous fade change, the thumbnail remained visible while the custom card background and title disappeared. The source still contained the title/background draw calls. The preceding change had applied Win32 layered alpha directly and retained WS_EX_LAYERED after completion. Exact native compositor behavior was not instrumented, so the complete rendering cause is not runtime-verified.

Changes:
- Restore the original 1px card inset, original background color, title/icon/close layout and accent strip.
- Hover changes only the outer border and a thin image outline; no background recoloring.
- Remove direct SetLayeredWindowAttributes calls.
- Use Unity ContainerWindow.SetAlpha(float), verified present in installed Unity 6000.0.83f1 by reflection and in official Unity 6000.0 reference source.
- At fade completion/failure, request full host opacity, clear residual WS_EX_LAYERED on the custom card, and immediately repaint it. Document window opacity is untouched.
- Keep the 120ms fade and immediate retarget behavior.

Validation: build succeeded with zero warnings/errors; all 86 hover assertions passed, including installed host alpha API availability and prior hover/click/timing checks. Actual visual restoration, hover appearance and fade remain pending user verification. No computer use or screenshot capture was performed. Nine guide descriptions were regenerated. Documentation checker errors are limited to existing pending/stale native screenshot captures; historical hashes were preserved.

Official reference:
https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Editor/Mono/ContainerWindow.bindings.cs

XERJ located PreviewOpacity; its current source and the complete preview card implementation were read before editing.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-chrome-restore/Verification.md)

- 원문 SHA-256: `dcd2a949a489257e0117c5e8527dfa0ea8862d6355528dafd9b2406ff79f85e6`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
