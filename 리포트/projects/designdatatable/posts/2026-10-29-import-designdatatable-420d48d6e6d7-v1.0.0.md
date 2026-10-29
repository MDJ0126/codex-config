---
title: Hover activation fix
date: 2026-10-26T08:32:50.418Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 버그
category: 이전 기록
summary: Hover activation fix — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/taskbar-hover-fix/Verification.md
source_sha256: cf772c6f50199b9e8a8080d2dd6c3856a4160b881cf003c167c727e3fe93f61a
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The user reported neither thumbnail nor temporary peek appeared.

Observed in Unity: main native outer bounds were (1912,-8)-(3848,1040), whereas GetMainWindowPosition returned client (1920,43), 1920x989. The old strict outer-rectangle comparison failed. Matching now also checks client origin and DWM visible-frame bounds, retaining process and ambiguity guards. The read-only probe confirmed the corrected main handle resolves and the exposed condition becomes true.

A second live blocker was a persistent pressedEntry after domain reload, with no active drag. Transient gesture fields now opt out of reload serialization and OnEnable clears them. A 200ms recovery cancels an abandoned press if native mouse-up was missed. The post-reload probe confirmed pressedEntry is empty. The user manually retried and confirmed successful operation, then asked to finish.

Validation: .NET build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors. 49 hover/geometry and 153 overflow assertions passed; git diff --check passed. Native state inspection was read-only: no mouse/keyboard automation, focus changes, screenshots or window activation were driven by tools. Screenshot recapture remains unperformed under the existing computer-use prohibition. No new native visual documentation claims were made.

Temporary Assets/Editor/DDTHoverReadOnlyProbe.cs, its meta and request marker were removed. Runtime evidence retained here includes only Unity records. Previous test evidence is preserved.

XERJ: used the project MCP index to locate WorkbookMinimizedShelf, then inspected current source and actual Unity runtime state.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/taskbar-hover-fix/Verification.md)

- 원문 SHA-256: `cf772c6f50199b9e8a8080d2dd6c3856a4160b881cf003c167c727e3fe93f61a`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
