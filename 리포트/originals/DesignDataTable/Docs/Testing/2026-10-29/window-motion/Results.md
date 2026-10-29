# Damped minimize, restore and maximize motion

Minimize: 320 ms critically damped normalized easing, with late fade. Restore from the taskbar, maximize and return to normal size: 300 ms damped movement of an opaque captured surface. Live editor layout changes only once. An already revealed hover peek commits directly. All native activation/style ownership protections remain in place; sibling document owners are detached before hiding a maximized document. Hover scans and visibility repair wait for the animated document. New clicks or foreground changes permanently cancel deferred focus. Reload/quitting cancellation shows the real document without activation and releases surfaces. Surface capacity uses the current monitor including non-client margins.

Final build passed, 0 warnings/errors. 86 actual Windows/Unity native checks passed: shrink geometry, bitmap readbacks/no black frames, unclipped expansion, controller cancellation, no-focus reveal, source geometry/style retention and GDI cleanup. 151 hover and 153 overflow regressions passed.

Screenless verification limit: no physical desktop pointer/keyboard input or maximize/restore/minimize button gesture was injected. The actual live ContainerWindow toggle path and perceived smoothness therefore remain unverified. The guide frame is the actual Windows renderer with a previously captured workbook UI and synthetic offscreen geometry; it is not a full desktop motion recording.

Nine language guides were regenerated. Historical unrelated screenshot staleness is preserved; overall documentation verification is not claimed to pass. XERJ node unavailable, rg used.
