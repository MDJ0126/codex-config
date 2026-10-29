# Full document preview fade without movement

The full-size temporary document preview now uses the same separate bitmap-overlay fade at the document's existing bounds, with an explicit zero Y offset at creation and every update. The custom thumbnail card retains its existing 8px rise. No native document alpha/layered style is changed.

The source bitmap uses the latest visible document capture or the saved pre-minimize snapshot when hidden. Newly retained snapshots are bounded to 1920x1200 instead of 320x200 to reduce enlargement artifacts in the full-size preview. Existing smaller snapshots remain valid until refreshed by a subsequent minimize.

The overlay is ordered below the custom card and declines activation/input. The real source is revealed at its unchanged position only after the fade reaches opacity. Clicking during the fade reveals and adopts it immediately. Canceling before reveal disposes only the overlay; canceling after reveal restores original visibility/z-order. The fade uses EditorApplication.update and unsubscribes on cancel/commit/finish/reload. The card live thumbnail is refreshed when the source is revealed.

No computer use was performed. Actual native visual appearance remains pending user confirmation. Nine guide descriptions were regenerated; historical screenshot/source hashes were preserved. Documentation errors are limited to existing pending/stale native screenshots.

XERJ located PeekSession; its actual subclass, visibility, commit and cancel paths were read before editing.

Validation: build passed with zero warnings/errors. 101 hover and 153 taskbar assertions passed, including pre-reveal cancellation vs post-reveal restoration and commit placement behavior. Native fade appearance/position still requires user confirmation.
