# Object property entry points

Verified in Unity 6000.0.83f1 on Windows, English UI, 2026-10-22.

The ribbon constructed WorkbookObjectPopup without the selected drawing part and ID. ObjectPointer handled selection and dragging but had no double-click property action.

The ribbon now calls ShowObjectProperties, which passes the current object identity. A left double-click opens the same editor and clears the drag gesture/hot control.

Actual UI checks after recompilation:
- Double-clicked the ellipse in ClosedShapeGradientsUi.xlsx: Objects opened with Shape 3 selected, anchor E1, size 160 x 85 and text Gradient ellipse.
- Clicked the ribbon Objects button with that ellipse selected: the same Shape 3 identity and values remained selected.
- Captures: object-properties-double-click-after-fix.jpg and object-properties-ribbon-after-fix.jpg.
- Backend: ../unity-backend-165751/complete.json, 160 groups, 14052 assertions, zero failures.

Not verified by these checks: applying gradient edits or saving a modified workbook. The automation tool activates the Unity root before popup input; an attempted scroll dismissed the transient popup. This tool limitation remains separate from the verified entry-point fixes.
