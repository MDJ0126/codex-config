---
title: Pixora 예제 소재 다양화 — Affinity·Blender
date: 2026-10-31T07:27:28.601Z
version: v1.0.1
status: 작업 중
state: progress
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 다양한 원본과 10개 실제 씬을 제작하고 공용 폰트·깔끔한 샘플 UI를 추가합니다.
format: tracefolio-v2
priority: normal
manual_id: pixora-varied-sample-art
round: 2
manual_state: active
payload: ../assets/submissions/2026-10-31-manual-pixora-varied-sample-art-20261031072728601-v1.0.1.json
---

## 목표와 완료 기준

편집 가능한 원본과 서로 다른 예제 결과물을 제공한다.

완료 기준: Affinity 원본·PNG, Blender 원본·FBX, 10개 단계별 씬 입력/출력 연결, 실행·픽셀/애니메이션 QA와 HTML 보고

후속 2회차 · 순차 진행 · 전체 6개 · 이번 실행 T01, T02, T03, T06, T04

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 소재와 기존 씬 확인 | 10개 씬의 입력과 생성 경로를 확인한다. | 역할별 소재 배정 | 현재 소스·에셋 속성 대조 | 완료 |
| T02 | Affinity 이미지 제작 | 벡터 원본을 Affinity에서 저장하고 PNG를 내보낸다. | 6종 이미지·4프레임 시트와 편집 가능한 원본 | 다시 열기·내보내기 픽셀 비교와 시각 확인 | 완료 |
| T03 | Blender 모델 제작 | 4종 모델과 움직임을 제작하고 FBX로 내보낸다. | 편집 가능한 blend와 서로 다른 모델 | Blender 실제 저장·렌더·FBX Unity 재임포트 | 완료 |
| T06 | 공용 폰트·샘플 UI | 재배포 가능한 Noto Sans KR 기반 정적 Regular·SemiBold를 Templete에 포함하고 Pixora·DataTable에서 연결한다. | 동일 Font 자산·적절한 제목/본문 대비·중립 색상·기존 표 표시 유지 | 실제 Unity 폰트 로드·글리프·두 제품 표시·라이선스·버전 요구·9개 언어 화면 | 진행 |
| T04 | 예제 통합과 실행 QA | 소재별 씬·코드·폰트·레이아웃을 통합하고 모든 씬을 실제 Play 모드와 마우스로 검증한다. | 10개 씬·5단계·움직임·그리드·폰트와 9개 언어 정상 | 실제 카메라·각 모델 상하 기준점·픽셀·부위·50번 네이티브 단계 클릭·9개 언어·DataTable 회귀 | 대기 |
| T05 | 결과 보고 | 근거·원본 사용법과 HTML 결과를 게시한다. | 실제 씬 화면·제한·작업 시간 포함 | 보고서 빌드·링크·게시 확인 | 대기 |

## QA 반복 기록

### Q01 · step · 2026-10-31T16:26:59.956343+09:00

10개 씬의 반복 입력 소스 확인

- T01 r1 · pass: 10개 씬의 반복 입력 소스 확인 · 근거: TutorialBuilder.cs, TutorialAssets.cs, XERJ TutorialAssets 검색
### Q02 · step · 2026-10-31T16:26:59.956343+09:00

7개 Affinity .af 재열기·PNG 픽셀 일치, 벡터 노드 확인

- T02 r1 · pass: 7개 Affinity .af 재열기·PNG 픽셀 일치, 벡터 노드 확인 · 근거: affinity-roundtrip.json, affinity-pixel-verification.json
### Q03 · step · 2026-10-31T16:26:59.956343+09:00

Blender 4개 재열기 및 키 변화 확인

- T03 r1 · pass: Blender 4개 재열기 및 키 변화 확인 · 근거: blender-reopen.json; 초기 Euler 비교 QA 스크립트 타입 오류는 수정 후 재실행
### Q04 · step · 2026-10-31T16:26:59.956343+09:00

구조 검사384개 통과했으나 실제 씬 렌더에서 FBX 축·단위 최상위 초기화로 모델 방향과 입력 표시 오류 발견

- T04 r1 · fail: 구조 검사384개 통과했으나 실제 씬 렌더에서 FBX 축·단위 최상위 초기화로 모델 방향과 입력 표시 오류 발견 · 근거: FirstPass/03-Model.png, FirstPass/result.json

## 남은 작업과 다음 반복

공용 폰트·다국어·DataTable 회귀, 문서·게시 진행 중

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-31-manual-20261031065713641-v1.0.0.html)

## 작업 시간

- 제안 시간: 40~70분
- 실제로 완료된 시간: 2026-10-31 15:48:59 KST 기록 시작; 진행 중
- XERJ: 사용 — TutorialAssets 검색 후 현재 소스 대조
