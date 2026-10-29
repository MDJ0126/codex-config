---
title: Image and shape preview — 2026-10-19
date: 2026-10-19T14:03:05.145Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Image and shape preview — 2026-10-19 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-19/DrawingPreview.md
source_sha256: 3bdb77dce0419fe9744d8ae028fe8ebe3d1fb693937e410c992048546b936f39
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The saved-original viewer now reads embedded PNG/JPEG images and basic DrawingML rectangles, rounded rectangles, ellipses, triangles, diamonds, straight connectors and text boxes. Drawing anchors, stacking order relative to charts, image crop, flip, rotation, explicit/theme colors, opacity, line width and basic text are used for simplified read-only display. It is not Excel-identical rendering. Custom/grouped shapes, SmartArt and unsupported image formats receive placeholders; legacy/unreadable drawing information receives a warning. The data ribbon shows the object count.

Viewing never writes or reconstructs media/drawing parts. Existing safe-save logic retains source media, drawing XML and relationships byte-for-byte, including unsupported grouped shapes. Complex structural saves remain blocked and object-containing workbooks are backed up. Preservation does not depend on whether an object can be displayed.

Embedded image limits: 8MB each, 8192 maximum dimension, 16M pixels each, 64MB compressed bytes per workbook and 16M decoded pixels per viewer. Image data is shared by package key, loaded into native textures only when visible, and released on reload/close. PNG chunk CRC and dimensions are checked before decode. External image URLs are never fetched. Data-only table loading skips object payloads.

Verification: 10,680 assertions passed. Fixtures include chart, image, seven supported shape/text types, custom star and grouped shape, Unicode text, fill/line colors, alpha, rotation, reflection and font size. Read leaves original bytes unchanged; data-save comparison retains drawing/media and the summary sheet exactly. Native Unity: 144 checks passed, including PNG and JPEG decoding, all-object detection, unsupported markers and shape bounds/content, plus preceding chart/preservation and three-scene tests. The initial native run exposed a bad CRC in the old test PNG; the test image was corrected and PNG CRC validation added. Failed history is retained. The final data-only-loading and placeholder-bound refinement passed the complete C# suite after the native run.

Actual interactive UI layout, clipping, image appearance and shape geometry have not been visually verified. Shapes/text and theme effects are approximations; editing objects and external/complex geometry rendering are not supported. See drawing-preview/unity-session.jsonl and summary.json.

Final complete native run includes a JPEG relationship/dimensions fixture and successful native JPEG decoding. It also includes the final data-only loading optimization and placeholder bounds.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-19/DrawingPreview.md)

- 원문 SHA-256: `3bdb77dce0419fe9744d8ae028fe8ebe3d1fb693937e410c992048546b936f39`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
