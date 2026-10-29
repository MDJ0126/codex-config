# First main-window click focus return

Actual read-only runtime trace isolated the delayed restore path. Before fix, at 11:06:40.949Z the card click left pendingCardRestore=1; at 11:06:41.041Z the pointer was over Console while the callback remained pending. At 11:06:41.698Z the pending count became zero and the workbook became foreground. Workbook owner remained zero, topmost=false, and tooltipOwners=0. Thus the delayed callback, rather than tooltip restacking or native ownership, accounted for this trace.

Fix: remove QueueCardRestore and its EditorApplication.delayCall. The cached preview host is hidden, not destroyed, so restoration can complete synchronously in mouse-down. GUIUtility.ExitGUI unwinds the old card input event after activation so it cannot reclaim focus. Existing ownership commit and hover behavior are preserved.

After fix, the trace shows pendingCardRestore=0 throughout restoration and main-window focus changes. The user explicitly confirmed normal behavior. BeforeFix.txt and AfterFix.txt preserve the read-only state evidence; no screenshot/video or keyboard contents were recorded. Other-app identities were reduced to outside-Unity.

Temporary Assets/Editor/DDTFocusReadOnlyProbe.cs, its meta and Temp/DDTFocusReadOnly.request were removed; recording stops immediately when the request is absent. Final build without the probe passed with zero warnings/errors. Existing 76 hover checks passed; the obsolete deferred-restore check was removed. Native UI confirmation was performed manually by the user; no computer-use automation was used. UI screenshot recapture is still pending under the existing prohibition.

An optional additional status update to the previously started Unity 에셋 페이지 작업 session was rejected by automatic approval review due to missing explicit permission for that particular update. No workaround or retry was attempted; it was not delivered. The user was asked whether to authorize it. The original handoff/start had already succeeded in the prior turn.

XERJ: project MCP search for RaiseForPopup; current code and actual runtime trace inspected before selecting the delayed-restore fix.
