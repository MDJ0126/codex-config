---
title: Preview frame continuity
date: 2026-10-26T12:13:06.964Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Preview frame continuity — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-frame-continuity/Verification.md
source_sha256: 155325aa6963de2c48f2ddc43c0710f9d5a4b89049b9576132acd11ef63e9059
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

User screenshots showed an image-free temporary card with smaller text and a different position, followed by the full native card with an image. Source confirmed independent GDI/IMGUI title renderers, snapshot-null composition and an overlay position read from the native window rather than the requested placement.

Changes:
- Use the requested card bounds for both overlay creation and final native presentation.
- Read back the complete composed bitmap after title/close text rendering and display those exact bytes in the final card as a Unity texture. Close hover, restore hit testing and edge hover remain interactive.
- Obtain a visible source snapshot if the shelf did not supply one. Never fade an image-free card when no source snapshot is available.
- Retain minimized snapshots as serialized PNG bytes across script reload, restoring their textures on shelf enable. Newly captured snapshots remain CPU-readable for encoding.
- Clean up card-owned snapshots and shared presentation textures on retarget/disable.

Existing minimized entries from before this version have no serialized snapshot bytes. Opening and minimizing one such document once generates the initial snapshot. Subsequent code reloads retain it.

No source workbook was saved or closed by tools. No computer use was performed. Actual visual confirmation of the corrected frame/position remains pending. Screenshot/source hashes were preserved; the native screenshot backlog remains open.

Build passed with zero warnings/errors. 88 hover and 153 taskbar assertions passed, including offscreen GDI pixel round-trip and selected-only raise ordering. Documentation checks found no issues beyond pending/stale native screenshot requirements. Native visual confirmation remains pending.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-frame-continuity/Verification.md)

- 원문 SHA-256: `155325aa6963de2c48f2ddc43c0710f9d5a4b89049b9576132acd11ef63e9059`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
