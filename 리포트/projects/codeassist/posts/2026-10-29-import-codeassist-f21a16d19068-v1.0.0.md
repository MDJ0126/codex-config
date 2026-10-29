---
title: VA / CodeAssist 누적 대조 검증 — 현재 판정
date: 2026-10-19T03:35:12.198Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: VA / CodeAssist 누적 대조 검증 — 현재 판정 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/background-103/index.html
source_sha256: dd0513032b424a1bf8f1df76182645e503c37a9742bf259fb6bdec357be12004
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

## VA-F001 · Find by Context representative-match

같은 needle의 다음 참조로 이동: 텍스트·커서·선택 범위 일치

### VA

실행 원문 JSON

관찰 상태

```
{
  "observedEditorState": {
    "file": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\background-103\\VA\\context-next.cpp",
    "text": "void ContextProbe() { int needle = 1; needle += 2; int other = needle; }\r\n",
    "caret": 44,
    "selectionStart": 38,
    "selectionLength": 6,
    "selectedText": "needle"
  },
  "error": null
}
```

### CA

실행 원문 JSON

관찰 상태

```
{
  "observedEditorState": {
    "file": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\background-103\\CA\\context-next.cpp",
    "text": "void ContextProbe() { int needle = 1; needle += 2; int other = needle; }\r\n",
    "caret": 44,
    "selectionStart": 38,
    "selectionLength": 6,
    "selectedText": "needle"
  },
  "error": null
}
```

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/background-103/index.html)

- 원문 SHA-256: `dd0513032b424a1bf8f1df76182645e503c37a9742bf259fb6bdec357be12004`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
