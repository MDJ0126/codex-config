# Bottom status reservation and subtle worksheet curves

The console-message row above the document shelf is Unity's native status row. Saved project layout evidence shows MainView.bottomViewHeight=20 while the AppStatusBar view persisted height=48 (the native 20 plus our 28). Old attach logic used the current view height as originalHeight and added another 28. Reloading that expanded rectangle produces 76, leaving a 28px spare area in the wrapped native row. See SavedLayoutEvidence.json; the user layout itself was only read, not edited.

Fix: use the parent MainView configured native height, reserve exactly native+28, preserve the bottom edge, and release exactly the native row. Clipping confines original content to its row. When no documents remain, recognized orphan DDT height increments are repaired without changing unrelated heights. Repeated-reservation and detach/reattach checks verify idempotence and recovery.

Both WorkbookViewer and DataTableEditor worksheet lists now use small 5px rounded corners and inset selection lines. Order, sizing, scrolling, selection/context actions, source names and semantic colors are retained. Existing 2px/3px selection-line thicknesses are retained in their respective editors. No changes were made to workbook content.

Validation: final .NET build against Unity 6000.0.83f1 libraries passed with zero warnings/errors. Status geometry: 92 assertions; existing hover: 54; overflow: 153; total 299. git diff --check passed. All nine guides have the new localized bottom-strip note; English remains default.

Not verified: actual Unity rendering, black-gap disappearance, native console/progress interaction and rounded tab appearance. Computer use and UI capture remain prohibited by the user. Screenshot/source hashes were preserved; documentation verification continues to fail for pending/stale captures rather than claiming visual verification. No runtime probe was installed.

XERJ: project MCP search for WorkbookMinimizedShelf, followed by current source and saved-layout inspection. Unity reference source was consulted only as API/layout context; implementation is project-authored.
