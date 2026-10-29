---
title: 실제 재검증 결과
date: 2026-10-18T08:24:26.071Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 실제 재검증 결과 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/verification-095.html
source_sha256: fd639f6fe2a1c5f7ffd4131adc2ca502d6adbb269a91a0c1fa16e7910cc548b8
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

검사 중 C++ 데이터베이스 위치 확인 모달이 키 입력을 가로막은 것을 화면·UI Automation 문구로 확인했습니다. 해당 검사 솔루션의 임시 DB 위치를 확인한 뒤 정상 재실행했습니다.

**버퍼·커서·선택·Undo 포함 4/4 통과(100%). 기본 단축키 명령 수신 21/22(95.45%). 전체 제품 동기화율이 아닙니다.**

선택 확장, 범위 이동, 괄호 삽입, 감싸기 및 Undo가 원본 VA 실행 결과와 일치합니다. 이전 안내창 간섭 실행의 실패를 제품 결함으로 단정한 보고를 정정합니다.

이름 바꾸기: 임시 Ctrl+Shift+F11에서 CA 명령과 native Rename 명령 수신·창 표시를 확인하고 변경 없이 취소했습니다. 원래 Alt+Shift+R은 여전히 미수신이며 근본 원인은 미확인입니다. 키 소유 명령 목록과 저수준 훅 진단을 남겼으나 특정 외부 프로그램 탓으로 단정하지 않습니다. 임시 키 설정 27개 원복 확인.

## scope-next 통과

```
{
  "id": "scope-next",
  "physicalInput": true,
  "expected": {
    "text": "void First() {}\r\nvoid Second() {}\r\n",
    "caret": 17,
    "selectionStart": 17,
    "selectionLength": 0,
    "selectedText": "",
    "zoom": 100,
    "activeFile": "C:\\Projects\\CodeAssist\\.local\\parity-runs\\20261018-015759-745\\VA\\epoch-18976\\scope-next.cpp"
  },
  "before": {
    "text": "void First() {}\r\nvoid Second() {}\r\n",
    "caret": 0,
    "selectionStart": 0,
    "selectionLength": 0,
    "selectedText": ""
  },
  "after": {
    "text": "void First() {}\r\nvoid Second() {}\r\n",
    "caret": 17,
    "selectionStart": 17,
    "selectionLength": 0,
    "selectedText": ""
  },
  "undo": null,
  "actionCommands": [
    {
      "guid": "{F9D77864-FAC8-457E-BD4F-552715151B6C}",
      "id": 269
    }
  ],
  "undoCommands": null,
  "bufferCursorSelectionMatch": true,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": true,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-095-final-verified/scope-next-before.png) [after 촬영](../assets/imports/docs/validation/behavior-095-final-verified/scope-next-after.png) 

## pair-insert 통과

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
  "actionCommands": [],
  "undoCommands": null,
  "bufferCursorSelectionMatch": true,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": true,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-095-final-verified/pair-insert-before.png) [after 촬영](../assets/imports/docs/validation/behavior-095-final-verified/pair-insert-after.png) 

## smart-select 통과

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
    "caret": 30,
    "selectionStart": 16,
    "selectionLength": 14,
    "selectedText": "int value = 1;"
  },
  "undo": null,
  "actionCommands": [
    {
      "guid": "{F9D77864-FAC8-457E-BD4F-552715151B6C}",
      "id": 274
    }
  ],
  "undoCommands": null,
  "bufferCursorSelectionMatch": true,
  "undoRequired": false,
  "undoMatchesVA": false,
  "passed": true,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-095-final-verified/smart-select-before.png) [after 촬영](../assets/imports/docs/validation/behavior-095-final-verified/smart-select-after.png) 

## surround-word 통과

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
    "text": "int value;",
    "caret": 9,
    "selectionStart": 4,
    "selectionLength": 5,
    "selectedText": "value"
  },
  "actionCommands": [
    {
      "guid": "{5EFC7975-14BC-11CF-9B2B-00AA00573819}",
      "id": 684
    },
    {
      "guid": "{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}",
      "id": 1990
    }
  ],
  "undoCommands": [
    {
      "guid": "{5EFC7975-14BC-11CF-9B2B-00AA00573819}",
      "id": 43
    },
    {
      "guid": "{5EFC7975-14BC-11CF-9B2B-00AA00573819}",
      "id": 684
    },
    {
      "guid": "{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}",
      "id": 1990
    }
  ],
  "bufferCursorSelectionMatch": true,
  "undoRequired": true,
  "undoMatchesVA": true,
  "passed": true,
  "error": null,
  "oracleFile": ".local\\parity-runs\\20261018-015759-745\\VA.results.json"
}
```

[before 촬영](../assets/imports/docs/validation/behavior-095-final-verified/surround-word-before.png) [after 촬영](../assets/imports/docs/validation/behavior-095-final-verified/surround-word-after.png) [undo 촬영](../assets/imports/docs/validation/behavior-095-final-verified/surround-word-undo.png) 

자동 검사기에 모달 사전 차단, 명령 수신 추적, 키 유지 시간, 원본 해시 검증을 추가했습니다. 제품 설치 파일은 0.0.94를 유지하며 일반 사용자 VS 0.0.88은 종료·교체하지 않았습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/verification-095.html)

- 원문 SHA-256: `fd639f6fe2a1c5f7ffd4131adc2ca502d6adbb269a91a0c1fa16e7910cc548b8`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
