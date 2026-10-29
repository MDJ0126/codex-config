---
title: VA·CA 자동 실행 비교
date: 2026-10-17T15:56:03.265Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: VA·CA 자동 실행 비교 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/automated-runner-20261018/20261018-005515-925/comparison.html
source_sha256: 38deb20db64270b966ea1934afb222e26eb0e5425b076ba0b28bf6c1e9a228dd
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

VA·CA 자동 실행 비교

## VA·CA 자동 실행 비교

Command-chain scenarios only. Scenario matches do not certify whole features, physical shortcuts, UI, themes, other languages or performance parity.

전체 유사도 미확인 · 완전 기능 인증 0/108

| 시나리오 | 기능 | 판정 | 양쪽 실행 | 소요 ms |
| --- | --- | --- | --- | --- |
| pair-insert | 46 | scenario-match | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1247, 'CA': 1066} |
| pair-delete | 46 | needs-review | {'VA': 'undo-restoration-failed', 'CA': 'observed'} | {'VA': 1296, 'CA': 1286} |
| surround-word | 55 | needs-review | {'VA': 'expectation-failed', 'CA': 'observed'} | {'VA': 1216, 'CA': 1132} |
| comment-continuation | 43 | needs-review | {'VA': 'expectation-failed', 'CA': 'expectation-failed'} | {'VA': 1164, 'CA': 1123} |
| reset-zoom | 49 | scenario-match | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1021, 'CA': 990} |
| smart-select | 51 | different | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1033, 'CA': 971} |
| scope-next | 9 | different | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1158, 'CA': 948} |
| scope-previous | 9 | different | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1004, 'CA': 928} |
| insert-bracket | 46 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1038, 'CA': 1075} |
| surround-bracket | 55 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 987, 'CA': 946} |
| insert-brace | 46 | different | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1047, 'CA': 968} |
| surround-brace | 55 | different | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1021, 'CA': 975} |
| insert-quote | 46 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1077, 'CA': 972} |
| surround-quote | 55 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1013, 'CA': 946} |
| insert-apostrophe | 46 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 999, 'CA': 945} |
| surround-apostrophe | 55 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1036, 'CA': 944} |
| comment-indented | 43 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1004, 'CA': 933} |
| comment-multiline | 43 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1005, 'CA': 943} |
| comment-star | 43 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1023, 'CA': 948} |
| dot-to-arrow | 69 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1094, 'CA': 992} |
| repair-case | 73 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1010, 'CA': 957} |
| acronym | 42 | needs-review | {'VA': 'no-op', 'CA': 'no-op'} | {'VA': 1545, 'CA': 1474} |
| shorthand | 50 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1546, 'CA': 1638} |
| completion | 44 | unasserted-agreement | {'VA': 'observed', 'CA': 'observed'} | {'VA': 1516, 'CA': 1471} |

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/automated-runner-20261018/20261018-005515-925/comparison.html)

- 원문 SHA-256: `38deb20db64270b966ea1934afb222e26eb0e5425b076ba0b28bf6c1e9a228dd`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
