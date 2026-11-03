---
title: 여러 폴더 조회 시 탐색기 대기 제거
date: 2026-11-03T07:33:14.036Z
version: v1.0.1
status: 작업 중
state: progress
project: windowsfoldersize
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 탐색기 크기 조회를 비동기 메모리 캐시로 분리하고 지연·무응답 브로커로 검증합니다.
format: tracefolio-v2
priority: normal
manual_id: windowsfoldersize-nonblocking-20261103
round: 1
manual_state: active
payload: ../assets/submissions/2026-11-03-manual-windowsfoldersize-nonblocking-20261103-20261103073314036-v1.0.1.json
---

## 목표와 완료 기준

다수 폴더 크기 조회가 브로커 응답을 기다리지 않게 수정

완료 기준: 무응답 서버·다수 조회에서 호출 즉시 반환, 캐시 갱신·장애 시 숫자 유지, 호환성 검사와 설치 파일 생성, 공개 보고. 작업 방해 금지에 따라 설치·탐색기 재시작은 보류.

최초 1회차 · 순차 진행 · 전체 4개 · 이번 실행 T01, T02, T03, T04

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 동기 대기 재현 | 현재 IPC 호출과 지연 서버의 실제 대기 시간 대조 | 문제 경로와 재현 수치 확인 | 20ms CallNamedPipe에 350ms 지연 응답 적용 | 완료 |
| T02 | 비동기 캐시 구현 | UI는 캐시만 조회, IPC는 제한 시간 있는 전용 스레드로 분리 | UI 경로에서 IPC와 대기 제거 | 무응답·지연·다수 폴더·숫자 유지 테스트 | 완료 |
| T03 | 회귀와 산출물 고정 | 네이티브 속성 표시 회귀, 빌드, MSI 구조와 해시 확인 | 테스트 통과 및 최신 단일 설치 파일 | headless 테스트 실행과 MSI 검증 | 완료 |
| T04 | 보고·공개 검증 | QA 근거와 적용 보류 사유를 공개 보고서로 게시 | 개별 보고서 외부 접근 성공 | 빌드·링크 검사와 배포 SHA·HTTP 본문 확인 | 진행 |

## QA 반복 기록

### Q01 · step · 2026-11-03T07:33:13.2203206Z

지연 응답의 동기 대기 재현

- T01 r1 · pass: 20ms 설정의 CallNamedPipe가 지연 응답에 364.37ms 대기 · 근거: src/async_cache_test.cpp 실제 named pipe 서버 350ms 지연
### Q02 · step · 2026-11-03T07:33:13.2203206Z

최초 캐시 경합 검증 실패 보존

- T02 r1 · fail: try_lock 경합 때 기본 queued 응답으로 돌아가 숫자 유지 검사 실패 · 근거: 첫 test-async 실행의 numeric retention FAIL
### Q03 · regression · 2026-11-03T07:33:13.2203206Z

호출 스레드별 마지막 스냅샷으로 경합 수정 후 재검증

- T02 r1 · pass: 10000회52.54ms, 최악136.90µs; 100ms IPC deadline111.81ms; 호출 스레드 I/O0; 숫자 유지 PASS · 근거: scripts/test-async.cmd 실행 결과
- T03 r1 · pass: 진행률·큐·실패·완료의 Windows 속성/COM 표시 모두 통과; MSI 검증0; SHA256일치 · 근거: DisplayTest·ProgressTest, WiX ICE 검사; SHA256 0161022c296a7e569c34a22fe3f63e02cca50853c7d838e1818d368a083fb3f8

## 남은 작업과 다음 반복

공개 배포 확인 중. 사용자 작업을 방해하지 않도록 설치·탐색기 재시작·실제 GUI 전후 촬영은 보류하며 UI 수용 검증 완료로 주장하지 않음.

이전 회차: [메뉴얼 열기](../../../projects/windowsfoldersize/reports/2026-11-03-manual-20261103072813288-v1.0.0.html)

## 작업 시간

- 제안 시간: 10~15분
- 실제로 완료된 시간: 2026-11-03 16:20:16 KST 시작; 코드·headless QA 완료, 공개 검증 진행
- XERJ: 미사용 — MCP 미제공, 프로젝트 검색 스크립트 없음; rg와 현재 소스로 대조
