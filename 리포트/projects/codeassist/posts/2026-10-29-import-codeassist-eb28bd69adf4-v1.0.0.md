---
title: VA·CA: 위치·개수·누락 기준 재평가
date: 2026-10-24T06:36:12.300Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: VA·CA: 위치·개수·누락 기준 재평가 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/structural-173/index.html
source_sha256: 56b68d067438d07fae5cfcc2762c7f19572bda0a32c4e1d8eb379f1c495681aa
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

VA·CA 구성 유사도 재평가 

## VA·CA: 위치·개수·누락 기준 재평가

**확인 가능한 구성 체크 유사도 86.98% · 83.5/96점**
비슷함 78개 · 부분 대응 11개 · 누락 7개. 개별 체크의 개수이며 독립 기능 96개라는 뜻은 아닙니다.

픽셀, 정확한 크기·색상, 브랜드명, 번역 길이 차이는 감점하지 않았습니다. 같은 위치·순서에 대응 기능과 컨트롤이 있으면 비슷함 1점, 관련 기능은 있지만 본문 그룹이 부족하면 0.5점, 대응 컨트롤·페이지가 없으면 0점입니다. 안내문만 있는 설정 본문은 누락으로 처리했습니다.

| 구성 | 점수 | 해석 |
| --- | --- | --- |
| 옵션 탐색 항목 | 90% · 27/30 | 독립 페이지 System Info·Startup·Logging and Recovery 없음. Startup 일부는 Advanced 본문에 있음. CA 추가 5페이지는 감점하지 않음. |
| 기준 도구 모음 | 100% · 11/11 | 기능별 버튼과 순서 대응. CA Welcome 추가 버튼은 별도. |
| Editor·Coloring 기준 컨트롤 | 100% · 32/32 | 17 체크박스 + 9 체크박스 + 6 색상 선택. 존재 기준. |
| 공통 상대 배치 | 100% · 6/6 | 좌측 검색·트리, 우측 본문, 상단 제목, 우측 하단 저장·취소, 본문 스크롤. |
| 기존 촬영 옵션 본문 17종 | 44.12% · 7.5/17점 | 비슷함 2 · 부분 대응 11 · 설정 본문 누락 4. 부분 대응은 실패로 버리지 않고 반점 인정. |

**이 점수는 전체 제품의 실행 동기화율이 아닙니다.** 기존 촬영이 있는 17종 옵션 본문과 상위 구성 항목의 재평가입니다. 다른 UI·실제 동작을 100% 확인한 것으로 확대하지 않았습니다. 108개 카탈로그의 부분 대응 기록도 이 점수 분자에 넣지 않았습니다. 이전 낮은 실행 검증 통과율을 UI 유사도처럼 제시하면 안 됩니다.

## 분명히 남은 차이

- 설정 본문 4종: Hashtags, Projects and Files, C/C++ Directories, Code Generation.

- 고급 설정 독립 페이지 3종: System Info, Startup, Logging and Recovery.

- 부분 대응 11종의 하위 그룹은 아래 표에 기록했습니다.

- 점수 밖의 확인된 미구현: Floating Elements 부동 툴바, 로컬 AI·에이전트, GPU 전용 분석기. 추천 목록은 3개 필터 버튼만 있어 추가 종류·접근성 필터 차이가 남습니다.

## 기존 실제 화면: Coloring

6색 세로 목록, 적용 대상 2행 3열, 아래 글꼴 속성, 오른쪽 아래 버튼이 대응하므로 비슷함으로 인정합니다.

VA 기존 촬영

![참고 화면](../assets/imports/docs/validation/options-160/va-coloring.jpg)

CA 기존 촬영

![참고 화면](../assets/imports/docs/validation/options-160/ca-coloring-final.jpg)

## 전체 체크 근거

