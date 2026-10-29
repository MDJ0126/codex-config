---
title: Taskbar context menu isolation
date: 2026-10-26T12:22:13.505Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Taskbar context menu isolation — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/taskbar-context-isolation/Verification.md
source_sha256: d15b890e25739038f0d82038f381c49bb5f5579b0b648619c847fbcd7b618e6f
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

User observation: right-clicking a minimized entry opened its document, the menu and a thumbnail at the same time.

Source findings:
- ShowContext supplied the minimized document as WorkbookPopup.Show's owner.
- That generic path attached and raised the document when the popup opened.
- Hover polling had no menu-open suppression, allowing a new thumbnail/peek while the menu was active.

Changes:
- Taskbar menus take a separate unowned popup route; they do not call Attach or RaiseForPopup for their minimized document. Automatic inherited-owner assignment explicitly excludes active taskbar menus.
- The menu tracks its active native EditorWindow lifecycle.
- Opening context blocks hover and cancels the current card/fade/peek. While a taskbar menu is open, UpdateHover cancels and returns before input/peek processing.
- After dismissal, the hover gate requires the pointer to leave the taskbar entry before a fresh preview session can begin.
- Restore/maximize/close actions remain explicit menu commands. Ordinary tool popups retain their document ownership.

Actual right-click/native-window interaction remains pending user verification; no computer use was performed. Existing workbook data and dirty edits were not saved or closed by tools. All nine guide descriptions were regenerated; historical screenshot/source hashes were preserved, and native screenshot verification remains pending.

XERJ located WorkbookTaskbarMenu; current menu, shelf and popup ownership implementations were read before editing.

Validation: build succeeded with zero warnings/errors. 95 hover and 153 taskbar assertions passed, including taskbar-vs-ordinary-popup ownership routing and menu/open/dismiss/leave/re-entry suppression. Documentation verification found no issues beyond pending/stale native screenshot requirements. Actual native interaction remains unverified.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/taskbar-context-isolation/Verification.md)

- 원문 SHA-256: `d15b890e25739038f0d82038f381c49bb5f5579b0b648619c847fbcd7b618e6f`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
