# Preview frame continuity

User screenshots showed an image-free temporary card with smaller text and a different position, followed by the full native card with an image. Source confirmed independent GDI/IMGUI title renderers, snapshot-null composition and an overlay position read from the native window rather than the requested placement.

Changes:
- Use the requested card bounds for both overlay creation and final native presentation.
- Read back the complete composed bitmap after title/close text rendering and display those exact bytes in the final card as a Unity texture. Close hover, restore hit testing and edge hover remain interactive.
- Obtain a visible source snapshot if the shelf did not supply one. Never fade an image-free card when no source snapshot is available.
- Retain minimized snapshots as serialized PNG bytes across script reload, restoring their textures on shelf enable. Newly captured snapshots remain CPU-readable for encoding.
- Clean up card-owned snapshots and shared presentation textures on retarget/disable.

Existing minimized entries from before this version have no serialized snapshot bytes. Opening and minimizing one such document once generates the initial snapshot. Subsequent code reloads retain it.

No source workbook was saved or closed by tools. No computer use was performed. Actual visual confirmation of the corrected frame/position remains pending. Screenshot/source hashes were preserved; the native screenshot backlog remains open.

Build passed with zero warnings/errors. 88 hover and 153 taskbar assertions passed, including offscreen GDI pixel round-trip and selected-only raise ordering. Documentation checks found no issues beyond pending/stale native screenshot requirements. Native visual confirmation remains pending.
