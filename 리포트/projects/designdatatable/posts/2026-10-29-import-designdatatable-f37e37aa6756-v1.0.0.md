---
title: Immediate thumbnail switching
date: 2026-10-26T11:28:01.600Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Immediate thumbnail switching — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-immediate-switch/Verification.md
source_sha256: ef925f21afa5d8f4497bade7425df9f0920171e143d397441436fad44623da8b
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

When a custom thumbnail is already visible, moving to another exposed workbook taskbar entry now replaces its content in the same native host immediately. The transition does not hide and reopen the host. An inactive cached host does not bypass the initial 400ms delay. Leaving both card and entries through the existing grace period resets the session. Full document peek continues to require 700ms over the new card; it does not start merely from a taskbar switch.

Validation: .NET build passed with zero warnings/errors. All 81 taskbar hover assertions passed, including immediate repeated retargets, fresh-session delay, leave/reset, separate full-document dwell and existing restoration checks. No native interaction or visual flicker claim is established by these checks. No computer use was performed.

Nine localized guide descriptions were regenerated. Documentation verification found no errors beyond existing pending/stale screenshot capture requirements. Native capture remains pending under the user's computer-use prohibition. Historical screenshot/source hashes were preserved.

XERJ MCP located WorkbookHoverTiming; the current implementation and its shelf caller were read before editing.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-immediate-switch/Verification.md)

- 원문 SHA-256: `ef925f21afa5d8f4497bade7425df9f0920171e143d397441436fad44623da8b`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
