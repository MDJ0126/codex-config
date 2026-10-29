# Restore preview card chrome

User observation: after the previous fade change, the thumbnail remained visible while the custom card background and title disappeared. The source still contained the title/background draw calls. The preceding change had applied Win32 layered alpha directly and retained WS_EX_LAYERED after completion. Exact native compositor behavior was not instrumented, so the complete rendering cause is not runtime-verified.

Changes:
- Restore the original 1px card inset, original background color, title/icon/close layout and accent strip.
- Hover changes only the outer border and a thin image outline; no background recoloring.
- Remove direct SetLayeredWindowAttributes calls.
- Use Unity ContainerWindow.SetAlpha(float), verified present in installed Unity 6000.0.83f1 by reflection and in official Unity 6000.0 reference source.
- At fade completion/failure, request full host opacity, clear residual WS_EX_LAYERED on the custom card, and immediately repaint it. Document window opacity is untouched.
- Keep the 120ms fade and immediate retarget behavior.

Validation: build succeeded with zero warnings/errors; all 86 hover assertions passed, including installed host alpha API availability and prior hover/click/timing checks. Actual visual restoration, hover appearance and fade remain pending user verification. No computer use or screenshot capture was performed. Nine guide descriptions were regenerated. Documentation checker errors are limited to existing pending/stale native screenshot captures; historical hashes were preserved.

Official reference:
https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Editor/Mono/ContainerWindow.bindings.cs

XERJ located PreviewOpacity; its current source and the complete preview card implementation were read before editing.
