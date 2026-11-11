---
title: Pixora Studio와 플레이어·몬스터 전용 픽셀 카메라 적용
date: 2026-11-11T15:36:51.055Z
version: v1.0.1
status: 완료
state: complete
project: fantasya
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 제품은 Pixora Studio만 가져오며 배경을 제외한 캐릭터만 실제 렌더로 비교합니다.
format: tracefolio-v2
priority: normal
manual_id: fantasya-pixora-character-pixels-20261111
round: 2
manual_state: complete
payload: ../assets/submissions/2026-11-11-manual-fantasya-pixora-character-pixels-20261111-20261111153651055-v1.0.1.json
---

## 목표와 완료 기준

Pixora Studio를 도입하고 플레이어·몬스터만 픽셀화한다.

완료 기준: 원본 DDT·다른 제품을 보존하고 캐릭터 한정 실제 렌더·배경 보존·정적 설정 저장·문서 근거를 확인한다.

후속 2회차 · 순차 진행 · 전체 3개 · 이번 실행 T01, T02, T03

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | Pixora 제품과 의존성 격리 도입 | DDT 데이터는 보존하고 Pixora Studio와 필수 Templete만 사본에 도입한다. | 기존 DDT와 신규 제품 어셈블리 컴파일 | 도입 전 파일 목록과 코드·버전·Unity 로그를 비교한다. | 완료 |
| T02 | 캐릭터 전용 픽셀 렌더 구성 | 카메라 전용 Forward 렌더러와 4픽셀 캐릭터 표식을 저장한다. | 플레이어·몬스터만 픽셀화, 배경 원본 유지 | 같은 프레임 OFF/ON 이미지와 배경 픽셀 차이, 대상 목록·상태 복구를 비교한다. | 완료 |
| T03 | 원본 반영과 사용·검증 문서 | 검증한 제품과 게임 에셋을 원본에 반영하고 문서와 공개 근거를 정리한다. | 데이터 보존·씬 재열기·문서 링크·공개 결과 확인 | 원본 사본 해시와 실제 실행, 데이터·원본 보존 근거를 확인한다. | 완료 |

## QA 반복 기록

### Q01 · step · 2026-11-11T22:29:58+09:00

최초 렌더에서 배경 차이 발견

- T01 r1 · pass: 제품·DDT 컴파일 및 일반 몬스터 코드 복원 · 근거: Unity import 및 pixora-character-render 로그
- T02 r1 · fail: 배경 563픽셀 차이, 원인 조사 · 근거: 최초 OFF/ON 비교
### Q02 · step · 2026-11-11T22:42:17+09:00

워밍업 후 실제 동일 프레임 비교 통과

- T02 r1 · pass: 캐릭터3개, 비캐릭터0개, 배경815995픽셀 차이0, OFF반복 차이0 · 근거: ../assets/pixora-20261111/render.json 및 before.png/after.png
### Q03 · final · 2026-11-11T15:36:49.381Z

원본 적용 근거와 승인된 공개 배포의 최종 대조

- T01 r1 · pass: 제품·DDT Unity 컴파일 확인 · 근거: 최초 버전의 Unity 실행 로그·카메라 PNG·DDT 해시 확인 결과 유지, 이번 게시 작업에서 게임 코드 재변경 없음
- T02 r1 · pass: 캐릭터 3개만 픽셀화, 배경 815995픽셀 차이 0 · 근거: 최초 버전의 Unity 실행 로그·카메라 PNG·DDT 해시 확인 결과 유지, 이번 게시 작업에서 게임 코드 재변경 없음
- T03 r1 · pass: 원본 적용·데이터 보존·문서 갱신과 공개 본문·이미지·첨부·Mermaid 3개 확인 완료 · 근거: https://mdj0126.github.io/codex-config/projects/fantasya/reports/2026-11-11-result-20261111134632367-v1.0.0.html 및 https://mdj0126.github.io/codex-config/projects/fantasya/reports/2026-11-11-result-20261111134634009-v1.0.0.html; GitHub Actions 43049441510 성공, 공개 HTTP·본문·이미지와 Mermaid 3개 확인

## 남은 작업과 다음 반복

승인된 공개 게시·main 푸시·문서 배포와 외부 확인 완료. 제품의 인스턴싱 경고, 다른 플랫폼·자연 전투·Image Maker 생성은 기존 미확인 범위입니다. 목록 카드 썸네일은 아카이브 기능 제한으로 미확인입니다. 새 버전은 이 완료 기록의 공개 배포만 남았습니다.

이전 회차: [메뉴얼 열기](../../../projects/fantasya/reports/2026-11-11-manual-20261111134807418-v1.0.0.html)

## 작업 시간

- 제안 시간: 2026-11-11 22:11 KST 시작, 30~50분
- 실제로 완료된 시간: 게임 적용: 39분 54초, 2026-11-11 22:51:47 KST 완료. 승인 후 공개 게시·첫 외부 확인: 5분 32초, 2026-11-12 00:36:49 KST. 최종 기록 배포 확인은 이어서 진행합니다.
- XERJ: 미사용 — 사용자 지침에 따라 rg·정확한 로컬 제품/URP 소스·실제 Unity 로그 사용
