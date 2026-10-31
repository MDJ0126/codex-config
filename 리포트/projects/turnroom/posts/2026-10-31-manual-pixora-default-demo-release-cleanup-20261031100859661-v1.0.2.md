---
title: Pixora 기본 데모·폴더 및 배포 제외 수정
date: 2026-10-31T10:08:59.661Z
version: v1.0.2
status: 완료
state: complete
project: turnroom
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 고객용 샘플과 개발 검증 자료를 분리하고 실제 배포 파일을 다시 검증합니다.
format: tracefolio-v2
priority: normal
manual_id: pixora-default-demo-release-cleanup
round: 3
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-pixora-default-demo-release-cleanup-20261031100859661-v1.0.2.json
---

## 목표와 완료 기준

Pixora 기본 데모와 Samples 루트를 최신 소재로 정리하고 개발 지침을 배포에서 제외한다.

완료 기준: 기본 데모·저장 작업 예제·10개 씬 최신 연결, 개발 자료 제외 패키지 및 깨끗한 설치·실행 검증과 HTML 보고

후속 3회차 · 순차 진행 · 전체 4개 · 이번 실행 T04

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 기본 데모와 개발 자료 분리 | 최신 이미지·모델·클립과 저장 작업 예제를 연결하고 기존 로봇 검증 자료의 GUID를 보존해 개발 폴더로 옮긴다. | Samples 기본 위치에 예전 데모가 없고 모든 기본 로딩이 최신 소재 | 실제 Studio 로드·작업 예제 재열기·의존성 검사 | 완료 |
| T02 | 배포 재생성·설치 검증 | 파일 단위 배포 제외 필터를 적용해 패키지를 다시 만들고 깨끗한 프로젝트에서 설치·실행한다. | 개발 AGENTS·Development·옛 데모 제외, 최신 씬·원본 포함 | 실제 unitypackage pathname·해시 대조 및 Unity 재설치 실행 | 완료 |
| T03 | 기록과 푸시 | 변경·실패·실제 화면과 작업 시간을 HTML 아카이브에 기록한다. | 푸시·보고 공개·링크 검증 | 명시적 스테이징·HTML 빌드·외부 응답 확인 | 완료 |
| T04 | 추가 아이콘 작업 폴더 보존·대조 | 지정된 바탕화면 폴더의 파일을 프로젝트 안에 보존하고 전체 SHA-256을 대조한다. | 원본 3개 보존 및 해시 일치 후 삭제 가능 여부 판정 | 모든 파일 크기 및 SHA-256 일치, 현재 제품 파일과 차이 확인 | 완료 |

## QA 반복 기록

### Q01 · final · 2026-10-31T18:01:43.300612+09:00

독립 Unity 실행·최종 패키지 감사·프로젝트 푸시·HTML 외부 배포 근거를 최종 대조했습니다. 기록 이전 실패와 남은 심사 항목은 결과 보고서에 보존합니다.

- T01 r1 · pass: 최신 기본 데모·Works 2개와 10개 씬 연결을 독립 Unity에서 검증했습니다. · 근거: 실제 Studio 캡처 및 fresh-install.json
- T02 r1 · pass: Pixora 단독 새 설치와 Samples 제거 본체 실행 통과. 최종 tar에서 개발 AGENTS·Development·옛 로봇·긴 경로 없음. · 근거: package-audit.json 및 core-without-samples.json
- T03 r1 · pass: 21dc3048 프로젝트 푸시, 8b4ca04 전역 지침 및 dd57492 보고서 푸시. Actions 41186855034 성공, 실제 HTML과 WebP HTTP 200. · 근거: 520개 보고서와 19467 링크 검증, GitHub Actions 성공 및 외부 URL 응답
### Q02 · step · 2026-10-31T19:08:56.585548+09:00

추가 지정한 아이콘 파일 3개 보존과 대조 결과

- T04 r1 · pass: PNG 2개와 SVG 1개 보존, 모두 동일 해시. 바탕화면 삭제 가능, 실제 삭제는 미수행. · 근거: ArtSources/Templete/DesktopIconArchive-20261031/verification.json의 파일 3개 해시 일치
### Q03 · final · 2026-10-31T19:08:56.585548+09:00

기존 완료 근거를 유지하고 추가 파일 보존 결과를 최종 대조했습니다.

- T01 r1 · pass: 최신 기본 데모·Works 2개와 10개 씬 연결을 독립 Unity에서 검증했습니다. · 근거: 실제 Studio 캡처 및 fresh-install.json
- T02 r1 · pass: Pixora 단독 새 설치와 Samples 제거 본체 실행 통과. 최종 tar에서 개발 AGENTS·Development·옛 로봇·긴 경로 없음. · 근거: package-audit.json 및 core-without-samples.json
- T03 r1 · pass: 21dc3048 프로젝트 푸시, 8b4ca04 전역 지침 및 dd57492 보고서 푸시. Actions 41186855034 성공, 실제 HTML과 WebP HTTP 200. · 근거: 520개 보고서와 19467 링크 검증, GitHub Actions 성공 및 외부 URL 응답
- T04 r1 · pass: PNG 2개와 SVG 1개 보존, 모두 동일 해시. 바탕화면 삭제 가능, 실제 삭제는 미수행. · 근거: ArtSources/Templete/DesktopIconArchive-20261031/verification.json의 파일 3개 해시 일치

## 남은 작업과 다음 반복

파일 보존 및 대조 완료. 바탕화면 삭제는 수행하지 않음.

이전 회차: [메뉴얼 열기](../../../projects/turnroom/reports/2026-10-31-manual-pixora-default-demo-release-cleanup-20261031090143618-v1.0.1.html)

## 작업 시간

- 제안 시간: 이번 후속 작업 1~3분; 최초 작업 시간은 이전 버전에 보존.
- 실제로 완료된 시간: 2026-10-31 19:07:19 KST 시작. 파일 보존·해시 검증 완료 19:07:39 KST(20초). 보고서 기록 시각 19:08:56 KST. 공개 배포 확인은 별도 진행.
- XERJ: 미사용 — 코드 검색 불필요 — 파일 목록·바이트·SHA-256 대조 작업입니다.
