# Rise with thumbnail fade

New preview cards now rise from 8 logical pixels below the final position while fading over the existing 120ms duration. Both alpha and Y position use the same smoothstep progress and the same UpdateLayeredWindow call. The DPI-scaled overlay offset reaches exactly zero before the real card is exposed at its existing final bounds. No document HWND or document group is moved. Existing visible-card retargets remain immediate and do not replay the entrance.

Validation: build passed with zero warnings/errors. 98 hover and 153 taskbar assertions passed, including start/midpoint/end position, no overshoot, fade, right-click menu suppression and independent document raise ordering. All nine guide descriptions were regenerated. Documentation checks report only pending/stale native screenshot captures. Actual native visual appearance remains pending user verification; no computer use or screen capture was performed and historical hashes were preserved.

XERJ located WorkbookPreviewAnimation; the actual overlay update path was inspected before editing.
