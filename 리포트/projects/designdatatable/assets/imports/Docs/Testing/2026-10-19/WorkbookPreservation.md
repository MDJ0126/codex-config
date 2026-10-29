# Workbook object preservation — 2026-10-19

Existing XLSX saves now retain the original worksheet structure and replace the editable data portion. Drawing connections, charts, images, relationships, unchanged rich text, cell style indices, print settings and other worksheet elements are retained. Unchanged package parts, including the summary worksheet and chart/media parts, are copied byte-for-byte. Original shared strings and styles remain available. Single-sheet saves also use this preservation path.

Safeguards:
- Chart/drawing/table/external-reference workbooks and worksheets with merges, conditional formatting, validations or hyperlinks reject changes to row count, column count, names or types. This includes automatic registration that inserts Id/schema rows. Prepare the native Id/name/type layout in Excel first; the registration popup recognizes an existing native layout without adding another Id column.
- Digital signatures, pivot/slicer parts, macros, ActiveX and embedded objects block original saves pending verified handling.
- Object-containing workbooks receive a copy in Tools/WorkbookBackups before replacement.
- Writes use a temporary package and atomic replacement; bounded retries handle replacement failures without falling back to destructive overwrite.
- Protection messages are translated in all nine UI languages. Nine HTML guides include the safety limits.

Verification: 10,343 assertions passed. Single-sheet and multi-sheet fixtures each contain a chart and image in the data sheet; the multi-sheet fixture also has a chart, image and summary formula on a second sheet. Package comparison verifies original chart/media/relationship parts, drawing references, summary worksheet, print settings, styles and rich text. Independent openpyxl reading confirms all charts/images remain readable, chart ranges remain unchanged, the summary formula is preserved and edited B3 equals 99. Rejected structural/pivot saves leave the complete original file byte-identical.

Actual Microsoft Excel rendering, chart-cache refresh and every third-party workbook feature have not been verified. Chart drawing/editing is not implemented. The tests establish preservation for the documented fixtures; they do not certify arbitrary Excel documents. See workbook-preservation/independent-reader.json and unity-session.jsonl for execution evidence.

Final isolated Unity run: 135 checks passed, including five workbook-object checks and all three sample scenes. The first attempt failed in an existing JSON/TSV/XML export replacement; after adding bounded atomic-replace retries, the complete fresh run passed. The failed execution remains in history.

