---
title: 카메라 렌더링 책임 분리 작업 메뉴얼
date: 2026-11-09T12:46:16.733Z
version: v1.0.2
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
round: 3
manual_state: qa
payload: ../assets/submissions/2026-11-09-manual-pixel-camera-isometric-separation-20261109-20261109124616733-v1.0.2.json
---

## 목표와 완료 기준

Pixel Camera는 지정한 Pixel Camera Object만 렌더링하고 Isometric Camera는 독립적으로 방향을 제어합니다.

완료 기준: Unity에서 활성화 중 Transform 잠금과 해제, 자유 시점에서만 회전 입력 가능, 시장 픽셀 표현을 확인하고 화면을 캡처합니다.

후속 3회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 렌더 경로, 예제 및 사용 안내 분리 | 시점 이름 변경, Transform 잠금과 해제, 자유 시점 전용 각도 입력, 기존 시장 예제와 문서 연결 확인 | 고정 방향의 회전 입력이 비활성화되고 자유 시점에서만 월드 Y축 회전 값을 편집할 수 있습니다. | 소스 및 UI 연결 대조 후 Unity에서 기본 방향, 자유 시점, 컴포넌트 활성화와 해제를 확인합니다. | QA 대기 |

## QA 반복 기록

아직 실행한 QA가 없습니다. 준비된 절차를 통과 결과로 표시하지 않습니다.

## 남은 작업과 다음 반복

Unity Inspector에서 실제 잠금, 방향 선택과 입력 비활성화, 비활성화 후 잠금 해제 및 화면 캡처는 미확인입니다. 전역 지침에 추가할 검증된 공통 교훈은 없었습니다.

이전 회차: [메뉴얼 열기](../../../projects/pixora/reports/2026-11-09-manual-pixel-camera-isometric-separation-20261109-20261109123207322-v1.0.1.html)

## 작업 시간

- 제안 시간: 코드 및 문서 확인 3~5분
- 실제로 완료된 시간: 21:42:56 KST 시작. 21:45:27 KST에 소스·9개 언어 가이드 정적 확인 (2분 31초). Unity 실제 실행 검증 미완료.
- XERJ: 미사용 — 코드 검색 불필요
