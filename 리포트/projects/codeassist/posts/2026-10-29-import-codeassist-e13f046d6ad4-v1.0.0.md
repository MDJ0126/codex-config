---
title: 실제 결과 자동 비교 0.0.94
date: 2026-10-18T08:24:26.071Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: 실제 결과 자동 비교 0.0.94 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/behavior-094.html
source_sha256: a2a1a699279726bf46d13f270f49382c328f867d71835b3813a0e9e170d3e03f
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

**정정: 별도 프로필의 이전 실패 실행은 C++ DB 안내창 간섭으로 무효입니다. [4/4 정상 재검증 결과](../assets/imports/docs/verification-095.html)를 확인하세요.**

## 실제 결과 자동 비교 0.0.94

일반 VS 설치는 0.0.88임을 확인했습니다. 기존 일반 VS를 유지하고 별도 CodeAssistAudit 프로필에 0.0.93을 설치한 후 0.0.94 패키지를 스테이징·실행했습니다. 두 프로필 결과를 혼합해 제품 전체 점수로 표시하지 않습니다.

비교기 자체 반례 검사 8/8 통과. 실제 입력 3/4개, 버퍼·커서·선택 결과 일치 2/3개(66.67%), Undo 포함 엄격 통과 1/4개(25%). 범위 이동 1개는 포커스 단계 차단으로 미검증입니다. 기능 108개 전체 일치율이 아닙니다.

제품 수정: 지원되지 않는 이동 명령 안내를 모달창에서 상태 표시줄+로그로 변경해 후속 입력 차단을 방지했습니다. VS2019/Modern 패키지 빌드 및 6개 바이너리·48개 키 등록 검증 통과. 모달 제거 수정의 실제 해당 경로 재검증은 남았습니다.

## scope-next · 미통과

```
{
  "id": "scope-next",
  "physicalInput": false,
  "expected": {
    "text": "void First() {}\r\nvoid Second() {}\r\n",
    "caret": 17,
    "selectionStart": 17,
    "selectionLength": 0,
    "selectedText": "",
    "zoom": 100,
    "activeFile": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\20261018-015759-745\\VA\\epoch-18976\\scope-next.cpp"
  },
  "before": null,
  "after": null,
  "undo": null,
  "bufferCursorSelectionMatch": false,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": false,
  "error": "Editor does not have keyboard focus",
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

## pair-insert · 통과

```
{
  "id": "pair-insert",
  "physicalInput": true,
  "expected": {
    "text": "()",
    "caret": 1,
    "selectionStart": 1,
    "selectionLength": 0,
    "selectedText": "",
    "zoom": 100,
    "activeFile": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\20261018-015759-745\\VA\\epoch-18976\\pair-insert.cpp"
  },
  "before": {
    "text": "",
    "caret": 0,
    "selectionStart": 0,
    "selectionLength": 0,
    "selectedText": ""
  },
  "after": {
    "text": "()",
    "caret": 1,
    "selectionStart": 1,
    "selectionLength": 0,
    "selectedText": ""
  },
  "undo": null,
  "bufferCursorSelectionMatch": true,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": true,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/pair-insert-before.png) [after 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/pair-insert-after.png) 

## smart-select · 미통과

```
{
  "id": "smart-select",
  "physicalInput": true,
  "expected": {
    "text": "void Sample() { int value = 1; }\r\n",
    "caret": 30,
    "selectionStart": 16,
    "selectionLength": 14,
    "selectedText": "int value = 1;",
    "zoom": 100,
    "activeFile": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\20261018-015759-745\\VA\\epoch-18976\\smart-select.cpp"
  },
  "before": {
    "text": "void Sample() { int value = 1; }\r\n",
    "caret": 25,
    "selectionStart": 20,
    "selectionLength": 5,
    "selectedText": "value"
  },
  "after": {
    "text": "void Sample() { int value = 1; }\r\n",
    "caret": 25,
    "selectionStart": 20,
    "selectionLength": 5,
    "selectedText": "value"
  },
  "undo": null,
  "bufferCursorSelectionMatch": false,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": false,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/smart-select-before.png) [after 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/smart-select-after.png) 

## surround-word · 미통과

```
{
  "id": "surround-word",
  "physicalInput": true,
  "expected": {
    "text": "int(value);",
    "caret": 10,
    "selectionStart": 10,
    "selectionLength": 0,
    "selectedText": "",
    "zoom": 100,
    "activeFile": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\20261018-015759-745\\VA\\epoch-18976\\surround-word.cpp"
  },
  "before": {
    "text": "int value;",
    "caret": 9,
    "selectionStart": 4,
    "selectionLength": 5,
    "selectedText": "value"
  },
  "after": {
    "text": "int(value);",
    "caret": 10,
    "selectionStart": 10,
    "selectionLength": 0,
    "selectedText": ""
  },
  "undo": {
    "text": "int(value);",
    "caret": 10,
    "selectionStart": 10,
    "selectionLength": 0,
    "selectedText": ""
  },
  "bufferCursorSelectionMatch": true,
  "undoRequired": true,
  "undoMatchesVA": false,
  "passed": false,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/surround-word-before.png) [after 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/surround-word-after.png) [undo 촬영](../assets/imports/docs/validation/behavior-094-installed-retry/surround-word-undo.png) 

## 실행

```
tools/ShortcutProbe/RunBehavior.ps1 -TargetProcessId <검사용 VS PID>
```

원본 VA 증거 SHA256 검증, 실제 편집기 포커스 확인, 동일 입력·선택 설정, OS 키 전송, 버퍼/커서/선택/Undo 비교, 단계별 촬영, 실패 시 비정상 종료를 구현했습니다. 사용자 소스를 수정하지 않고 생성된 검사 파일만 편집했습니다. 임시 바인딩 3개 원복 확인.

미완료: 선택 확장 무반응, 감싸기 Undo, 범위 이동 포커스, 이름 바꾸기 단축키. 기존 68.25% 임시 종합 지수는 이번 엄격 검사 결과로 상향하지 않습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/behavior-094.html)

- 원문 SHA-256: `a2a1a699279726bf46d13f270f49382c328f867d71835b3813a0e9e170d3e03f`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
