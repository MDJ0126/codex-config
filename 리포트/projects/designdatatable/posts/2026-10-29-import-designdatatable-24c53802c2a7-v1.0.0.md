---
title: Object property entry points
date: 2026-10-22T08:04:47.717Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 버그
category: 이전 기록
summary: Object property entry points — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-22/submission-english-ui/ObjectPropertiesFix.md
source_sha256: 0b6c5211c468bbb0ebb5fb812608d39a64cf04a85a67927a5631df3147ab29a9
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Verified in Unity 6000.0.83f1 on Windows, English UI, 2026-10-22.

The ribbon constructed WorkbookObjectPopup without the selected drawing part and ID. ObjectPointer handled selection and dragging but had no double-click property action.

The ribbon now calls ShowObjectProperties, which passes the current object identity. A left double-click opens the same editor and clears the drag gesture/hot control.

Actual UI checks after recompilation:
- Double-clicked the ellipse in ClosedShapeGradientsUi.xlsx: Objects opened with Shape 3 selected, anchor E1, size 160 x 85 and text Gradient ellipse.
- Clicked the ribbon Objects button with that ellipse selected: the same Shape 3 identity and values remained selected.
- Captures: object-properties-double-click-after-fix.jpg and object-properties-ribbon-after-fix.jpg.
- Backend: ../unity-backend-165751/complete.json, 160 groups, 14052 assertions, zero failures.

Not verified by these checks: applying gradient edits or saving a modified workbook. The automation tool activates the Unity root before popup input; an attempted scroll dismissed the transient popup. This tool limitation remains separate from the verified entry-point fixes.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-22/submission-english-ui/ObjectPropertiesFix.md)

- 원문 SHA-256: `0b6c5211c468bbb0ebb5fb812608d39a64cf04a85a67927a5631df3147ab29a9`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
