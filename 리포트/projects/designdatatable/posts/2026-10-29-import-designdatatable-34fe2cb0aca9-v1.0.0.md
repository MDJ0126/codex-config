---
title: Creation menus and cell input
date: 2026-10-27T12:41:05.235Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Creation menus and cell input — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/create-input/Verification.md
source_sha256: 98478f9b77d5bac2173bd89b14b909d3c1524e7540b6bd2c5166bbad68256532
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

- Build: zero warnings/errors. Source creation/persistence checks: 10 passed.
- Native Unity 6000.0.83f1 English menu inspected: three format choices; Standard Excel created a blank worksheet.
- Real Unity EditorWindow.SendEvent key sequence and IMGUI repaint processing: a → ab → abc → abcd, Enter advances one row; x → xy, Tab advances one column. Saved XLSX independently inspected: A2=abcd, A3=xy.
- Before correction the same probe produced bcd, then suppressed the next cell input. Explicit TextEditor.OnFocus/caret initialization and clearing editingTextField after commit fixed the recorded paths.
- Hardware keyboard delivery and IME composition were not tested. Computer Use typing activated the main Unity window, so it was not treated as a valid document input test.
- Test windows closed; temporary editor probe removed. User's unsaved Localized.xlsx preserved.
- Nine-language creation/input instructions rebuilt; pre-existing stale documentation captures are reported by VerifyLocalizedDocs, not marked current.
- XERJ MCP searched successfully, but its result did not identify the needed file; current source verified with rg. Local launcher status/recovery could not connect from sandbox.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/create-input/Verification.md)

- 원문 SHA-256: `98478f9b77d5bac2173bd89b14b909d3c1524e7540b6bd2c5166bbad68256532`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
