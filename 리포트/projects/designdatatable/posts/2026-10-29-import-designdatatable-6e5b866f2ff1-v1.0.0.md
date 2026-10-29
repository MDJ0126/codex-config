---
title: Fade runtime diagnosis and bitmap content correction
date: 2026-10-26T12:05:04.751Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Fade runtime diagnosis and bitmap content correction — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/preview-fade-clock/Verification.md
source_sha256: f13db169355b936de33f38aa26c215f45bef9dbd7803f89d26d5b9e2ce9db6c3
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The user confirmed that the first bitmap implementation did not animate correctly, then observed black followed by the original card after the clock fix. These observations are accepted as runtime evidence.

Confirmed source defect: the animation lived in EditorWindow.Update while the real window remained hidden until completion. Unity 6000.0 documents Update as running on visible windows. It now subscribes directly to EditorApplication.update and unsubscribes on disable.

Actual Unity logs after that change confirmed begin -> midpoint -> complete. The user's black-frame observation showed that a successful PrintWindow call did not establish valid captured Unity GPU content. This path is removed from PreviewFade.

The fade frame is now composed from the same card background/border/accent colors, existing status-icon texture and pre-minimize document snapshot. Non-readable source textures are read through a temporary RenderTexture, not a hidden-window screenshot. The complete BGRA buffer is uploaded to an offscreen GDI bitmap and its title/close glyphs are drawn before the separate overlay is faded. Real Unity/document window alpha, group restore and minimize code are unchanged.

Native error stages are recorded as taskbar.fade.prepare.<phase>.<code>; successful begin/midpoint/complete events contain no document contents or paths. No screen capture or UI control tools were used.

Unity 6000.0 reference:
https://docs.unity3d.com/6000.0/Documentation/ScriptReference/EditorWindow.Update.html

Actual visual confirmation of the newly composed frame remains pending. Existing historical screenshot/source hashes are unchanged.

Validation after frame correction: build passed with zero warnings/errors; 88 hover assertions and 153 taskbar assertions passed. The new offscreen GDI test verified bitmap upload, readback, BGR channels and top-down orientation without creating or showing a window. Native visual confirmation remains pending.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/preview-fade-clock/Verification.md)

- 원문 SHA-256: `f13db169355b936de33f38aa26c215f45bef9dbd7803f89d26d5b9e2ce9db6c3`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
