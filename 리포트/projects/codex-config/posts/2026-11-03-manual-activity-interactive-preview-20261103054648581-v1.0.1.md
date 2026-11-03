---
title: 잔디 미리보기 링크·유지·반응 개선
date: 2026-11-03T05:46:48.581Z
version: v1.0.1
status: 완료
state: complete
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 보고서 바로가기와 마우스 유지, 반복 계산·DOM 생성을 개선합니다.
format: tracefolio-v2
priority: normal
manual_id: activity-interactive-preview
round: 1
manual_state: complete
payload: ../assets/submissions/2026-11-03-manual-activity-interactive-preview-20261103054648581-v1.0.1.json
---

## 목표와 완료 기준

보고서 바로가기와 마우스 유지, 반복 계산·DOM 생성을 개선합니다.

완료 기준: 제목 실제 이동·마우스/키보드 유지·필터·호버 계산/DOM 성능·공개 배포 확인

최초 1회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 미리보기 링크·상호작용·성능 개선 | 제목 실제 이동·마우스/키보드 유지·필터·호버 계산/DOM 성능·공개 배포 확인 | 제목 실제 이동·마우스/키보드 유지·필터·호버 계산/DOM 성능·공개 배포 확인 | KST 경계·0건·윤년·프로젝트/유형 조합과 날짜 클릭·모바일/테마 실제 확인 | 완료 |

## QA 반복 기록

### Q01 · step · 2026-11-03T05:46:47.941Z

초기 미리보기 이동 검사에서 닫힘 재현

- T01 r1 · fail: 스크롤 이벤트의 무조건 hide가 미리보기를 닫는 실제 스택을 확인했습니다. · 근거: activity-grass.js scroll listener; 포커스/포인터 유지 중에는 위치 갱신하도록 수정 후 재검증
### Q02 · final · 2026-11-03T05:46:47.942Z

이동·유지·재사용·공개 확인

- T01 r1 · pass: 브라우저24개·자동 테스트48개와 공개 링크·유지 검사 통과. 호버200회 새 DOM 1263개→0개, 날짜 선택 시 기존 달력 유지. · 근거: cce2af00; assets/activity-interactive-2026-11-03/browser-qa.json·public-qa.json·before-performance.json·after-performance.json

## 남은 작업과 다음 반복

없음

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-11-03-manual-20261103053855780-v1.0.0.html)

## 작업 시간

- 제안 시간: 6~10분
- 실제로 완료된 시간: 시작 2026-11-03 14:38:12 KST · 기능 공개 확인 2026-11-03T05:46:47.942Z · 결과 기록 최종 배포 시각은 채팅에 표기
- XERJ: 미사용 — 연결/검색 스크립트 미제공 — rg로 실제 소스 확인
