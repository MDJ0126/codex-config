---
title: 카메라 렌더링 책임 분리 작업 메뉴얼
date: 2026-11-09T12:50:09.623Z
version: v1.0.3
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
round: 4
manual_state: qa
payload: ../assets/submissions/2026-11-09-manual-pixel-camera-isometric-separation-20261109-20261109125009623-v1.0.3.json
---

## 목표와 완료 기준

Pixel Camera는 지정한 Pixel Camera Object만 렌더링하고 Isometric Camera는 독립적으로 방향을 제어합니다.

완료 기준: Unity에서 활성화 중 Transform 잠금과 해제, 자유 시점에서만 회전 입력 가능, 시장 픽셀 표현을 확인하고 화면을 캡처합니다.

후속 4회차 · 순차 진행 · 전체 1개 · 이번 실행 T01

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 렌더 경로, 예제 및 사용 안내 분리 | 시점 이름 변경, Transform 잠금과 해제, 자유 시점 전용 각도 입력, 기존 시장 예제와 문서 연결 확인 및 설명을 설정 아래로 이동 | 고정 방향의 회전 입력이 비활성화되고 자유 시점에서만 월드 Y축 회전 값을 편집할 수 있습니다. 설정 아래에 설명이 표시됩니다. | 소스 및 UI 연결 대조 후 Unity에서 기본 방향, 자유 시점, 컴포넌트 활성화와 해제를 확인합니다. 설정 카드와 안내 영역의 실제 순서와 가독성을 확인합니다. | QA 대기 |

## QA 반복 기록

아직 실행한 QA가 없습니다. 준비된 절차를 통과 결과로 표시하지 않습니다.

## 남은 작업과 다음 반복

Unity Inspector 실제 표시 및 수정 후 화면 캡처는 미확인입니다. 사용자가 제공한 이미지는 수정 전 참고 화면입니다. 전역 지침에 추가할 검증된 공통 교훈은 없었습니다.

이전 회차: [메뉴얼 열기](../../../projects/pixora/reports/2026-11-09-manual-pixel-camera-isometric-separation-20261109-20261109124616733-v1.0.2.html)

## 작업 시간

- 제안 시간: 수정 및 코드 확인 2~4분
- 실제로 완료된 시간: 21:48:48 KST 작업 시작. 21:49:26 KST까지 코드 수정 및 정적 확인 (38초). 실제 Unity 화면 확인은 미실행.
- XERJ: 미사용 — 코드 검색 불필요
