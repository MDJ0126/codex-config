# Game Data pill action

Changed the far-right orange rectangular tab to a fully rounded 24px-high pill, inset 3px vertically. The label is 게임 데이터로 전환하기 in Korean and Switch to Game Data in default English. All nine supported languages have localized action labels. Width follows the bold measured label plus 32px padding; the left tab strip continues to scroll. The rectangular selected underline was removed. The existing current-sheet-as-game-data custom help and Game Data tool-panel behavior remain intact.

Validation: build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors; 80 existing ribbon artwork/layout assertions passed. The new translation row has all nine values and the exact Korean/English labels. git diff --check passed. Nine guide descriptions updated.

Actual Unity appearance, translated layouts and screenshot recapture were not verified because the user prohibited computer use. Historical capture/source hashes were preserved; localized-doc verification intentionally remains failing for pending/stale UI captures. No new tests or runtime probes were added for this reversible UI change.

XERJ: project MCP search for SpreadsheetRibbon, followed by current-source inspection.
