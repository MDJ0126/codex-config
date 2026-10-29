---
title: 2026-10-17 실제 실행 대조 · 사용자 요청으로 중단
date: 2026-10-17T05:15:45.125Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 버그
category: 이전 기록
summary: 2026-10-17 실제 실행 대조 · 사용자 요청으로 중단 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/runtime-stop-20261017.html
source_sha256: f656b6b60213a49ad11679647f850b88394353ac11ca9c9a5332a37ee3d78fff
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

저장: 2026-10-17 14:15:15 KST. 제품 버전 0.0.86 유지.

108개 전체 실행 미완료. 양쪽 표본 관찰 18/108 (16.67%). 이 비율은 전체 유사도나 구현 완성률이 아닙니다. 전체 유사도 미확인 · 완전 동등성 인증 0/108 · 부분 소스 대응108/108.

이번 세션의 저장된 관찰32개는 기능32개를 의미하지 않습니다. 이전 실행 결과와 함께 아래 표에 구분했습니다.

정확한 상태·재개 절차 JSON · 실제 UI 관찰32개 · 이전 관찰11개

중단 버퍼 확인: VA 테스트 파일은 디스크와 동일. CA 소유 EditorInteractionFixture.cpp 첫 글자에 미저장 j가 추가되어 있습니다. 다음 재개 시 해당 소유 버퍼만 복원하고, UI 입력 수신 창을 observer 로그로 교차 확인해야 합니다. 마지막 입력 오수신 이후 결과는 기능 판정에서 제외합니다.

## 재개 시 주의

- VA/CA 전체108개 실제 실행 비교를 이어갈 것. 이미 시험한4기능만 반복하지 말 것.

- VA 정상 PID4824 창198686, CA Exp PID13916 창198738. 재개 시 새 list_windows로 재선택.

- Computer Use 입력은 정확한 창을 활성화하고 새 스크린샷 좌표 사용. 같은 devenv의 A11y 인덱스/포커스 정보가 서로 섞이거나 stale인 사례가 있으므로 수신 프로세스 observer 로그로 교차 확인.

- CA Outline 부동 창과 Tools 메뉴/추천 팝업이 열린 상태. 사용자 중단 후 추가 UI 입력하지 않음.

- tests/EditorInteractionFixture.cpp 및 .local 소유 테스트 버퍼에 미저장 입력이 있을 수 있음. 사용자 main.cpp 변경 금지. 다음 실행 때 observer 로그와 디스크를 대조해 소유 버퍼만 정리.

- Read-only observer 테스트 VSIX는 두 프로필 설치. tools/AuditObserver/; 제품 VSIX에 포함하지 않음.

- 뷰 관찰기 .local/full-runtime/observer-4824.json 및 observer-13916.json. observedAt/파일/실제 text로 확인.

- node_repl bridge PTY45836은 재시작 후 무효일 수 있음. .local/pc-verification-client/launch.py로 공식 연결 복구.

- 기존 참조 검색 VA4 vs CA6 및 필드 캡슐화 선택 거부 차이부터 수정 후보로 유지. 현재 요청은 비교이므로 제품 수정/버전 증가/재설치 없음.

## 108개 상태

