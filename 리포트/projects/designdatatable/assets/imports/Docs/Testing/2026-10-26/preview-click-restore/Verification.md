# Commit restoration when clicking a preview card

The user reported that clicking the custom card while full-size peek was shown did not keep the document restored.

Source findings: the native pointer path handled restore only over the taskbar entry. Card IMGUI restore handled the thumbnail image only; title and margins had no restore action. In addition, temporary ownership was not relinquished until after Commit returned, leaving cleanup order dependent on nested native callbacks.

Fix: hit-test the entire card except the close button. Handle an exposed native card click on mouse-down before focus-loss/hover cleanup, with a whole-card IMGUI fallback. The GUI fallback commits the native peek immediately, deferring only document activation/host closure until IMGUI unwinds. A shared lease owner detaches before Commit/Dispose, preventing reentrant cleanup or later mouse leave from hiding a committed document. Native PeekSession's existing commit removes input interception and retains visibility. Close-button behavior remains the normal document-close/save-confirmation path. ESC still cancels.

Validation: final .NET build against Unity 6000.0.83f1 libraries passed with zero warnings/errors. 76 hover checks cover title/image/margins, close exclusion, outside clicks, negative-coordinate monitors, reentrant commit/cancel and later cancellation after commit, alongside existing timing/geometry/ABI checks. Existing overflow: 153 checks; status layout: 92; total 321. git diff --check passed. Nine localized guides updated.

Limits: current native click behavior has not been manually confirmed. A retry question is pending. No computer-use automation, screen capture, native click injection or window activation was executed by tools. Documentation verification remains failing for pending/stale captures, with historical screenshot/source hashes preserved. Historical read-only runtime evidence from taskbar-hover-fix confirmed the native card was the pointer root while hovered; this supports the exposed-card guard, but is not a test of this new click fix.

Handoff: the user explicitly requested starting the agreed work in the existing Unity 에셋 페이지 작업 session. A message was sent to thread 01a17245-169b-7bc0-92bb-1576f0d86493, preserving the computer-use prohibition and warning that actual click restoration must be checked before sales filming. wait_threads returned active/inProgress, and the destination assistant acknowledged starting environment/material inspection and capture planning. No new thread or subagent was created, and no navigation/focus change was requested.

XERJ: project MCP search for WorkbookHoverPreview, followed by current-source and historical runtime-record inspection.
