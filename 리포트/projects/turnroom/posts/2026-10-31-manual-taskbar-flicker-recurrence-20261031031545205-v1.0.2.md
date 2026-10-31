---
title: 최소화 목록 선택 시 전체 창 깜박임 재발 조사
date: 2026-10-31T03:15:45.205Z
version: v1.0.2
status: 완료
state: complete
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 이전 공개 버그의 접관·재정렬 경로를 대조하고 재발 방지 회귀를 추가합니다.
format: tracefolio-v2
priority: normal
manual_id: taskbar-flicker-recurrence
round: 3
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-taskbar-flicker-recurrence-20261031031545205-v1.0.2.json
---

## 목표와 완료 기준

목록 선택 중 불필요한 숨김/표시와 순차 전체 창 재정렬을 제거한다.

완료 기준: 과거 공개 보고서·현재 코드·회귀 결과 대조, 선택한 창과 다른 창 상태 보존, 실제 네이티브/화면 검증은 제어 재개 후 수행.

후속 3회차 · 순차 진행 · 전체 3개 · 이번 실행 T02, T03

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 기존 재발 기록 대조 | 이전 접관 안정성과 교차 선택 버그를 현재 경로와 비교한다. | 이미 수정한 경로와 재발 우회 경로 구분 | 공개 HTML 및 실제 소스/읽기 전용 이벤트 관찰 | 완료 |
| T02 | 공통 선택과 접관 수정 | 미리보기 클릭 취소 순서와 공통 창 재정렬을 수정한다. 실제 메인 창 덮임을 공통 가드로 수정한다. | 취소 후 재표시와 순차 중간 화면 감소 | 가짜 lease·선택 정책·원래 창 순서/최소화 회귀 | 완료 |
| T03 | 실행 검증과 보고 | 무창 회귀 및 패키지/보고서를 검증한다. | 실행한 범위와 미검증 네이티브 상태 구분 | 누르기 구간 실제 프레임·이벤트, 목록12회, 최소화·복원4회,peek,오버플로·공통 목록 복원 | 완료 |

## QA 반복 기록

### Q01 · regression · 2026-10-30T23:03:33.287852+09:00

과거 기록/현재 경로 대조와198개 회귀; 네이티브 시각 검증 보류

- T01 r1 · pass: 공개 이전 보고·현재 경로 대조 및198개 무창 회귀 통과. · 근거: Artifacts/FlickerRecurrence; 이전 공개 보고서 HTTP200
- T02 r1 · pass: 공개 이전 보고·현재 경로 대조 및198개 무창 회귀 통과. · 근거: Artifacts/FlickerRecurrence; 이전 공개 보고서 HTTP200
- T03 r1 · blocked: 컴퓨터 제어 중단 지시가 유지돼 실제 네이티브 클릭·중간 프레임 검증을 수행하지 않았습니다. · 근거: Artifacts/FlickerRecurrence; 이전 공개 보고서 HTTP200
### Q02 · final · 2026-10-31T12:15:44.749707+09:00

실제 재현→수정→프레임·상태·회귀 QA

- T01 r1 · pass: 공개 이전 보고·현재 경로 대조 및198개 무창 회귀 통과. · 근거: Artifacts/FlickerRecurrence/QA-20261031; 실제 before/after WebP;868f9ecd
- T02 r2 · pass: 868f9ecd 수정·푸시,198개 회귀와 실제 프레임 검증 통과. · 근거: Artifacts/FlickerRecurrence/QA-20261031; 실제 before/after WebP;868f9ecd
- T03 r2 · pass: 분리 Unity에서 실제 재현→수정→최종 반복 QA 통과. 확대 검증 한계는 보고서에 기록. · 근거: Artifacts/FlickerRecurrence/QA-20261031; 실제 before/after WebP;868f9ecd

## 남은 작업과 다음 반복

해결 범위는 위 Windows/Unity 버전에서 재현한 목록 클릭 중 메인 창 덮임입니다. 다른 운영체제·DPI 배율·장시간 스트레스·모든 미저장 문서 내용의 전체 회귀는 미실행입니다. 드래그·닫기 입력 코드는 유지했지만 이번 실제 재검증은 별도입니다. 판매용 패키지를 새로 만들지 않았습니다. 사용 중인 원래 에디터의 소스 리로드 여부는 별도이며 실행 증거는 분리 QA 에디터입니다.

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-30-manual-taskbar-flicker-recurrence-20261030140343997-v1.0.1.html)

## 작업 시간

- 제안 시간: 10~20분. 추가 원인 수정 후 게시 검증에5~10분 추가 예상 보고.
- 실제로 완료된 시간: 시작2026-10-31 11:50:19 KST. 구현·실행QA·코드 푸시 확인 후 보고서 생성까지25.4분 · 2026-10-31 12:15:44 KST. 외부 배포는 생성 후 별도 확인합니다.
- XERJ: 사용 — XERJ RestorePeekFromAnchor 검색 후 현재 소스·Win32 이벤트·실제 프레임 대조. 최초 노드 중단은 검색 재실행으로 복구했습니다.
