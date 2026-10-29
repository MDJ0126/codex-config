# Independent workbook restore and minimize

User reported that opening one workbook exposed all workbooks and minimizing one lowered them all. This is accepted as the observed runtime behavior. The prior transparency experiments were not native-verified and are fully removed in this change. Their causal involvement is not established by runtime instrumentation.

Source-confirmed behavior before this change:
- Restore supplied every logically visible peer HWND to ActivateDocument, which explicitly raised the entire list.
- RaiseForPopup also supplied all visible peers, so opening an associated popup could expose other documents.
- Minimize hid its target without synchronously detaching document owners. Owner detachment otherwise happened on a periodic 250ms update.

Changes:
- Restore activates/raises only its selected HWND, with owner detachment before showing it.
- Popup raising applies to the associated document only.
- Minimize normalizes the ownership of visible workbook windows before hiding its one target. Other entry flags are untouched.
- All SetAlpha/SetLayeredWindowAttributes calls and whole-window fade timers are removed. Card layout, border-only hover and immediate thumbnail retargets remain.
- The custom card clears a leftover layered style from the previous implementation when first shown.

Native per-window behavior remains pending user verification; no computer use or UI automation was performed. Existing data and dirty edit sessions were not saved or closed. Nine translated descriptions were regenerated; documentation errors are limited to pending/stale screenshot recapture requirements. Historical hashes were retained.

Build: zero warnings/errors. Hover suite: 82 assertions passed. Taskbar overflow suite: 153 assertions passed. The selected-only raise-order regression case passed. These checks do not execute native hide/show or establish actual on-screen isolation.
