---
title: VA·CA 자동 실행 비교
date: 2026-10-17T15:15:24.799Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: VA·CA 자동 실행 비교 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/automated-runner-20261018/20261018-001144-713/comparison.html
source_sha256: 1dbd0b56dba86d2ae2f4f65b8baf728c3f5c4dfaa4fd7db38540e4a13366c668
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
| pair-insert | 46 | needs-review | {'VA': 'missing-evidence', 'CA': 'wrong-active-document'} | {'VA': None, 'CA': 2118} |
| pair-delete | 46 | needs-review | {'VA': 'missing-evidence', 'CA': 'expectation-failed'} | {'VA': None, 'CA': 814} |
| surround-word | 55 | needs-review | {'VA': 'missing-evidence', 'CA': 'observed'} | {'VA': None, 'CA': 894} |
| comment-continuation | 43 | needs-review | {'VA': 'missing-evidence', 'CA': 'expectation-failed'} | {'VA': None, 'CA': 809} |
| reset-zoom | 49 | needs-review | {'VA': 'missing-evidence', 'CA': 'observed'} | {'VA': None, 'CA': 621} |
| smart-select | 51 | needs-review | {'VA': 'missing-evidence', 'CA': 'observed'} | {'VA': None, 'CA': 558} |
| scope-next | 9 | needs-review | {'VA': 'missing-evidence', 'CA': 'observed'} | {'VA': None, 'CA': 573} |
| scope-previous | 9 | needs-review | {'VA': 'missing-evidence', 'CA': 'observed'} | {'VA': None, 'CA': 707} |

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/automated-runner-20261018/20261018-001144-713/comparison.html)

- 원문 SHA-256: `1dbd0b56dba86d2ae2f4f65b8baf728c3f5c4dfaa4fd7db38540e4a13366c668`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
