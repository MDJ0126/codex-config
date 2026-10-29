---
title: VA·CA 자동 실행 비교
date: 2026-10-17T17:21:51.548Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: VA·CA 자동 실행 비교 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/automated-runner-20261018/20261018-021854-OptionsVerified/comparison.html
source_sha256: 0e498c430de99d6ae444a028a6329ee8dc6757a74135d3fc58fa9d7ef1f3f781
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

VA·CA 자동 실행 비교

## VA·CA 자동 실행 비교

108개 항목의 제한된 자동 프로브입니다. 옵션 페이지 확인·대화상자 취소는 기능 검증 통과가 아닙니다. 명령 후보 실패는 원본 기능 부재를 뜻하지 않습니다. 전체 기능·단축키·테마·성능 유사도는 미확인입니다.

전체 유사도 미확인 · 완전 기능 인증 0/108

| 시나리오 | 기능 ID | 기능명 | 검증 범위 | 판정 | 양쪽 실행 | UI 관찰 | 호출 오류 | 소요 ms |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| feature-001 | 1 | Find by Context | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 3274, 'CA': None} |
| feature-002 | 2 | Find References | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2690, 'CA': None} |
| feature-008 | 8 | List Methods in File | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2215, 'CA': None} |
| feature-013 | 13 | VA Hashtags | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2267, 'CA': None} |
| feature-045 | 45 | Format After Paste | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2147, 'CA': None} |
| feature-052 | 52 | Smart Suggestions | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2180, 'CA': None} |
| feature-060 | 60 | Enhanced Syntax Coloring | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2065, 'CA': None} |
| feature-074 | 74 | Underline Spelling Errors | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2044, 'CA': None} |
| feature-076 | 76 | Code Inspection Settings | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2406, 'CA': None} |
| feature-077 | 77 | Code Issues in the Editor | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2179, 'CA': None} |
| feature-081 | 81 | for Refactoring | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2266, 'CA': None} |
| feature-092 | 92 | VA Step Filter | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2098, 'CA': None} |
| feature-095 | 95 | Simple Link Patterns | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2246, 'CA': None} |
| feature-102 | 102 | Custom File Extensions | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2069, 'CA': None} |
| feature-104 | 104 | Mouse | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2269, 'CA': None} |
| feature-106 | 106 | Non-Microsoft Build Processes | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2421, 'CA': None} |
| feature-107 | 107 | Parse Concurrency | options-page-only | needs-review | {'VA': 'ui-cancel-scope-only', 'CA': 'missing-evidence'} | {'VA': 'dialog-observed-and-cancelled', 'CA': None} | {'VA': None, 'CA': None} | {'VA': 2316, 'CA': None} |

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/automated-runner-20261018/20261018-021854-OptionsVerified/comparison.html)

- 원문 SHA-256: `0e498c430de99d6ae444a028a6329ee8dc6757a74135d3fc58fa9d7ef1f3f781`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
