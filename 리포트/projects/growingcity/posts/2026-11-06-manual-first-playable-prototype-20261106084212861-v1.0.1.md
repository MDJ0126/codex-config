---
title: 첫 시제품 — 자동 성장·파괴 점수·재건
date: 2026-11-06T08:42:12.861Z
version: v1.0.1
status: QA 중
state: verification
project: growingcity
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 컴퓨터 유즈와 GUI 실행 없이 첫 시제품 씬·코드와 독립 검증을 작성합니다.
format: tracefolio-v2
priority: normal
manual_id: first-playable-prototype
round: 1
manual_state: qa
payload: ../assets/submissions/2026-11-06-manual-first-playable-prototype-20261106084212861-v1.0.1.json
---

## 목표와 완료 기준

건물 하나에서 자라는 도시를 파괴하고 점수를 얻으며 자동 재건하는 Unity 웹 대상 시제품

완료 기준: 전용 씬·편집 가능한 UI·설정·외부 에셋 참조 작성, 컴파일 및 핵심 로직 실행 검증. GUI·웹 실제 화면은 사용자 제약에 따라 미실행으로 구분

최초 1회차 · 순차 진행 · 전체 7개 · 이번 실행 T01, T02, T03, T04, T05, T06, T07

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 도시 자동 성장 | 도시 자동 성장 구현 후 소스 대조와 실행 가능한 독립 QA | 도시 자동 성장의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 완료 |
| T02 | 등장 효과 | 등장 효과 구현 후 소스 대조와 실행 가능한 독립 QA | 등장 효과의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | QA 대기 |
| T03 | 자동 카메라 | 자동 카메라 구현 후 소스 대조와 실행 가능한 독립 QA | 자동 카메라의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 대기 |
| T04 | 지정 위치 파괴 | 지정 위치 파괴 구현 후 소스 대조와 실행 가능한 독립 QA | 지정 위치 파괴의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 대기 |
| T05 | 효과와 점수 | 효과와 점수 구현 후 소스 대조와 실행 가능한 독립 QA | 효과와 점수의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 대기 |
| T06 | 자동 재건 | 자동 재건 구현 후 소스 대조와 실행 가능한 독립 QA | 자동 재건의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 대기 |
| T07 | 통합·컴파일·원본 보존 검증 | 통합·컴파일·원본 보존 검증 구현 후 소스 대조와 실행 가능한 독립 QA | 통합·컴파일·원본 보존 검증의 관찰 가능한 시제품 동작과 검사 근거 | 실행 가능한 .NET 로직 검사 또는 컴파일·직렬화 참조 검사, 실제 화면 검증과 구분 | 대기 |

## QA 반복 기록

### Q01 · final · 2026-11-06T08:42:12.2944420+00:00

독립 검사 완료와 실제 화면 미실행을 구분

- T01 r1 · pass: 독립 .NET 성장 시나리오 실행 통과 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T02 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T03 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T04 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T05 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T06 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json
- T07 r1 · not_run: 컴파일·직렬화 검사 통과. 실제 시각·조작·브라우저 실행은 사용자 화면 사용 조건으로 미실행 · 근거: QA/Prototype/logic-results-v2.txt, unity-compile.txt, editor-compile.txt, scene-verification.json

## 남은 작업과 다음 반복

실제 Play·카메라·UI·타격감·음량·웹 빌드와 브라우저 실행 미검증

이전 회차: [메뉴얼 열기](../../../projects/growingcity/reports/2026-11-06-manual-20261106084048434-v1.0.0.html)

## 작업 시간

- 제안 시간: 15~25분
- 실제로 완료된 시간: 구현 및 독립 검사 완료: 2026-11-06 17:40:37 KST, 소요 19분 57초. 실제 Play·웹 실행 완료 시각은 없으며 공개 게시 검증은 별도 진행.
- XERJ: 미사용 — XERJ 제공 연결 없음. rg와 현재 소스·에셋·실행 검사 사용
