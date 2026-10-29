# Minimal two-tab documentation and animation preference

## Implemented
- A persisted ON-by-default Window animations option in Settings > Appearance, localized in all nine UI languages. Reset restores ON. OFF bypasses minimize/restore/maximize motion and preview fade/rise/hover transitions while preserving hover delays. Toggling OFF completes/cancels active captured surfaces safely on their next update.
- Replaced append-only user-guide generation with one concise source of truth: seven green spreadsheet chapters and seven orange game-data chapters. Features use short Feature/Description tables. Conversion has four steps and a typed XLSX example; Summary, local/global Enums, validation, Play read-only, localization and runtime API are covered.
- 19 newly rendered native Unity screenshots: 15 workbook/popup/preferences views and four actual Play-mode games. Games alternate 2D/3D and are paired with their real source tables. Native English UI only; non-English Unity layouts were not visually certified.
- Nine offline HTML guides and entry pages, keyboard-accessible tabs, language switching, direct chapter links, mobile layout, and refreshed English/Korean 11-page PDFs with two section colors and page numbers.

## Verification
- Production/editor C# build: 0 warnings, 0 errors.
- Six unique native preference checks passed, including migration from an old preference file without the animations field, persistent OFF/ON, Reset, and disabled restore/maximize dispatch.
- 151 taskbar hover and 153 overflow assertions passed.
- 113 headless Microsoft Edge checks passed across nine languages: tabs, color, keyboard access, anchors, language links, image decode, mobile overflow, no JS errors. Isolated profiles; no user browser or desktop input.
- Four actual Unity Play captures completed without recorded Error/Exception; representative 2D/3D scene bytes and relevant runtime source hashes match the main workspace.
- Current guide verification: nine languages, 19 referenced screenshots, zero link/image/source-hash errors. The 93 unused older capture records and pending historical notes are retained; they are not certified current or silently re-hashed.
- English and Korean PDFs each have 11 pages; all pages rendered with Poppler, contact sheets and representative full-size pages visually reviewed. Runtime code and animation-setting text extracted successfully.

## Limits
Physical pointer/keyboard interaction with live document animation settings was not injected; native callbacks, persistence, guarded dispatch and rendered UI were tested. Actual desktop minimize/maximize feel remains unverified. Independent native-speaker review of translations was not performed. Initial headless launch/profile and a Japanese mobile heading overflow were recorded and resolved before the passing run.

XERJ failed: node not running. rg was used for current source checks.
