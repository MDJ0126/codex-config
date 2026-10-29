---
title: 아이콘 소스 감사 — 현재 0.0.121
date: 2026-10-23T06:41:13.461Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: 아이콘 소스 감사 — 현재 0.0.121 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/icon-source-audit-121/index.html
source_sha256: 3ff6bf3c13913aa36a2a459e8dc6a52a8ef82a3c383a56d37df7eefb542a5af0
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

아이콘 소스 감사

## 아이콘 소스 감사 — 현재 0.0.121

SVG 원본 13개에서 text 노드는 0개입니다. WPF 공통 아이콘에는 12개 명시 경로가 있습니다. 정적 호출 중 Forward는 기본 오른쪽 화살표 경로를 사용합니다. 이 사실만으로 표시 오류라고 판단하지 않았습니다.

닫기 버튼의 ×, 대응 파일 버튼의 문자 초기값은 실제 Content에서 Path 아이콘으로 교체됩니다. 문자열 검색만으로 텍스트 대체 아이콘이 화면에 남았다고 판정하지 않았습니다.

아래는 원본 SVG 미리보기이며 실제 VS 화면 캡처가 아닙니다. 동적 이름·DPI·테마·VA 배치는 미검증입니다. 소스 검사로 새 제품 동등성 통과를 추가하지 않았습니다.

| 원본 | text 노드 | 에셋 미리보기 |
| --- | --- | --- |
| BananaIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/BananaIcon.svg) |
| CorrespondingIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/CorrespondingIcon.svg) |
| FileIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/FileIcon.svg) |
| NextIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/NextIcon.svg) |
| PreviousIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/PreviousIcon.svg) |
| ReferencesIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/ReferencesIcon.svg) |
| SkipIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/SkipIcon.svg) |
| SnippetIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/SnippetIcon.svg) |
| SpellIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/SpellIcon.svg) |
| SymbolIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/SymbolIcon.svg) |
| ThreadIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/ThreadIcon.svg) |
| toolbar-strip.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/toolbar-strip.svg) |
| WelcomeIcon.svg | 0 | ![참고 화면](../assets/imports/CodeAssist.Extension/Resources/Toolbar/WelcomeIcon.svg) |

호출 위치·원본 해시

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/icon-source-audit-121/index.html)

- 원문 SHA-256: `3ff6bf3c13913aa36a2a459e8dc6a52a8ef82a3c383a56d37df7eefb542a5af0`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
