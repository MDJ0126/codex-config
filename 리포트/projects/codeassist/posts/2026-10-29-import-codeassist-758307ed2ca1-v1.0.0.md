---
title: 최신0.0.172 — 전체 검증 미완료
date: 2026-10-24T06:20:05.121Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 최신0.0.172 — 전체 검증 미완료 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/current-172/index.html
source_sha256: f3e9f2c3c05c9bfc006789fa0d275f1ebcbcad897d369e98680059b80daad9d9
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

최신172 자동 대조 결과

## 최신0.0.172 — 전체 검증 미완료

**108개 행을 기록했지만 전체 기능 대조를 완료하지 못했습니다.** 정상적인 양쪽 실행은 1/108개(0.93%)입니다. 대표 동작 일치 1개, 차이 0개, 미검증 107개입니다. 0.93%는 이번 실행에서 확인된 대표 표본의 최소 비율이며 제품 전체 동기화율이 아닙니다. 전체 동기화율 산출은 실패했습니다. 구버전17/108은 합산하지 않았습니다.

## 실행기 실패 원인

VA의 RPC_E_CALL_REJECTED와 남아 있던 VA Sort Selected Lines 및 Visual Studio의 예기치 않은 오류 대화상자를 확인했습니다. 잘못된 Edit.TypeChar 명령, Create file 취소 누락, 분리 실행 파일의 EnvDTE 의존성 누락과 lease.json 파일 공유 잠금도 확인했습니다. 수정 후 재실행했지만 CA 시간 초과로 격리되어107개 기능의 정상 양쪽 비교는 완료하지 못했습니다. 호스트 격리로 건너뛴 행과 옵션 창 열기·닫기를 기능 검증 완료로 세지 않습니다.

## Welcome 수정173

번역표와 버튼 사이 공백 개수 차이를 수정했습니다. Copy feedback은 고정된 GitHub 프로젝트 이슈 작성 주소에 인코딩된 제목·본문을 넣어 여는 버튼으로 바꿨습니다. 사용자가 브라우저에서 확인 후 등록하며 자동 게시하지 않습니다. 영어·한국어 및 URL 인코딩 검사 포함461/461개 통과. 두 SDK 빌드와 검사 프로필173 설치 바이너리6개 확인. 설치 후 실제 Welcome 화면은 아직 미검증입니다. 수정 소스는6ff9790으로 푸시했습니다.

## 108개 최신 판정표

| ID | 기능 | 상태 | VA / CA | 근거 |
| --- | --- | --- | --- | --- |
| VA-F001 | Find by Context | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F002 | Find References | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F003 | Find Symbol in Solution | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F004 | GoTo Implementation | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F005 | GoTo Member | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F006 | GoTo Related | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F007 | Highlight Current Line | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F008 | List Methods in File | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F009 | Move Scope | representative-match | complete / complete | Only this explicit representative behavior matches |
| VA-F010 | Navigate Back and Forward | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F011 | Open Corresponding File | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F012 | Open File in Solution | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F013 | VA Hashtags | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F014 | VA Navigation Bar | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F015 | Access to Refactoring Commands | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F016 | Change Signature | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F017 | Convert Between Pointer and Instance | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F018 | Encapsulate Field | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F019 | Extract Method | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F020 | Introduce Variable | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F021 | Move Implementation to Header File | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F022 | Move Implementation to Source File | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F023 | Move Method Implementations to Source File | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F024 | Move Selection to New File | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F025 | Rename | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F026 | Rename Files | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F027 | Simplify Instance Declaration | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F028 | Add Forward Declaration | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F029 | Add Member | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F030 | Add Missing Case Statements | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F031 | Add Similar Member | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F032 | Add Using Directive | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F033 | Add include Directive | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F034 | Add/Remove Braces | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F035 | Create Declaration | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F036 | Create File | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F037 | Create from Usage | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F038 | Create Implementation | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F039 | Create Method Implementations | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F040 | Document Method | timeout | action-error / timeout | Diagnostic worker exceeded shared item budget |
| VA-F041 | Implement Interface / Virtual Methods | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F042 | Acronyms | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F043 | Auto-Extend Multi-Line Comments | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F044 | Enhanced Listboxes | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F045 | Format After Paste | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F046 | Insert () and Closing Brackets and Quotes | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F047 | Insert _ After m and Shift | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F048 | Multiple Clipboards | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F049 | Reset Editor Zoom | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F050 | Shorthand | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F051 | Smart Select | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F052 | Smart Suggestions | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F053 | Sort Selected Lines | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F054 | Suggestion Lists | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F055 | Surround Selection | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F056 | Surround With | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F057 | Column Indicator | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F058 | Context Field | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F059 | Definition Field | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F060 | Enhanced Syntax Coloring | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F061 | Local Symbols in Bold | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F062 | Highlight Find Results | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F063 | Highlight Matching Words | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F064 | Highlight References to Symbol Under Cursor | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F065 | Parameter Info | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F066 | Print in Color | timeout | timeout / host-quarantined | Diagnostic worker exceeded shared item budget |
| VA-F067 | Quick Info | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F068 | System Symbols in Italics | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F069 | Convert Dot to -> | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F070 | Highlight Files in Output Window | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F071 | Highlight Matching Braces and Parentheses | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F072 | Highlight Mismatching Braces and Parentheses | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F073 | Repair Case | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F074 | Underline Spelling Errors | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F075 | Underline Unknown Symbols | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F076 | Code Inspection Settings | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F077 | Code Issues in the Editor | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F078 | Quick Fixes in the Editor | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F079 | Quick Fixes in a Tool Window | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F080 | VA Snippets | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F081 | for Refactoring | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F082 | for Smart Suggestions | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F083 | for Surround With | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F084 | with Shortcuts | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F085 | Create from Selection | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F086 | Environment Placeholders | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F087 | Input Placeholders | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F088 | Reserved Strings | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F089 | Address Resolver | unverified | host-quarantined / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F090 | PDB Explorer | unverified | host-quarantined / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F091 | VA Memory View | unverified | action-error / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F092 | VA Step Filter | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F093 | VA Outline | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F094 | VA View | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F095 | Simple Link Patterns | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F096 | Complex Link Patterns | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F097 | Connect to FogBugz and GitHub | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F098 | Preview Files | unverified | host-quarantined / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F099 | Open Hyperlinks in an External Browser | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F100 | Custom Plug-ins | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F101 | Context Menus | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F102 | Custom File Extensions | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F103 | Keyboard Shortcuts | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F104 | Mouse | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F105 | Multiple Platforms | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F106 | Non-Microsoft Build Processes | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F107 | Parse Concurrency | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |
| VA-F108 | Options Dialog | unverified | complete / host-quarantined | Host/worker blocked; no pass inferred |

집계 JSON · 108개 판정 · 461개 검사 · 실행 계획

제안 최초30분, 이후 남은 예산20분으로 변경. 기록 시각 2026-10-24T15:20:05.117982+09:00, 최초 착수 이후1672초,20분 예산 시작 이후1575초. 기한을 초과했고 전체 요청은 미완료입니다. XERJ 사용, 누락은 rg로 보완.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/current-172/index.html)

- 원문 SHA-256: `f3e9f2c3c05c9bfc006789fa0d275f1ebcbcad897d369e98680059b80daad9d9`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
