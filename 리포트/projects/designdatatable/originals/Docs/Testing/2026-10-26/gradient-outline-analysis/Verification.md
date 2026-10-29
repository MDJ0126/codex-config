# ClosedShapeGradientsUi outline comparison

Read-only analysis. No workbook or product source was changed.

Original: Assets/Editor/StoreAuditSamples/ClosedShapeGradientsUi.xlsx
Published: Assets/DesignDataTable/Sample/StoreAuditSamples/ClosedShapeGradientsUi.xlsx

Both packages have 13 shapes. Neither package contains a:ln, a:lnRef, xdr:style or a theme part. Canonical XML of all 13 xdr:spPr elements matches between original and published copies. Packaging changed anchors/labels, not shape properties.

WorkbookDrawings.cs:119 sets NoLine only when a:ln/a:noFill exists. With no a:ln, NoLine becomes false, Line remains empty and LineWidth defaults to 9525/9525 = 1 pixel. WorkbookDrawingGUI.cs resolves an empty line color to the editor accent and draws a stroke whenever NoLine is false. The prior production-raster preview independently shows these outlines.

The supplied Google Sheets screenshot has no visible outlines. For this sample, borderless display matches the supplied reference and no authored outline exists in the package. The added stroke is the current renderer fallback, not a difference introduced in the distributed workbook. This does not establish that every missing a:ln should mean no line: inherited shape styles must be resolved where present. This sample has none.

Official structure references:
https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.drawing.outline?view=openxml-3.0.1
https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.drawing.linereference?view=openxml-3.0.1

Excel desktop rendering was not run. No computer use was performed. XML/source and prior target-raster execution establish the fallback mechanism; the supplied screenshot establishes the reference appearance.
