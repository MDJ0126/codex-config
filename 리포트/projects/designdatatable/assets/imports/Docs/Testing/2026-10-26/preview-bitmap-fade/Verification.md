# Bitmap-only custom-card fade

The requested 120ms fade-in is restored without SetAlpha or SetLayeredWindowAttributes on Unity/document windows. A temporary, ownerless STATIC bitmap overlay is created with WS_EX_LAYERED, WS_EX_NOACTIVATE, WS_EX_TOOLWINDOW and WS_EX_TRANSPARENT. PrintWindow captures only the already prepared custom-card HWND. UpdateLayeredWindow changes SourceConstantAlpha on that separate overlay. Once fully opaque, the real prepared card is shown before the bitmap overlay is disposed. All bitmap/DC/window resources are released on completion, cancellation, retarget or domain reload. Failed overlay preparation falls back to showing the original card.

The normal card design and border-only hover remain. Immediate visible retargets discard the old overlay and show the new original card without starting another fade. No document-window show/minimize/group code was changed by this implementation; the previous selected-only restore/owner-detachment fixes remain.

Validation: build passed with zero warnings/errors. Hover suite passed 85 assertions and taskbar overflow suite passed 153 assertions. Covered fade endpoints/midpoint, Win32 blend ABI, initial dwell, immediate retarget, card-only peek, selected-only raise ordering, restore hit areas and existing taskbar behavior. Source search confirms there are no SetAlpha/SetLayeredWindowAttributes calls in the window implementation.

Actual overlay bitmap capture, composited fade appearance and native per-window behavior remain unverified without user testing. No computer use was performed. No native screenshot was recaptured; historical image/source hashes were preserved. Nine localized descriptions were regenerated; documentation verification found only existing pending/stale screenshot requirements.

Primary API references consulted:
https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-updatelayeredwindow
https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-blendfunction

XERJ located WorkbookHoverPreview; current code and native thumbnail implementation were read before editing.
