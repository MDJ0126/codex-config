---
title: Immediate custom thumbnail; card-only peek
date: 2026-10-26T08:36:38.627Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Immediate custom thumbnail; card-only peek — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/taskbar-immediate-hover/Verification.md
source_sha256: 3f3597024b79ef664cc1341c1e8a2a3bc394d425e9a317c4641b6a6c90ba496d
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The taskbar thumbnail opens immediately and does not expire while the entry or card is hovered. The full-size document only peeks while the card itself is hovered, with no dwell delay. Leaving the card immediately restores native window visibility/input/stacking, while returning to the taskbar entry retains the card. Leaving both dismisses the card after the existing 160ms transit gap. The previous 400ms re-open cooldown was removed. Click-to-restore and close confirmation remain in place.

Validation: build passed, zero warnings/errors. 52 hover assertions cover immediate thumbnail, no peek on entry, immediate peek on card, no expiry at long elapsed times, return-to-tab behavior, cancel, placement, input filtering, ABI and native rectangle matching. 153 existing overflow assertions passed. Nine language guide sections updated; English remains default. git diff --check passed.

Actual UI interaction and screenshot recapture were not performed because the user prohibited computer use. Documentation validation remains failing solely for existing pending captures and changed source hashes; these are not passing UI verification. Historical screenshot/source hashes were preserved. No temporary runtime diagnostic probe was added for this change.

XERJ: project MCP search for WorkbookHoverTiming followed by direct inspection of current sources.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/taskbar-immediate-hover/Verification.md)

- 원문 SHA-256: `3f3597024b79ef664cc1341c1e8a2a3bc394d425e9a317c4641b6a6c90ba496d`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
