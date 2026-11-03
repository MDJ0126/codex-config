---
title: Affinity 저장 권한 복구 지침
date: 2026-11-03T16:36:53.314Z
version: v1.0.1
status: 완료
state: complete
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 권한 오류 발생 시 프로젝트 폴더 권한을 원인별로 복구하고 실제 저장까지 검증하도록 합니다.
format: tracefolio-v2
priority: normal
manual_id: affinity-project-permission-recovery
round: 1
manual_state: complete
payload: ../assets/submissions/2026-11-03-manual-affinity-project-permission-recovery-20261103163653314-v1.0.1.json
---

## 목표와 완료 기준

저장 오류를 반복·방치하지 않고 프로젝트 범위의 권한 복구와 실제 저장 검증을 공통화합니다.

완료 기준: 전역 지침·공개 가이드 통합, 설치본 동기화·빌드·푸시·배포·공개 본문 확인

최초 1회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 복구 지침 통합·동기화·공개 | 전역 지침·공개 가이드 통합, 설치본 동기화·빌드·푸시·배포·공개 본문 확인 | 전역 지침·공개 가이드 통합, 설치본 동기화·빌드·푸시·배포·공개 본문 확인 | 사용자 요구·기존 예외·프로젝트 경계·실제 저장 검증 의무·동일 해시·공개 본문 대조 | 완료 |

## QA 반복 기록

### Q01 · final · 2026-11-03T16:36:52.343Z

요구·범위·동기화·공개 문구 대조

- T01 r1 · pass: 지침3곳 동일 SHA256·945개 빌드·33,344개 링크 검사·공개 가이드2곳과 원격 지침 본문 확인 · 근거: 548f2dc0; SHA256 0EE1256B33236040655A204814E26713EA82107CD063BD173BB28EE0E327E9CF; assets/permission-policy-2026-11-04/public-qa.json

## 남은 작업과 다음 반복

없음. 실제 프로젝트 폴더 권한을 바꾸거나 저장 장애를 복구한 작업은 아닙니다.

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-11-03-manual-20261103163406898-v1.0.0.html)

## 작업 시간

- 제안 시간: 3~5분
- 실제로 완료된 시간: 시작 2026-11-04 01:33:01 KST · 지침 공개 확인 2026-11-03T16:36:52.345Z · 최종 기록 배포 확인은 채팅에 표기
- XERJ: 미사용 — 코드 검색 불필요 — 지침·문서 수정
