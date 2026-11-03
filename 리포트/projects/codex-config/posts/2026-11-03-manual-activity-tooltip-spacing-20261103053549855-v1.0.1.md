---
title: 잔디 제목 툴팁·균일 간격 수정
date: 2026-11-03T05:35:49.855Z
version: v1.0.1
status: 완료
state: complete
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 날짜별 보고서 제목 미리보기와 균일한 셀 간격을 추가합니다.
format: tracefolio-v2
priority: normal
manual_id: activity-tooltip-spacing
round: 1
manual_state: complete
payload: ../assets/submissions/2026-11-03-manual-activity-tooltip-spacing-20261103053549855-v1.0.1.json
---

## 목표와 완료 기준

날짜별 보고서 제목 미리보기와 균일한 셀 간격을 추가합니다.

완료 기준: 실제 제목·빈 날짜·툴팁 화면 경계·상하좌우 동일 간격·필터·공개 배포 확인

최초 1회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 툴팁·간격 구현 및 검증 | 실제 제목·빈 날짜·툴팁 화면 경계·상하좌우 동일 간격·필터·공개 배포 확인 | 실제 제목·빈 날짜·툴팁 화면 경계·상하좌우 동일 간격·필터·공개 배포 확인 | KST 경계·0건·윤년·프로젝트/유형 조합과 날짜 클릭·모바일/테마 실제 확인 | 완료 |

## QA 반복 기록

### Q01 · final · 2026-11-03T05:35:48.515Z

툴팁·간격·기존 필터 최종 검증

- T01 r1 · pass: 브라우저19개·자동 테스트48개·공개5개 통과. 제목·빈 날짜·균일 간격·화면 경계·키보드·필터 확인. · 근거: b7bfb120; assets/activity-tooltip-2026-11-03/browser-qa.json 및 public-qa.json

## 남은 작업과 다음 반복

없음

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-11-03-manual-20261103053237547-v1.0.0.html)

## 작업 시간

- 제안 시간: 5~8분
- 실제로 완료된 시간: 시작 2026-11-03 14:31:37 KST · 기능 공개 확인 2026-11-03T05:35:48.516Z · 최종 기록 배포 시각은 채팅에 표기
- XERJ: 미사용 — 연결/검색 스크립트 미제공 — rg로 실제 소스 확인
