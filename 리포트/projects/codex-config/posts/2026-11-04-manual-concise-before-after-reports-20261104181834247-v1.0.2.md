---
title: 간결한 보고·분할 작성·전후 화면 기준 적용
date: 2026-11-04T18:18:34.247Z
version: v1.0.2
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
round: 2
manual_state: active
payload: ../assets/submissions/2026-11-04-manual-concise-before-after-reports-20261104181834247-v1.0.2.json
---

## 목표와 완료 기준

읽기 쉬운 보고와 실제 전후 비교를 공통 작성 기준으로 적용합니다.

완료 기준: 지침·가이드 변경과 빌드·링크 검사를 통과하고 커밋·푸시·공개 URL을 확인합니다.

후속 2회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 보고 기준 통합·검증·공개 | 수정 범위별 전후 증거와 간결한 요약 기준을 통합하고 지침 동기화·빌드·공개 배포를 확인합니다. | 다수 수정에 다수 전후 쌍을 요구하고 누락 대조·요약·기존 예외가 명시됩니다. | 사용자 요구·기존 조건·지침 동일 해시·공개 가이드 본문 대조 | 진행 |

## QA 반복 기록

### Q01 · final · 2026-11-01T21:58:16.506Z

지침·생성·공개 본문 대조

- T01 r1 · pass: 지침3곳 해시 일치, 718개 보고서 빌드·26,378개 링크 검사 통과, 공개 가이드 본문 확인. · 근거: 커밋 fc19ae94; guide/index.html 및 guide/execution.html 공개 HTTP200·요구 문구 확인

## 남은 작업과 다음 반복

공개 배포 확인 대기

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-11-01-manual-concise-before-after-reports-20261101215816937-v1.0.1.html)

## 작업 시간

- 제안 시간: 지침 수정·동기화 약 1~2분, 공개 배포 확인 별도
- 실제로 완료된 시간: 시작 2026-11-05 03:17:30 KST · 진행 중
- XERJ: 미사용 — 코드 검색 불필요 — 문서·지침 수정