| ID | 기능 | 실행 상태 | 관찰/남은 검증 |
| --- | --- | --- | --- |
| VA-F001 | Find by Context | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F002 | Find References | different | Health 필드 참조: VA 4개/2파일, CA 6개로 동명 지역변수 포함. 전체 참조 의미 분석 동등성 실패. |
| VA-F003 | Find Symbol in Solution | sample-match | 기존 빈 심볼 목록19개 일치. 전체 경계 검증 미완료. |
| VA-F004 | GoTo Implementation | different | GetHealth 구현→선언: VA 즉시 이동, CA 후보2개 중 선택 후 동일 선언으로 이동. |
| VA-F005 | GoTo Member | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F006 | GoTo Related | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F007 | Highlight Current Line | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F008 | List Methods in File | different | 메서드5개 일치. VA 탐색바 드롭다운과 CA 선택창 UI 차이. |
| VA-F009 | Move Scope | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F010 | Navigate Back and Forward | partial | 뒤로 이동은 양쪽 CPP로 복귀. Alt+Right는 양쪽 자동완성 입력 발생: 앞으로 명령 경로 추가 검증 필요. |
| VA-F011 | Open Corresponding File | sample-match | 기존 대응 파일 툴바 CPP→FixtureActor.h 양쪽 이동. 단축키 경로 별도 미완료. |
| VA-F012 | Open File in Solution | sample-match | 기존 빈 검색3파일 및 Fixture -cpp 검색 결과 일치. |
| VA-F013 | VA Hashtags | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F014 | VA Navigation Bar | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F015 | Access to Refactoring Commands | different | Health의 Alt+Shift+Q: VA 전용 리팩터링 메뉴, CA VS 기본 Quick Actions 위임. |
| VA-F016 | Change Signature | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F017 | Convert Between Pointer and Instance | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F018 | Encapsulate Field | different | 동일 Health 단어 선택: VA 필드 캡슐화 및 CPP 참조 갱신/Undo 실행. CA는 지원하지 않는 선택 문법 오류로 거부. |
| VA-F019 | Extract Method | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F020 | Introduce Variable | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F021 | Move Implementation to Header File | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F022 | Move Implementation to Source File | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F023 | Move Method Implementations to Source File | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F024 | Move Selection to New File | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F025 | Rename | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F026 | Rename Files | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F027 | Simplify Instance Declaration | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F028 | Add Forward Declaration | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F029 | Add Member | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F030 | Add Missing Case Statements | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F031 | Add Similar Member | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F032 | Add Using Directive | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F033 | Add include Directive | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F034 | Add/Remove Braces | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F035 | Create Declaration | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F036 | Create File | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F037 | Create from Usage | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F038 | Create Implementation | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F039 | Create Method Implementations | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F040 | Document Method | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F041 | Implement Interface / Virtual Methods | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F042 | Acronyms | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F043 | Auto-Extend Multi-Line Comments | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F044 | Enhanced Listboxes | sample-match | 동일 Actor.GetHe 입력에서 Ctrl+Space로 GetHealth() 확정 및 Undo 양쪽 확인. 팝업 색상/필터 전체 검증 아님. |
| VA-F045 | Format After Paste | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F046 | Insert () and Closing Brackets and Quotes | sample-match | 물리 Shift+9로 () 생성, Backspace로 빈 쌍 삭제 양쪽 확인. 전체 괄호/따옴표/Undo 경계 미완료. |
| VA-F047 | Insert _ After m and Shift | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F048 | Multiple Clipboards | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F049 | Reset Editor Zoom | sample-match | 110%→100% 양쪽 확인. VA 공식 VAssistX.ResetEditorZoom 명령 실행, CA Ctrl+0. VA Ctrl+0 미지정은 기능 부재가 아님. |
| VA-F050 | Shorthand | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F051 | Smart Select | partial | 식별자 선택 확장 시 양쪽 둘러싼 함수로 확장. 서로 다른 소유 테스트 함수 사용, VA 축소 키 경로 미검증. |
| VA-F052 | Smart Suggestions | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F053 | Sort Selected Lines | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F054 | Suggestion Lists | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F055 | Surround Selection | sample-match | 물리 Shift+9로 식별자 감싸기 및 Ctrl+Z 복원 양쪽 확인. Unicode type_text("(") 결과는 물리 키 동등성에서 제외. |
| VA-F056 | Surround With | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F057 | Column Indicator | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F058 | Context Field | different | Health 선언의 VA Context는 ABaseActor.Health, CA ABaseActor. 지연된 UI 상태와 의미 비교 주의. |
| VA-F059 | Definition Field | different | VA Definition은 초기값 포함 float Health = 100.0f, CA float Health 표시 관찰. |
| VA-F060 | Enhanced Syntax Coloring | partial | 동일 헤더에서 양쪽 심볼 색상 화면 관찰. 전체 토큰/테마/기본값 동등성 미검증. |
| VA-F061 | Local Symbols in Bold | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F062 | Highlight Find Results | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F063 | Highlight Matching Words | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F064 | Highlight References to Symbol Under Cursor | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F065 | Parameter Info | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F066 | Print in Color | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F067 | Quick Info | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F068 | System Symbols in Italics | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F069 | Convert Dot to -> | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F070 | Highlight Files in Output Window | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F071 | Highlight Matching Braces and Parentheses | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F072 | Highlight Mismatching Braces and Parentheses | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F073 | Repair Case | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F074 | Underline Spelling Errors | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F075 | Underline Unknown Symbols | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F076 | Code Inspection Settings | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F077 | Code Issues in the Editor | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F078 | Quick Fixes in the Editor | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F079 | Quick Fixes in a Tool Window | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F080 | VA Snippets | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F081 | for Refactoring | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F082 | for Smart Suggestions | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F083 | for Surround With | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F084 | with Shortcuts | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F085 | Create from Selection | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F086 | Environment Placeholders | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F087 | Input Placeholders | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F088 | Reserved Strings | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F089 | Address Resolver | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F090 | PDB Explorer | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F091 | VA Memory View | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F092 | VA Step Filter | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F093 | VA Outline | different | 동일 EditorInteractionFixture에서 양쪽 함수1개 표시. VA 오른쪽 도킹/다크, CA 흰 배경 부동 창/Global 노드/이름 중복 형식. |
| VA-F094 | VA View | one-sided | VA View 실행 및 파일/심볼 콤보 확인. CA Workspace 실행 비교는 사용자 중단으로 미실행. |
| VA-F095 | Simple Link Patterns | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F096 | Complex Link Patterns | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F097 | Connect to FogBugz and GitHub | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F098 | Preview Files | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F099 | Open Hyperlinks in an External Browser | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F100 | Custom Plug-ins | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F101 | Context Menus | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F102 | Custom File Extensions | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F103 | Keyboard Shortcuts | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F104 | Mouse | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F105 | Multiple Platforms | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F106 | Non-Microsoft Build Processes | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F107 | Parse Concurrency | not-executed | 실제 양쪽 실행 비교 미완료. 소스/문서 존재로 통과 처리하지 않음. |
| VA-F108 | Options Dialog | one-sided | CA 옵션 창 실제 열기 확인. 모든 옵션 토글 실행 비교 미완료. |

XERJ: 사용 — 비교·검증 코드 검색. 기록 정리 제안1분. 전체 검증은 미완료이며 완료 시각을 기록하지 않습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/runtime-stop-20261017.html)

- 원문 SHA-256: `f656b6b60213a49ad11679647f850b88394353ac11ca9c9a5332a37ee3d78fff`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
