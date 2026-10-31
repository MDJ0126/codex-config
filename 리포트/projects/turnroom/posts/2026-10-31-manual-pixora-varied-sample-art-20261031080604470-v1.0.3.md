---
title: Pixora 예제 소재 다양화 — Affinity·Blender
date: 2026-10-31T08:06:04.470Z
version: v1.0.3
status: 완료
state: complete
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 다양한 원본과 10개 실제 씬을 제작하고 공용 폰트·깔끔한 샘플 UI를 추가합니다.
format: tracefolio-v2
priority: normal
manual_id: pixora-varied-sample-art
round: 4
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-pixora-varied-sample-art-20261031080604470-v1.0.3.json
---

## 목표와 완료 기준

편집 가능한 원본과 서로 다른 예제 결과물을 제공한다.

완료 기준: Affinity 원본·PNG, Blender 원본·FBX, 10개 단계별 씬 입력/출력 연결, 실행·픽셀/애니메이션 QA와 HTML 보고

후속 4회차 · 순차 진행 · 전체 6개 · 이번 실행 T05

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 소재와 기존 씬 확인 | 10개 씬의 입력과 생성 경로를 확인한다. | 역할별 소재 배정 | 현재 소스·에셋 속성 대조 | 완료 |
| T02 | Affinity 이미지 제작 | 벡터 원본을 Affinity에서 저장하고 PNG를 내보낸다. | 6종 이미지·4프레임 시트와 편집 가능한 원본 | 다시 열기·내보내기 픽셀 비교와 시각 확인 | 완료 |
| T03 | Blender 모델 제작 | 4종 모델과 움직임을 제작하고 FBX로 내보낸다. | 편집 가능한 blend와 서로 다른 모델 | Blender 실제 저장·렌더·FBX Unity 재임포트 | 완료 |
| T06 | 공용 폰트·샘플 UI | 재배포 가능한 Noto Sans KR 기반 정적 Regular·SemiBold를 Templete에 포함하고 Pixora·DataTable에서 연결한다. | 동일 Font 자산·적절한 제목/본문 대비·중립 색상·기존 표 표시 유지 | 실제 Unity 폰트 로드·글리프·두 제품 표시·라이선스·버전 요구·9개 언어 화면 | 완료 |
| T04 | 예제 통합과 실행 QA | 소재별 씬·코드·폰트·레이아웃을 통합하고 모든 씬을 실제 Play 모드와 마우스로 검증한다. | 10개 씬·5단계·움직임·그리드·폰트와 9개 언어 정상 | 실제 카메라·각 모델 상하 기준점·픽셀·부위·50번 네이티브 단계 클릭·9개 언어·DataTable 회귀 | 완료 |
| T05 | 결과 보고 | 근거·원본 사용법과 HTML 결과를 게시한다. | 실제 씬 화면·제한·작업 시간 포함 | 보고서 빌드·링크·게시 확인 | 완료 |

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
### Q05 · step · 2026-10-31T16:54:18.419956+09:00

공용 Font 두 굵기·대표 9언어 글리프·DataTable 실제 UI 스타일 일치·18개 실제 화면 확인

- T06 r1 · pass: 공용 Font 두 굵기·대표 9언어 글리프·DataTable 실제 UI 스타일 일치·18개 실제 화면 확인 · 근거: final-shared-fonts-ddt-03.json; font-ddt-final-probe-04.json; 두 제품 9개 언어 캡처; OFL·Source.json
### Q06 · step · 2026-10-31T16:54:18.419956+09:00

최종 소재 Unity396개 검사·실제 50번 단계 클릭·9개 가이드126장90코드 예제 통과

- T04 r2 · pass: 최종 소재 Unity396개 검사·실제 50번 단계 클릭·9개 가이드126장90코드 예제 통과 · 근거: SampleArtworkQA/result.json 16:36:53; native-scenes.json 16:46:14; VerifyLocalizedGuides PASS; Affinity 최종7개 픽셀 일치
### Q07 · step · 2026-10-31T17:06:04.042838+09:00

보고서·푸시·공개 결과 검증 완료

- T05 r2 · pass: 프로젝트 d31b5a09와 메타데이터773768a6 푸시. 최종 결과 HTML200, Actions41182980977 success. 509개 HTML/Markdown·18488개 링크·필터/검색 및41개 자동 테스트 확인. · 근거: 외부 result-20261031080201009 HTML200; Actions41182980977; verify.mjs; report-tests.log
### Q08 · final · 2026-10-31T17:06:04.042838+09:00

전체 현재 작업과 선행 버전 최종 QA 통과

- T01 r1 · pass: 10개 씬의 공통 로봇 재사용과 입력·생성 경로 확인 · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트
- T02 r1 · pass: 6종 이미지와 4프레임 시트의 Affinity .af 원본·PNG, 7개 재열기 출력 픽셀 일치 · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트
- T03 r1 · pass: 4종 .blend·FBX 실제 저장·렌더·재열기, 새·드론 애니메이션 키 확인 · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트
- T06 r1 · pass: 공용 Font 두 굵기·대표 9언어 글리프·DataTable 실제 UI 스타일 일치·18개 실제 화면 확인 · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트
- T04 r2 · pass: 최종 소재 Unity396개 검사·실제 50번 단계 클릭·9개 가이드126장90코드 예제 통과 · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트
- T05 r2 · pass: 프로젝트 d31b5a09와 메타데이터773768a6 푸시. 최종 결과 HTML200, Actions41182980977 success. 509개 HTML/Markdown·18488개 링크·필터/검색 및41개 자동 테스트 확인. · 근거: Affinity 재열기 PNG 픽셀 일치; Blender 재열기; Unity396검사; native50클릭;9개언어18화면; 결과 HTML200; Git푸시;509개문서/18488링크;41개테스트

## 남은 작업과 다음 반복

없음. 판매용 unitypackage 및 스토어 게시는 요청 범위에 포함하지 않았습니다.

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-31-manual-pixora-varied-sample-art-20261031075447433-v1.0.2.html)

## 작업 시간

- 제안 시간: 40~70분
- 실제로 완료된 시간: 기록 시작2026-10-31 15:48:59 KST. 결과 보고 공개 확인2026-10-31 17:04:40 KST, 75분41초. 완료 메뉴얼 기록2026-10-31 17:06:04 KST; 마지막 배포 확인은 채팅에서 별도 기록.
- XERJ: 사용 — TutorialAssets 검색 후 현재 소스 대조
