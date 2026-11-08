---
title: 간결한 보고·분할 작성·전후 화면 기준 적용
date: 2026-11-08T07:45:51.430Z
version: v1.0.4
status: 작업 중
state: progress
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 지침과 공개 작성 가이드에 핵심 요약·주제별 분할·전후 화면 기준을 반영합니다.
format: tracefolio-v2
priority: normal
manual_id: concise-before-after-reports
round: 3
manual_state: active
payload: ../assets/submissions/2026-11-08-manual-concise-before-after-reports-20261108074551430-v1.0.4.json
---

## 목표와 완료 기준

읽기 쉬운 보고와 실제 전후 비교를 공통 작성 기준으로 적용합니다.

완료 기준: 지침·가이드 변경과 빌드·링크 검사를 통과하고 커밋·푸시·공개 URL을 확인합니다.

후속 3회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 보고 기준 통합·검증·공개 | 작업 대화 기록의 발언자·실제 순서·핵심 인용·반영 결과를 보고서에 남기는 기준을 추가하고 원본 세션 로그 금지와 구분해 동기화·공개합니다. | 대화 기록 의무·긴 기록 분리·민감정보 제외·기존 보고 형식·원본 로그 보호가 공존합니다. | 사용자 요구와 기존 지침 대조·설치본 동일성·빌드·링크·공개 본문 검사 | 진행 |

## QA 반복 기록

### Q01 · final · 2026-11-01T21:58:16.506Z

지침·생성·공개 본문 대조

- T01 r1 · pass: 지침3곳 해시 일치, 718개 보고서 빌드·26,378개 링크 검사 통과, 공개 가이드 본문 확인. · 근거: 커밋 fc19ae94; guide/index.html 및 guide/execution.html 공개 HTTP200·요구 문구 확인
### Q03 · final · 2026-11-04T18:21:18.117Z

수정 범위별 촬영·요약 규칙과 공개 본문 확인

- T01 r2 · pass: 전역3곳 동일 해시, 1131개 보고서·39,057개 링크 검사 및 공개 가이드2곳·원격 지침 문구 확인 · 근거: 9802f14f; SHA256 B659EF656B9D5A37EE55A4471FBCCB5DCD9FE87EC42AE686B23F2739624721E3; assets/screenshot-coverage-2026-11-05/public-qa.json

## 남은 작업과 다음 반복

공개 배포 확인 대기

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-11-04-manual-concise-before-after-reports-20261104182118708-v1.0.3.html)

## 작업 시간

- 제안 시간: 지침 수정·동기화 약 1~2분, 공개 배포 확인 별도
- 실제로 완료된 시간: 시작 2026-11-08 16:44:40 KST · 진행 중
- XERJ: 미사용 — 코드 검색 불필요 — 문서·지침 수정
