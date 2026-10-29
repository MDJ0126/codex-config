# Windows-style hover timing and transition stability

The user reported visible flicker when the card/peek appeared and requested a Windows-style delay.

Implemented behavior: 400ms taskbar dwell before the custom card, then 700ms dwell on the card alone before full-size peek. Neither expires while hovered. 120ms peek-exit grace prevents repeated show/hide at card boundaries; the card allows a 250ms transit gap. A pending show deadline cannot open a card after the pointer has already left both regions.

Concrete source findings and changes:
- RefreshLive previously unregistered and recreated the DWM thumbnail on both peek transitions. It now preserves the relationship, changing only visibility. Hidden sources register the same lease with fVisible=false; their existing snapshot remains behind it.
- Peek previously changed WS_EX_NOACTIVATE on entry and restored the style on exit. The subclass hit-test/mouse-activation guard and SWP_NOACTIVATE already provide non-activation/input protection, so the style mutations were removed.
- The new card is synchronously painted after native creation/stack setup. A hidden document is painted before and immediately after showing rather than relying on a later queued repaint. Card styles are updated only when different.
- Thumbnail registration failures retry a bounded eight times at 100ms intervals, rather than permanently keeping the initial placeholder after a handle-not-ready frame.

Validation: build against installed Unity 6000.0.83f1 assemblies passed with zero warnings/errors. Hover policy, placement, ABI, native rectangle and input-filter checks: 54 assertions. Existing overflow regression: 153 assertions. git diff --check passed. Nine translated hover guide sections updated; English remains default.

Visual limitation: these source paths can create extra transitions/placeholder frames, but source inspection and headless checks do not establish the complete pixel-level cause or prove that visible flicker is eliminated. No computer-use tools, native window manipulation tests, focus changes or screenshots were executed. The user has an optional manual retry question pending. Actual UI capture and all-language visual verification remain unperformed under the existing computer-use prohibition; original screenshot/source hashes were preserved and documentation verification remains failing for pending/stale captures. No runtime probe was installed.

API evidence: Unity 6000.0 reference source shows popup creation brings a window live before subsequent reflow: https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Editor/Mono/ContainerWindow.cs . Windows thumbnail visibility can be updated on the existing relationship: https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ns-dwmapi-dwm_thumbnail_properties . Non-activating positioning: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos .

XERJ: project MCP search for RefreshLive, validated against current source. Historical test evidence was preserved.
