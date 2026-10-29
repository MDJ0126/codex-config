# Excel workflow improvements — 2026-10-19

## Implemented

- Paste Special: captured values, transpose, skip blanks, add/subtract/multiply/divide. Ctrl+Alt+V opens the accent-themed popup; Ctrl+Shift+V pastes captured values.
- Scalar paste fills the selected rectangle. Dense blocks repeat into exact multiples; incompatible dimensions reject atomically. Sparse source holes remain untouched and sparse blocks cannot tile.
- View: formula display (Ctrl+`), visible cells (Alt+;), blanks, formulas and constants. Ctrl+PageUp/PageDown switches registered sheets outside text editing.
- COUNTA, COUNTBLANK, COUNTIF, SUMIF and AVERAGEIF; formula help now lists 27 functions. Conditional criteria support comparisons, criterion cells, text wildcards and escapes.
- Nine-language UI strings and guides updated; English remains the default for a new workspace. English PDF regenerated and seven rendered pages inspected.

## Debugging evidence

The added regression `row insertion preserves address-looking literal String values` failed before the fix. String columns containing `=A1` must remain literal text. Typed translation guards now cover paste, sparse paste, fill, auto-fill, sort, insertion/deletion and cut/move. Quoted text within numeric formulas is excluded from reference rewriting and F4 toggling.

COUNT and the new counting functions now propagate reference cycles instead of silently treating a cycle as an uncounted value. New tests cover direct and indirect cycles. A localization key collision (`paste.note`) was caught during execution and corrected to `special.note`.

Arithmetic paste validates the complete candidate before applying it. Tests cover division by zero, overflow, fractional results into integer fields, invalid types, duplicate Ids and disabled expansion, confirming that failed operations preserve the original table. Copied values are immutable snapshots rather than recalculated from a later-edited source.

## Evidence and remaining verification

- Latest core run: **8,981 assertions passed**, compiling all project C# sources against installed Unity assemblies.
- Prior native run: **81 checks passed**, including real editor Undo/Redo, localization, function help and all three sample scenes. See [record](final/unity-session.jsonl).
- Final latest-source native rerun: **82 checks passed**, with no recorded fail/runtime-error/runner-error. All three scenes completed at 2026-10-19 11:36:51 KST. [Execution record](verified/unity-session.jsonl). This includes the added String-literal regression, actual editor Change/Undo/Redo APIs and Mono formula evaluation. These API checks do not replace mouse gesture tests. The runner now rejects stale reports.
- The focused advanced suite passed **106 assertions**, included within the 8,981 total rather than additional to it.
- [Captured Korean workspace](screenshots/workspace-ko.png) illustrates the existing layout. It was captured from the original editor with an older loaded assembly; it does **not** verify the newly added controls.
- Manual cell gestures and the new popup layout remain **unverified**. Closing Formula Help responded, but cell input did not reliably reach the expected target. Do not infer successful input from the presence of a screenshot.
- The first final batch retry failed to initialize/reconnect the Unity Licensing Client before test entry. This is distinct from a product assertion failure.
- The existing `output/release/DesignDataTable.unitypackage` predates these additions. It is not the current source snapshot. A new submission candidate and validator run must be produced when preparing release.

## Remaining Excel gaps

This is a typed game-data editor, with a numeric formula subset. Complete Excel criteria coercion is not implemented; SUMIF/AVERAGEIF ranges must have identical dimensions. Arithmetic paste writes results and replaces destination formulas. Native rich formatting, merged cells, charts, pivot tables, macros, external workbook references and full Excel formula coverage are not implemented. These require separate product decisions and validation rather than being marked complete.

## Reference comparison

- [Microsoft Paste options](https://support.microsoft.com/en-au/excel/paste-options?nochrome=true): values, transpose and arithmetic operations informed the added workflow.
- [Microsoft COUNTA](https://support.microsoft.com/en-gb/excel/functions/counta-function?nochrome=true): nonempty information, text and errors informed regression cases.

XERJ: 미사용 — 연결 도구가 없어 rg로 현재 소스를 확인했습니다.
