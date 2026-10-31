---
title: 공통 창 관리 최적화와 강한 스트레스 QA
date: 2026-10-31T05:46:21.221Z
version: v1.0.1
status: 작업 중
state: progress
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 현재 소스의 시간·할당·호출 수를 먼저 측정하고 최적화 후 같은 부하와 실제 창 회귀를 비교합니다.
format: tracefolio-v2
priority: normal
manual_id: window-performance-stress
round: 2
manual_state: active
payload: ../assets/submissions/2026-10-31-manual-window-performance-stress-20261031054621221-v1.0.1.json
---

## 목표와 완료 기준

동작과 창 상태를 보존하면서 측정된 공통 창 관리 비용을 줄인다.

완료 기준: 기준/개선 측정, 고부하 무창 상태·자원 검사, 실제 다중 창 전환·최소화/복원·프레임 회귀와 HTML 보고 완료.

후속 2회차 · 순차 진행 · 전체 4개 · 이번 실행 T01, T02, T03, T04

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 기준 성능 측정 | 1/8/32/128개 창의 등록·관찰 비용과 호출·할당을 측정한다. | 현재 병목과 기준 수치 확인 | 동일 런타임·입력·워밍업·측정 조건 보존 | 완료 |
| T02 | 공통 코드 리팩토링 | 측정된 반복 등록·탐색·상태 조회를 단순화한다. | 기존 API·동작 유지 및 측정 비용 감소 | 동일 기준 재측정과 상태 모델 대조 | 진행 |
| T03 | 강한 스트레스와 회귀 | 무작위 상태·다중 창·수명 반복·기존 깜박임 검사를 실행한다. | 잘못된 창 복원·상태 손실·자원 누적 없음 | 대량 반복·최대 부하·실패 이력 보존 | 대기 |
| T04 | 실제 실행과 게시 | 분리 Unity의 실제 전환·최소화·복원·중간 프레임을 검증하고 게시한다. | 실제 검증 근거와 한계를 포함한 HTML | 현재 로드 소스 확인·실제 클릭·화면·빌드/링크·외부 URL | 대기 |

## QA 반복 기록

### Q01 · step · 2026-10-31T14:46:00.399304+09:00

기준 성능과 기존 소스 대조

- T01 r1 · pass: 1/8/32/128개 창의 기준 측정 완료. GC 할당 API는1MiB 시험 할당에도0을 반환해 사용할 수 없음을 확인. · 근거: WorkFiles/Temp/WindowStressQA/Baseline.json;Before-WindowServices.cs;Before-WindowFocus.cs
### Q02 · step · 2026-10-31T14:46:00.399304+09:00

초기 최적화와 논리 스트레스

- T02 r1 · pass: 128개 포커스500회1440.93ms→33.79ms,조회128500→64000.11259개 논리 검증 통과. · 근거: Optimized-Stress.json
### Q03 · regression · 2026-10-31T14:46:00.399304+09:00

실제 부하 실패 보존

- T04 r1 · fail: 실제3000회 선택 후 최소화15번째 실패. MiniGameBalance.xlsx*와Unity제목 불일치,동일위치다중창에서핸들없음.2회재현. · 근거: NativeStress-Failed-First.json;NativeStress-Failed-Diagnosed.json

## 남은 작업과 다음 반복

측정·구현·스트레스·실제 회귀·게시 진행 예정.

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-31-manual-20261031052458244-v1.0.0.html)

## 작업 시간

- 제안 시간: 30~50분
- 실제로 완료된 시간: 시작2026-10-31 14:22:38 KST. 진행 중.
- XERJ: 사용 — XERJ PreserveVisiblePeers 검색 후 현재 공통/호스트 소스와 대조.
