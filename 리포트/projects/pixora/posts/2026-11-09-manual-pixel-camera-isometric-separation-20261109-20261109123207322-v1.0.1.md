---
title: 카메라 렌더링 책임 분리 작업 메뉴얼
date: 2026-11-09T12:32:07.322Z
version: v1.0.1
status: QA 중
state: verification
project: pixora
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 오브젝트 픽셀 렌더링과 독립 아이소매트릭 시점을 분리합니다.
format: tracefolio-v2
priority: normal
manual_id: pixel-camera-isometric-separation-20261109
round: 2
manual_state: qa
payload: ../assets/submissions/2026-11-09-manual-pixel-camera-isometric-separation-20261109-20261109123207322-v1.0.1.json
---

## 목표와 완료 기준

Pixel Camera는 지정한 Pixel Camera Object만 렌더링하고 Isometric Camera는 독립적으로 방향을 제어합니다.

완료 기준: Unity에서 추적 유무에 따른 이동과 월드 Y축 공전, 시장 픽셀 렌더링을 검증하고 화면을 캡처합니다.

후속 2회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 렌더 경로, 예제 및 사용 안내 분리 | 카메라 Transform 이동을 주시점에 반영하고 시장 프리팹에 지정 오브젝트 픽셀 경로를 연결합니다. | 추적 대상 없이 이동할 수 있고, 시장 예제의 지정 메시가 픽셀화됩니다. | 씬 연결 대조 후 Unity에서 이동·공전과 실제 픽셀화 및 윤곽선을 확인합니다. | QA 대기 |

## QA 반복 기록

아직 실행한 QA가 없습니다. 준비된 절차를 통과 결과로 표시하지 않습니다.

## 남은 작업과 다음 반복

Unity에서 이동·회전 조작과 픽셀 효과의 실제 표시 및 전후 스크린샷은 미확인입니다. 전역 지침에 추가할 검증된 공통 교훈은 없었습니다.

이전 회차: [메뉴얼 열기](../../../projects/pixora/reports/2026-11-09-manual-20261109121020766-v1.0.0.html)

## 작업 시간

- 제안 시간: 추가 범위를 포함해 3~5분
- 실제로 완료된 시간: 21:28:13 KST 시작. 21:30:59 KST까지 코드·예제·문서 정적 확인 (2분 46초). Unity 실행 검증 미완료.
- XERJ: 미사용 — 코드 검색 불필요
