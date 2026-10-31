---
title: 프로젝트 XLSX 아이콘 배경 제거
date: 2026-10-31T10:54:09.429Z
version: v1.0.1
status: QA 중
state: verification
project: designdatatable
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 배경 덮개와 이중 그리기를 없애고 투명 네이티브 아이콘을 사용합니다.
format: tracefolio-v2
priority: normal
manual_id: transparent-workbook-project-icon-20261031
round: 2
manual_state: qa
payload: ../assets/submissions/2026-10-31-manual-transparent-workbook-project-icon-20261031-20261031105409429-v1.0.1.json
---

## 목표와 완료 기준

README처럼 프로젝트 배경·선택색이 자연스럽게 보이는 XLSX 아이콘

완료 기준: 가져오기 아이콘 알파·색상·메인/시트 참조·재가져오기와 빌드 확인. 실제 화면은 조작 금지에 따라 미검증 명시.

후속 2회차 · 순차 진행 · 전체 2개 · 이번 실행 T01, T02

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 불투명 배경·이중 그리기 제거 | XLSX를 투명 네이티브 썸네일로 등록하고 중복 오버레이 생략; 이전 로고 삭제 및 현재 아이콘으로 참조 정리 | 프로젝트 선택 배경을 칠하지 않고 파일 아이콘 색상이 유지됨 | 실제 가져오기 객체·아이콘 투명 픽셀과 색상 검사 | 완료 |
| T02 | 비대화형 회귀·웹 보고 | 메인 자산·숨긴 썸네일·시트 참조와 테마 캐시 검사 후 게시 | 회귀 통과 및 화면 미검증을 구분한 공개 HTTPS 보고서 | Unity batchmode로 검사하고 배포·본문 접근 확인 | QA 대기 |

## QA 반복 기록

### Q01 · step · 2026-10-31T10:54:09.092109+00:00

실제 importer 네이티브 아이콘 검증

- T01 r2 · pass: 투명 네이티브 캐시 RGBA, 소스 보존, 참조 유지 및 이전 로고 삭제 확인 · 근거: assets/native-icon-qa/results.json
- T02 r1 · not_run: headless19검사 통과. 실제 화면 검증·재촬영 미실행 · 근거: 화면 조작 금지

## 남은 작업과 다음 반복

Project 창의 선택·포커스·Ping 실화면 및 가이드 재촬영 대기

이전 회차: [메뉴얼 열기](../../../projects/designdatatable/reports/2026-10-31-manual-20261031104404193-v1.0.0.html)

## 작업 시간

- 제안 시간: 5~10분
- 실제로 완료된 시간: 2026-10-31 19:39:12 KST 시작
- XERJ: 사용 — XERJ WorkbookAssets.DrawProjectIcon 검색 후 현재 소스 확인
