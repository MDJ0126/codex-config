---
title: 현재 단축키 등록 비교 — CA 0.0.121
date: 2026-10-23T06:37:32.577Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 검토
category: 이전 기록
summary: 현재 단축키 등록 비교 — CA 0.0.121 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/bindings-live-121/index.html
source_sha256: 7678bc170308a9c821057af90d5741b3a9d9143c94246477176c3b78946f1791
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

현재 단축키 등록 비교 0.0.121

## 현재 단축키 등록 비교 — CA 0.0.121

편집기 범위의 지정 조합 양쪽 등록 15/22 (68.18%). CA 22/22, VA 15/22. 전체 범위·추가 키까지 같은 항목 2/22.

현재 사용자 프로필의 등록 상태입니다. VA에서 키가 비어 있어도 명령이 없는 것으로 보지 않습니다. 공장 기본값이나 실제 키 실행 동등성을 뜻하지 않습니다. 이번 실제 키 입력 0건, 기능 전체 인증 0건. 화면·입력·명령 호출 없이 DTE에서 등록값만 읽었습니다.

| 지정 조합 | VA 명령 | VA 등록 | CA 명령 | CA 등록 | 편집기 조합 일치 |
| --- | --- | --- | --- | --- | --- |
| ALT+K | VisualAssist.FindSelected | [] | Tools.CodeAssistFindSelected | ['전역::Alt+K', '텍스트 편집기::Alt+K'] | False |
| CTRL+SHIFT+V | VisualAssist.Paste | [] | Tools.CodeAssistMultipleClipboards | ['전역::Ctrl+Shift+V', '텍스트 편집기::Ctrl+Shift+V'] | False |
| ALT+SHIFT+H | VisualAssist.VAHashtags | ['전역::Shift+Alt+H', '텍스트 편집기::Shift+Alt+H'] | Tools.CodeAssistHashtags | ['전역::Shift+Alt+H', '텍스트 편집기::Shift+Alt+H'] | True |
| ALT+SHIFT+] | VisualAssist.SmartSelectExtend | ['텍스트 편집기::Shift+Alt+]'] | Tools.CodeAssistExtendSelection | ['전역::Shift+Alt+]', '텍스트 편집기::Shift+Alt+]'] | True |
| ALT+SHIFT+[ | VisualAssist.SmartSelectShrink | ['텍스트 편집기::Shift+Alt+['] | Tools.CodeAssistShrinkSelection | ['전역::Shift+Alt+[', '텍스트 편집기::Shift+Alt+['] | True |
| ALT+] | VisualAssist.SmartSelectExtendBlock | ['텍스트 편집기::Alt+]'] | Tools.CodeAssistExtendBlockSelection | ['전역::Alt+]', '텍스트 편집기::Alt+]'] | True |
| ALT+[ | VisualAssist.SmartSelectShrinkBlock | ['텍스트 편집기::Alt+['] | Tools.CodeAssistShrinkBlockSelection | ['전역::Alt+[', '텍스트 편집기::Alt+['] | True |
| CTRL+0 | VisualAssist.ResetEditorZoom | [] | Tools.CodeAssistResetEditorZoom | ['전역::Ctrl+0', '텍스트 편집기::Ctrl+0'] | False |
| ALT+G | VisualAssist.GotoImplementation | ['텍스트 편집기::Alt+G'] | Tools.CodeAssistGotoSymbol | ['전역::Alt+G', '전역::F12', '텍스트 편집기::Alt+G', '텍스트 편집기::F12'] | True |
| ALT+SHIFT+F | VisualAssist.FindReferences | ['텍스트 편집기::Shift+Alt+F'] | Tools.CodeAssistFindReferences | ['전역::Shift+Alt+F', '전역::Shift+F12', '텍스트 편집기::Shift+Alt+F', '텍스트 편집기::Shift+F12'] | True |
| ALT+SHIFT+O | VisualAssist.OpenFileInSolutionDialog | ['텍스트 편집기::Shift+Alt+O'] | Tools.CodeAssistOpenFile | ['전역::Shift+Alt+O', '텍스트 편집기::Shift+Alt+O'] | True |
| ALT+SHIFT+S | VisualAssist.FindSymbolDialog | ['전역::Shift+Alt+S', '텍스트 편집기::Shift+Alt+S'] | Tools.CodeAssistFindSymbol | ['전역::Shift+Alt+S', '텍스트 편집기::Shift+Alt+S'] | True |
| ALT+O | VisualAssist.OpenCorrespondingFile | [] | Tools.CodeAssistOpenCorrespondingFile | ['전역::Alt+O', '텍스트 편집기::Alt+O'] | False |
| ALT+M | VisualAssist.ListMethodsInCurrentFile | ['텍스트 편집기::Alt+M'] | Tools.CodeAssistListMethods | ['전역::Alt+M', '텍스트 편집기::Alt+M'] | True |
| ALT+SHIFT+G | VisualAssist.GotoRelated | ['텍스트 편집기::Shift+Alt+G'] | Tools.CodeAssistRelatedSymbolCandidates | ['전역::Shift+Alt+G', '텍스트 편집기::Shift+Alt+G'] | True |
| ALT+LEFT | VisualAssist.NavigateBack | ['텍스트 편집기::Alt+왼쪽 화살표'] | Tools.CodeAssistNavigateBack | ['전역::Alt+왼쪽 화살표', '텍스트 편집기::Alt+왼쪽 화살표'] | True |
| ALT+RIGHT | VisualAssist.NavigateForward | [] | Tools.CodeAssistNavigateForward | ['전역::Alt+오른쪽 화살표', '텍스트 편집기::Alt+오른쪽 화살표'] | False |
| ALT+SHIFT+R | VisualAssist.RefactorRename | ['텍스트 편집기::Shift+Alt+R'] | Tools.CodeAssistRename | ['전역::Shift+Alt+R', '텍스트 편집기::Shift+Alt+R'] | True |
| ALT+SHIFT+Q | VisualAssist.RefactorContextMenu | ['텍스트 편집기::Shift+Alt+Q'] | Tools.CodeAssistQuickActionsVisualStudio | ['전역::Shift+Alt+Q', '텍스트 편집기::Shift+Alt+Q'] | True |
| ALT+SHIFT+C | VisualAssist.RefactorCreateFromUsage | ['텍스트 편집기::Shift+Alt+C'] | Tools.CodeAssistCreateFromUsageVisualStudioQuickActions | ['전역::Shift+Alt+C', '텍스트 편집기::Shift+Alt+C'] | True |
| ALT+DOWN | VisualAssist.ScopeNext | [] | Tools.CodeAssistNextScope | ['전역::Alt+아래쪽 화살표', '텍스트 편집기::Alt+아래쪽 화살표'] | False |
| ALT+UP | VisualAssist.ScopePrevious | [] | Tools.CodeAssistPreviousScope | ['전역::Alt+위쪽 화살표', '텍스트 편집기::Alt+위쪽 화살표'] | False |

VA 현재 원본 · CA 현재 원본 · 계산 JSON

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/bindings-live-121/index.html)

- 원문 SHA-256: `7678bc170308a9c821057af90d5741b3a9d9143c94246477176c3b78946f1791`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
