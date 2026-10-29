---
title: Independent workbook restore and minimize
date: 2026-10-26T11:52:11.569Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Independent workbook restore and minimize — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/independent-window-restore/Verification.md
source_sha256: abb1967b62e6d2d4ba516ddfb56a0073dfa1187b3fc7f9a2141d9e4ef625b282
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

User reported that opening one workbook exposed all workbooks and minimizing one lowered them all. This is accepted as the observed runtime behavior. The prior transparency experiments were not native-verified and are fully removed in this change. Their causal involvement is not established by runtime instrumentation.

Source-confirmed behavior before this change:
- Restore supplied every logically visible peer HWND to ActivateDocument, which explicitly raised the entire list.
- RaiseForPopup also supplied all visible peers, so opening an associated popup could expose other documents.
- Minimize hid its target without synchronously detaching document owners. Owner detachment otherwise happened on a periodic 250ms update.

Changes:
- Restore activates/raises only its selected HWND, with owner detachment before showing it.
- Popup raising applies to the associated document only.
- Minimize normalizes the ownership of visible workbook windows before hiding its one target. Other entry flags are untouched.
- All SetAlpha/SetLayeredWindowAttributes calls and whole-window fade timers are removed. Card layout, border-only hover and immediate thumbnail retargets remain.
- The custom card clears a leftover layered style from the previous implementation when first shown.

Native per-window behavior remains pending user verification; no computer use or UI automation was performed. Existing data and dirty edit sessions were not saved or closed. Nine translated descriptions were regenerated; documentation errors are limited to pending/stale screenshot recapture requirements. Historical hashes were retained.

Build: zero warnings/errors. Hover suite: 82 assertions passed. Taskbar overflow suite: 153 assertions passed. The selected-only raise-order regression case passed. These checks do not execute native hide/show or establish actual on-screen isolation.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/independent-window-restore/Verification.md)

- 원문 SHA-256: `abb1967b62e6d2d4ba516ddfb56a0073dfa1187b3fc7f9a2141d9e4ef625b282`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
