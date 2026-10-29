# Custom preview hover and fade

The custom thumbnail card now interpolates its background, outer border and image outline on pointer hover (90ms). Native pointer state is supplied by the existing shelf poll, so the visual does not depend on Unity being foreground. The close button keeps its red hover cue and existing hit test.

A newly displayed card uses a 120ms smoothstep alpha fade. Only the custom card HWND is passed to PreviewOpacity; document peek/restore HWNDs are not changed. Windows SetLayeredWindowAttributes with LWA_ALPHA provides whole-host alpha. The style bit is retained rather than toggled every frame. Alpha is prepared while hidden before the initial show. A visible reused host retains its animation state, so immediate retargeting does not fade out/restart. A hidden reused host starts a fresh fade. If native alpha setup fails, the code requests full opacity rather than leaving a hidden interactive card.

Build passed with zero warnings/errors. 85 hover checks passed, covering fade endpoints/intermediate stages, existing timing/retarget behavior, click restoration, placement and native layout contracts. These are headless checks; actual native transparency, compositor/DWM interaction and hover appearance remain unverified. No computer use was performed. The existing screenshot recapture backlog remains open; historical source/image hashes were preserved. All nine language descriptions were regenerated; documentation verification reported only pending/stale screenshot requirements.

Native API reference consulted:
https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setlayeredwindowattributes

XERJ located WorkbookHoverPreview; current source and native preview implementation were inspected before editing.
