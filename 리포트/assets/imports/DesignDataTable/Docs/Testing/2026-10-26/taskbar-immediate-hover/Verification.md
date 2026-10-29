# Immediate custom thumbnail; card-only peek

The taskbar thumbnail opens immediately and does not expire while the entry or card is hovered. The full-size document only peeks while the card itself is hovered, with no dwell delay. Leaving the card immediately restores native window visibility/input/stacking, while returning to the taskbar entry retains the card. Leaving both dismisses the card after the existing 160ms transit gap. The previous 400ms re-open cooldown was removed. Click-to-restore and close confirmation remain in place.

Validation: build passed, zero warnings/errors. 52 hover assertions cover immediate thumbnail, no peek on entry, immediate peek on card, no expiry at long elapsed times, return-to-tab behavior, cancel, placement, input filtering, ABI and native rectangle matching. 153 existing overflow assertions passed. Nine language guide sections updated; English remains default. git diff --check passed.

Actual UI interaction and screenshot recapture were not performed because the user prohibited computer use. Documentation validation remains failing solely for existing pending captures and changed source hashes; these are not passing UI verification. Historical screenshot/source hashes were preserved. No temporary runtime diagnostic probe was added for this change.

XERJ: project MCP search for WorkbookHoverTiming followed by direct inspection of current sources.
