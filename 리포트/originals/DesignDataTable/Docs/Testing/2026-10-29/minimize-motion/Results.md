# Minimize motion

A captured workbook surface shrinks/moves to the cached real taskbar-entry anchor over 0.24 s, fading during the final 35%. Overflow entries use the overflow-control anchor. Only the source document is hidden; original placement and edit state are retained. Restore cancels the surface before normal existing restoration. Hover previews wait until the flight ends. Compile, close and shutdown clean up the bitmap surface. Missing snapshot/target or unsupported native rendering falls back to the existing immediate minimize.

54 native checks passed: initial/final bounds, monotonic size, opacity, actual UpdateLayeredWindow geometry, bitmap readbacks at five time points, no black frames, cancellation, GDI cleanup, unchanged source window geometry/styles and unchanged foreground. No visible window was presented to the user's desktop. The actual desktop minimize-button-to-taskbar and restoration/focus gestures were not exercised; these remain a limitation of this screenless verification. Existing taskbar/hover regressions were run separately.

The guide image is an actual native renderer frame using a workbook screenshot; offscreen test geometry is synthetic. It is not a full desktop animation recording. Nine language guide text records this distinction. Historical stale screenshots remain outside this task; no overall documentation verification pass is claimed.

XERJ server was not running; rg was used after the status check.

Regression results: 101 taskbar-hover assertions and 153 overflow/list assertions passed. These headless regressions do not claim physical desktop interaction.
