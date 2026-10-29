---
title: Background hover and reusable prepared preview host
date: 2026-10-26T10:56:44.311Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Background hover and reusable prepared preview host — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-background-host/Verification.md
source_sha256: 9410380549f8364f529a4be37577197ea2daeb76a1a6b8e732314ebb00482d8c
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The user reported ongoing preview flicker and no mini card when Unity was not focused.

Confirmed source condition: UpdateHover cancelled whenever PreviewProcessActive was false, or the foreground handle differed from the entry snapshot. Both foreground conditions have been removed. Eligibility now uses actual exposed taskbar/card hit testing for all preview states, including during peek, so another application covering an entry prevents activation. Hover never requests foreground activation; explicit document clicks retain the existing native activation path.

Lifecycle changes: one card host is cached per shelf. Hover exit, retarget and click hide it with SWP_NOACTIVATE instead of destroying its EditorWindow/ContainerWindow. Same-source DWM leases are retained across reuse; source changes reset the lease while the destination is hidden. Removing the shelf, closing all documents or reloading assemblies closes the cache. Hidden cards do not schedule idle repaint retries, and their native host is excluded from layout saving. The card is an unowned NoShadow custom host, replacing Unity's tooltip host mode.

Initial presentation: project-authored reflection setup uses the installed HostView/ContainerWindow APIs with loadPosition=false, displayImmediately=false and setFocus=false. After layout preparation, the owned native handle is located even when hidden, configured without activation, painted and then exposed. Reuse prepares the hidden host before showing. The retained native handle is used for DWM registration while hidden, avoiding a handle-not-ready placeholder retry. Native pointer capture is released only when owned by that card.

Validation: final .NET build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors. Existing tests include installed prepared-host API availability, timer/click/ownership behavior, native ABI and geometry: 77 hover, 153 overflow, 92 status-layout assertions (322 total). git diff --check passed. Nine localized guide notes updated, with English default. No new tests were added that pretend to execute native rendering or host reuse.

Limits: host creation/reuse, inactive-Unity presentation, current native focus/input behavior and pixel-level flicker elimination have NOT been executed/visually verified by tools. A manual retry question is pending. The former foreground gate is a verified source blocker; complete visual root cause and resolution remain unconfirmed. Computer use remains prohibited. No probe, screenshots, key/mouse injection, foreground manipulation or user app restart was executed by tools. Documentation capture/source-hash verification remains failing for pending/stale captures and historical hashes were preserved.

XERJ: project MCP search for PreviewProcessActive followed by current source and Unity 6000.0 reference API inspection. Reference context: https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Editor/Mono/ContainerWindow.cs and https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Editor/Mono/HostView.cs . Framework source was consulted as API context; the reflection setup is project-authored.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-background-host/Verification.md)

- 원문 SHA-256: `9410380549f8364f529a4be37577197ea2daeb76a1a6b8e732314ebb00482d8c`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
