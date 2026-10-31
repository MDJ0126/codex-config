---
title: 공통 창 관리 최적화와 강한 스트레스 QA
date: 2026-10-31T06:35:26.126Z
version: v1.0.2
status: 완료
state: complete
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 현재 소스의 시간·할당·호출 수를 먼저 측정하고 최적화 후 같은 부하와 실제 창 회귀를 비교합니다.
format: tracefolio-v2
priority: normal
manual_id: window-performance-stress
round: 3
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-window-performance-stress-20261031063526126-v1.0.2.json
---

## 목표와 완료 기준

동작과 창 상태를 보존하면서 측정된 공통 창 관리 비용을 줄인다.

완료 기준: 기준/개선 측정, 고부하 무창 상태·자원 검사, 실제 다중 창 전환·최소화/복원·프레임 회귀와 HTML 보고 완료.

후속 3회차 · 순차 진행 · 전체 4개 · 이번 실행 T01, T02, T03, T04

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 기준 성능 측정 | 1/8/32/128개 창의 등록·관찰 비용과 호출·할당을 측정한다. | 현재 병목과 기준 수치 확인 | 동일 런타임·입력·워밍업·측정 조건 보존 | 완료 |
| T02 | 공통 코드 리팩토링 | 측정된 반복 등록·탐색·상태 조회를 단순화한다. | 기존 API·동작 유지 및 측정 비용 감소 | 동일 기준 재측정과 상태 모델 대조 | 완료 |
| T03 | 강한 스트레스와 회귀 | 무작위 상태·다중 창·수명 반복·기존 깜박임 검사를 실행한다. | 잘못된 창 복원·상태 손실·자원 누적 없음 | 대량 반복·최대 부하·실패 이력 보존 | 완료 |
| T04 | 실제 실행과 게시 | 분리 Unity의 실제 전환·최소화·복원·중간 프레임을 검증하고 게시한다. | 실제 검증 근거와 한계를 포함한 HTML | 현재 로드 소스 확인·실제 클릭·화면·빌드/링크·외부 URL | 완료 |

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
### Q04 · final · 2026-10-31T15:35:15.357597+09:00

최종 실제 반복·논리·회귀·계측 한계와화면 대조 완료

- T01 r1 · pass: 유효 기준 측정 및 계측기 한계 확인. · 근거: Artifacts/WindowStress/2026-10-31;9238f018; 실제 PNG/WebP와 JSON
- T02 r3 · pass: 읽기 전용 등록 스냅샷·관찰 범위 공유·미저장 제목 수정9238f018 푸시. · 근거: Artifacts/WindowStress/2026-10-31;9238f018; 실제 PNG/WebP와 JSON
- T03 r3 · pass: 11259개 논리 검증,206개 회귀,2400개 실제 핸들 대조 및수명·빈 목록·1000회 관찰 해제 통과. · 근거: Artifacts/WindowStress/2026-10-31;9238f018; 실제 PNG/WebP와 JSON
- T04 r3 · pass: 실제24개 창4000회와상태 보존,실제60클릭 및248프레임 대조·HTML 생성 검증. · 근거: Artifacts/WindowStress/2026-10-31;9238f018; 실제 PNG/WebP와 JSON

## 남은 작업과 다음 반복

완전한 무할당·누수 없음으로 보고하지 않습니다. Unity의 GC.GetAllocatedBytesForCurrentThread는1MiB 시험 할당에도0, Process.PrivateMemorySize64도0을 반환해 해당 값은 사용할 수 없습니다. 별도 PowerShell 메모리 표본의 관측 최대는 약1.91GiB이며 특정 표본 구간의 값입니다. 생성·파괴 후 Mono 사용량은 기존약7.06→10.83MB, 개선약7.07→10.81MB로 비슷하게 증가했습니다. 이것만으로 장기 누수 여부는 확정할 수 없습니다.

검증은 Windows/Unity6000.0.83f1의 분리 QA 프로젝트에서 수행했습니다. 다른OS·DPI·수시간 이상 부하·전체 Pixora 변환/내보내기 성능·모든 제품 버전 조합은 이번 범위 밖입니다. 판매용 unitypackage를 새로 만들지 않았고 동시에 진행 중이던 다른 변경은 커밋하지 않았습니다.

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-31-manual-window-performance-stress-20261031054621221-v1.0.1.html)

## 작업 시간

- 제안 시간: 30~50분
- 실제로 완료된 시간: 시작2026-10-31 14:22:38 KST. 구현·실행 검증·코드 푸시 후 보고서 생성까지72.6분 · 2026-10-31 15:35:15 KST. 게시 확인은 생성 후 별도 수행합니다.
- XERJ: 사용 — XERJ PreserveVisiblePeers 검색 후 현재 공통/호스트 소스와 대조.
