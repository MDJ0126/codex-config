# Game Data tab label follows registration state

Ordinary sheets retain “Switch to Game Data”. Already registered sheets use the existing localized “Game Data” label and “Registered · Edit game data” help text. The same registered-state flag controls the button title and custom help.

WorkbookViewer supplies IsGameData, which resolves the active sheet's registration. DataTableEditor supplies tableInfo != null. Both shared-ribbon callers now pass that state; selecting a different sheet refreshes it through the existing GUI presentation state. The pill style, tab index, registration command and editing behavior are unchanged. Existing nine-language strings are reused.

All nine guide descriptions were regenerated. Native UI screenshots/visual interaction were not captured under the user's existing computer-use prohibition. Historical screenshot/source hashes were preserved. Documentation verification reports only the pending/stale native capture backlog.

XERJ found the ribbon.gameaction rendering site, validated against current source and both actual call sites.

Build passed with zero warnings/errors. All 82 ribbon artwork/layout assertions passed, including ordinary-vs-registered tab label selection. Actual Unity visual confirmation remains pending.