| 분류 | 항목 | 판정 | 근거·차이 | 원자료 |
| --- | --- | --- | --- | --- |
| 옵션 탐색 항목 | Advanced | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | System Info | 누락 | 동일한 독립 페이지 없음. Startup 일부 설정은 Advanced 본문에 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Startup | 누락 | 동일한 독립 페이지 없음. Startup 일부 설정은 Advanced 본문에 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Logging and Recovery | 누락 | 동일한 독립 페이지 없음. Startup 일부 설정은 Advanced 본문에 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Performance | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Games & GPUs | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Debug Assistance | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | VA Intelligence | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Code Inspection | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Checker Sets | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Per-File | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Project-Wide | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Code Generation | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Projects | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | C/C++ Directories | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Projects and Files | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Suggestions | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Enhanced Listboxes | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Navigation | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | VA Hashtags | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Source Links | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Goto | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Editor | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Mouse | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Display | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Smart Select | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Floating Elements | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Highlighting | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Underlining | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 옵션 탐색 항목 | Coloring | 비슷함 | 브랜드명 차이는 허용. 같은 상위 범주에 대응 항목 존재 | VA.results.json treeHeaders / ProductOptions.cs Group·Page |
| 도구 모음 | Options | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Open File in Solution | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Find Symbol | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Find References | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Find Previous by Context | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Find Next by Context | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Insert Snippet | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Spell Check | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Open Corresponding File | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Skip All Breakpoints | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 도구 모음 | Bind Breakpoints to Current Thread | 비슷함 | 동일 기능 순서. CA Welcome 추가 버튼은 누락으로 계산하지 않음 | 사용자 VA 도구 모음 참조 / CodeAssistCommands.vsct:132–142 |
| 기준 컨트롤 개수 | Editor 체크박스 1 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 2 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 3 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 4 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 5 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 6 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 7 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 8 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 9 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 10 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 11 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 12 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 13 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 14 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 15 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 16 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Editor 체크박스 17 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 1 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 2 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 3 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 4 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 5 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 6 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 7 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 8 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 체크박스 9 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 1 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 2 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 3 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 4 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 5 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 기준 컨트롤 개수 | Coloring 색상 선택 6 | 비슷함 | 기준 컨트롤 존재 여부. 동작·설정값 동일 여부는 채점하지 않음 | options-126/summary.json ui + ProductOptions.cs:315–373 + options-160 대응 화면 |
| 공통 배치 | 검색은 좌측 상단 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 공통 배치 | 탐색 트리는 좌측 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 공통 배치 | 선택 페이지 본문은 우측 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 공통 배치 | 페이지 제목은 본문 상단 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 공통 배치 | 저장·취소는 우측 하단 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 공통 배치 | 본문 세로 스크롤 | 비슷함 | 픽셀·폭·브랜드·문자열 길이 차이는 감점하지 않음 | options-160 VA·CA 화면 / ProductOptions.cs:186–271 |
| 옵션 본문 17종 | Goto | 부분 대응 | 이동 범위·기본 이동 우선순위 그룹 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Highlighting | 부분 대응 | 강조 토글은 있으나 색상·세부 표시 그룹 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Display | 부분 대응 | 탐색바·정렬은 있으나 상하 위치·Quick Info 세부 구성 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | VA Hashtags | 누락 | 본문이 사용 안내문. 최소 길이·세부 설정 컨트롤 없음 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Editor | 비슷함 | 기준 17 체크박스와 하위 들여쓰기·괄호 라디오·Surround 그룹 존재 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Enhanced Listboxes | 부분 대응 | 8개 토글은 있으나 원본 멤버·공급자·표시 옵션 구성 일부 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Coloring | 비슷함 | 6색 세로 목록·적용 대상 2행 3열·글꼴 속성 그룹 대응 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Underlining | 부분 대응 | 진단 켜기만 있고 심볼·밑줄 색상 그룹 부족 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Project-Wide | 부분 대응 | 검사 모드 선택은 있으나 프로젝트 수명·체커 조합 구성 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Per-File | 부분 대응 | 파일 검사 토글은 있으나 체커·동작 시점 구성 부족 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Code Generation | 누락 | 명령 안내문. 원본 생성 규칙 설정 본문 없음 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Debug Assistance | 부분 대응 | 스텝 필터 토글·패턴은 있으나 원본 선택 방식 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Source Links | 부분 대응 | 규칙 목록·편집 필드는 있으나 원본 활성화·그룹 구성 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Projects and Files | 누락 | 인덱싱 안내문. 파일·프로젝트 처리 설정 본문 없음 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Mouse | 부분 대응 | Alt 클릭만 있으며 원본 휠·클릭 조합 설정 부족 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | C/C++ Directories | 누락 | 프로젝트 경로 사용 안내문. 플랫폼·경로 목록 편집 없음 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |
| 옵션 본문 17종 | Performance | 부분 대응 | 파서 스레드·지연값은 있으나 원본 파싱 수명·우선순위 그룹 차이 | layout-audit-0.0.92 기존 VA 화면 + 현재 ProductOptions.cs |

계산 원자료 JSON · 17종 기존 화면 목록 · 기준 컨트롤 개수

소스 0.0.173 / VA2630 기존 기록. Computer Use·키 입력·VS 재시작 없이 파일과 저장된 이미지만 검토했습니다. XERJ: 사용 — ProductOptions Pages 검색 후 현재 소스 확인.

제안 시간: 10분. 기록 시각: 2026-10-24 15:35:23 +09:00

실제로 완료된 시간: 408.2초 · 2026-10-24 15:36:12 +09:00 종료. 계산 합계·96항목·링크 파일 존재 확인 완료.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/structural-173/index.html)

- 원문 SHA-256: `56b68d067438d07fae5cfcc2762c7f19572bda0a32c4e1d8eb379f1c495681aa`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
