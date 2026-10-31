---
title: 이미지 작업 Affinity 필수·출력 품질 검토 규칙
date: 2026-10-31T12:06:39.622Z
version: v1.0.1
status: 완료
state: complete
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 이미지 제작·편집은 실제 Affinity 사용을 필수화하고 임의 대체와 품질 검토 생략을 금지합니다.
format: tracefolio-v2
priority: normal
manual_id: affinity-image-required
round: 1
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-affinity-image-required-20261031120639622-v1.0.1.json
---

## 목표와 완료 기준

이미지 작업을 실제 Affinity 기반으로 수행하고 편집 가능한 원본·출력 품질을 검증하도록 합니다.

완료 기준: 충돌 문구를 정리하고 설치 지침·가이드를 동기화·푸시·배포한 뒤 개별 공개 보고서를 확인합니다.

최초 1회차 · 순차 진행 · 전체 2개 · 이번 실행 T01, T02

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | Affinity 필수·대체 금지·품질 규칙 통합 | 기존 이미지 우선 사용·대체 허용 문구를 필수 사용과 품질 확인 규칙으로 교체합니다. | 실제 앱 연결·원본·출력 품질·사용자 화면 비간섭이 명시되며 임의 이미지 생성 대체가 금지됩니다. | 기존 문구와 요구를 대조하고 CLI doctor/status 읽기 전용 연결 결과를 확인합니다. | 완료 |
| T02 | 전역 동기화·공개 가이드·결과 보고 | 설치본/저장소 해시·빌드·링크를 확인하고 Sync.ps1 Push 및 Pages 외부 접근을 검증합니다. | 전역 지침이 동일하고 공개 가이드·개별 보고서가 인증 없이 열립니다. | 해시·Actions·개별 URL 본문 확인, 실제 이미지 제작은 미실행으로 구분 | 완료 |

## QA 반복 기록

### Q01 · step · 2026-10-31T12:04:09.990Z

기존 지침 충돌 해소와 실제 연결 읽기 검사

- T01 r1 · pass: 이미지 Affinity 필수·임의 대체 금지·출력 품질 검토를 통합했습니다. doctor와 status 연결 성공이며 이미지 편집/출력은 이번 문서 작업에서 실행하지 않았습니다. · 근거: assets/affinity-required-2026-10-31/policy-review.json; 전역3곳 해시9C9038BC2301EA08C91DB79052DC8DE4709C0C461652DA16B1BEF1B248D5C0EB
### Q02 · final · 2026-10-31T12:06:39.348Z

지침 충돌 제거·전역 동기화·공개 문서 최종 QA

- T01 r1 · pass: 기존 충돌 문구를 교체하고 실제 출력 품질 검토·원본 보존·캡처 진실성 조건을 추가했습니다. Affinity3.3.1 연결·상태 조회를 확인했으며 문서는 수정하지 않았습니다. · 근거: assets/affinity-required-2026-10-31/policy-review.json 및 public-qa.json; SHA256 9C9038BC2301EA08C91DB79052DC8DE4709C0C461652DA16B1BEF1B248D5C0EB; Actions41199490830 success
- T02 r1 · pass: 전역3곳 해시 일치·Sync.ps1 Push·Actions41199490830 성공과 인증 없는 공개 가이드·개별 메뉴얼·원격 지침4곳을 확인했습니다. · 근거: assets/affinity-required-2026-10-31/policy-review.json 및 public-qa.json; SHA256 9C9038BC2301EA08C91DB79052DC8DE4709C0C461652DA16B1BEF1B248D5C0EB; Actions41199490830 success

## 남은 작업과 다음 반복

정책 적용·첫 공개 검증 완료. 실제 이미지 재제작은 이번 지침 변경 요청의 범위가 아닙니다.

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-10-31-manual-20261031120412750-v1.0.0.html)

## 작업 시간

- 제안 시간: 4~7분
- 실제로 완료된 시간: 시작2026-10-31 21:01:23 KST · 정책·가이드 공개 확인 2026-10-31T12:06:39.348Z · 5.3분. 결과 기록의 최종 공개 확인은 채팅에 표기합니다.
- XERJ: 미사용 — 코드 검색 불필요 — 지침·문서 대조 및 Affinity 읽기 전용 연결 확인
