---
title: Documentation and store refresh — 2026-10-29
date: 2026-10-28T18:01:26.751Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Documentation and store refresh — 2026-10-29 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-29/docs-store-refresh/Results.md
source_sha256: b5cab10e1c2c1b53e6734b54af6693584d561720218a4d075c7d560bbe10a239
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

## Delivered

Nine concise offline HTML guides, English/Korean PDFs, locally bundled Inter 4.1 and Pretendard 1.3.9 with unmodified source provenance and SIL OFL licenses. Two-tab general/game-data navigation and feature/description tables remain concise. No CDN or system-font installation is required.

All 25 active guide images were newly captured in an isolated native Unity 6000.0.83f1 Windows project: 15 workbook/UI screens, 6 selected dialogs, 4 real Play-mode game renders. The capture run also produced 27 dialog states for inspection. Current source bytes were compared against the isolated project before recording manifest hashes. Capture completion timestamps and native logs are preserved. Repeated old check lines in native append logs are not counted as extra assertions.

Current store delivery: 7 feature gallery images and 4 key images, original native capture pixels, editable HTML/CSS/SVG sources and local font licenses. Copy is separated into canonical JSON, English listing, Korean review copy and portal fields. Historical listing files point to the current canonical source. A verified ZIP contains the complete local still-image delivery.

## Verification

- Nine-language link, image and current source-hash verification: zero errors; 25 active images, 93 unused historical records preserved.
- 128 headless browser checks: tabs, keyboard, anchors, entry links, mobile bounds, image decoding and bundled font loading.
- 11 store PNGs: expected dimensions, loaded native images, element bounds; contact sheet and corrected card individually inspected.
- Two PDFs, 11 pages each: all 22 rendered pages visually inspected; text extraction and embedded glyph programs checked. Chromium serializes variable-font glyphs as Type3. Poppler emitted a Type3 bounding-box warning while rendering; no page or text extraction failure occurred. An initial assertion that PDF resources retained font-family names was incorrect and was replaced with embedded-glyph and text checks; browser font-family loading is tested separately.
- Store file hashes, canonical-copy parity, archive CRC and every archived payload hash verified.

## Scope

No physical desktop input, no foreground interaction, no live portal upload, no store submission and no newly approved package binary. English native UI was captured; all nine native UI language layouts were not separately certified. Older videos show earlier UI and are excluded from this current still-image delivery. Before upload, confirm the portal image slots and validate the selected release package.

XERJ: failed — server was not running; rg fallback used.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-29/docs-store-refresh/Results.md)

- 원문 SHA-256: `b5cab10e1c2c1b53e6734b54af6693584d561720218a4d075c7d560bbe10a239`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
