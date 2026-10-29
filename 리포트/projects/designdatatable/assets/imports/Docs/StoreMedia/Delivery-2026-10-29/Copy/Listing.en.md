# Design Data Table – Spreadsheet & Game Data Editor

Edit spreadsheets in Unity. Turn worksheets into typed game data and localization, with 24 playable examples.

## Description

Keep spreadsheet editing and game data in one Unity workflow. Open a workbook, register a worksheet, then use generated C# tables in your game.

## Features

- Edit XLSX and CSV files inside Unity: cells, formatting, filtering, supported formulas, charts and drawing objects.
- Convert worksheets into typed game tables with generated C# access and binary data.
- Keep column names, types and descriptions together with a two-way Summary sheet.
- Define local and global enums. Choose values from popups and see invalid data in the grid.
- Create localization tables and retrieve text through a generated enum-based GetString API.
- Explore 24 independent game scenes: 12 in 2D and 12 in 3D, with data-file buttons and optional autoplay.
- Read the offline guide and use the editor in nine supported languages.
- Connect AI tools through local MCP, CLI or Python for table search, Id/range editing, schema and Enum management, validated batches and native PNG previews.
- Use documented sample code and update recognized source-code summaries in supported languages.

## Quick workflow

1. Create or open an XLSX workbook inside Assets.
2. Choose Game Data → Use as Game Data Table.
3. Review column names and data types, then register the worksheet.
4. Save and access the generated table from C#.

## Requirements

Unity 6000.0.83f1 on Windows; Built-in Render Pipeline sample scenes.

Dependencies: Unity UI (uGUI), 2D Sprite package for sprite-based samples.

Other Unity versions, operating systems, render pipelines and build backends require separate validation. No universal compatibility claim is made.

## Before you buy

- Excel compatibility is partial. Supported formulas are recalculated; unsupported features may be preserved without editing or rendering support.
- External files can be opened. Registration requires a source inside the project Assets folder.
- Game data, localization and local/global Enum workbooks are read-only during Play mode. Edit the source after leaving Play mode.
- Game examples demonstrate data workflows; they are not complete production games.
- AI data operations require Python 3.10+ and an open, idle Unity editor outside Play mode. MCP requires client configuration; editor-free workbook execution is not provided.

## Release notes — 2.0.2 (draft)

- Added enum-based localization and local/global enums with value pickers and validation.
- Added automatic two-way Summary sheets and direct registered-sheet search and navigation.
- Expanded the sample collection to 24 independent 2D/3D games with localization, data links and optional autoplay.
- Added a shared MCP, CLI and Python workflow for validated game-table and spreadsheet editing with native previews.
- Improved input responsiveness, save protection, Play-mode safeguards, performance and contextual editor tools.
- Refreshed nine-language guides, screenshots and sample-code documentation; added source-summary language settings.

## AI assistance

AI-assisted development was used for code, documentation, translations and selected sample visual assets. Included third-party assets and fonts retain their own license notices.

## Third-party notices

See Assets/DesignDataTable/Third-Party Notices.txt and bundled license files. Inter and Pretendard are redistributed unmodified under the SIL Open Font License 1.1.
