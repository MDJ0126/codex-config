---
title: 2026-10-16 설치본 대조 점검
date: 2026-10-16T05:01:31.343Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: 2026-10-16 설치본 대조 점검 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/comparison-20261016.html
source_sha256: caaf31fd36f6efb3fa84e27461142aa964c61acd2a73e2a573bc3ef6a1eabfe0
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

CodeAssist 0.0.76 설치 바이너리 6개를 패키지와 대조했습니다. VA 10.9.2630.0의 네이티브 리소스를 REA로 다시 추출했습니다. VA와 같은 입력을 사용한 격리 실행 비교는 아직 수행하지 않았습니다.

## 실제 Visual Studio 확인

- 통과: 한국어 Welcome이 코드 편집 영역의 문서 탭으로 표시됩니다.

- 통과: 두 번째 도구 모음 버튼이 파일 찾기 창을 엽니다.

- 실패 관찰: 파일 찾기에 main을 입력했지만 목록이 비어 있습니다. 인덱스 준비 완료 표시도 확인했습니다. 직접 원인은 미확인입니다.

- 통과: Welcome 문서 버튼에서 설치본 내부 사용 안내가 열립니다.

탐색 명령은 NavigationCommands.cs:188에서 UnrealProjectPath로 초기화합니다. ProjectIndexService.Initialize는 빈 경로를 무시합니다. 범용 솔루션 루트 처리 결함 후보이며 이번 실행의 빈 목록 원인으로 확정하지 않습니다.

## REA 증거

VA_X64.dll: 리소스159개, 디코딩한 대화상자44개. VAOpsWin64.dll: 리소스181개, 대화상자26개. 이는 리소스 목록 확보이며 전체 내부 동작의 디컴파일 완료를 뜻하지 않습니다. 리소스 좌표는 런타임 기본값의 증거가 아닙니다.

## 남은 대조

VA 격리 실행, 추천 색상·심볼 정확도, 모든 옵션·기본값, 리팩터링과 Undo, CPU·응답 시간, UnrealTestProject 실행 비교는 미검증입니다.

**원본 유사도: 미측정. 부분 구현 대응:108/108개. 완전 구현 검증:0/108개. VA 완전 동등성 검증:0/108개(0%). 문서 기반 부분 명세 확보:108/108개(100%). 전체 역분석 완료율:미확인.**

실행 기록 JSON · [인계 문서](../assets/imports/docs/handoff.html)

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/comparison-20261016.html)

- 원문 SHA-256: `caaf31fd36f6efb3fa84e27461142aa964c61acd2a73e2a573bc3ef6a1eabfe0`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
