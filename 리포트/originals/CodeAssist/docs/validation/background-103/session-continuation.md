# Ongoing verification — do not stop at this checkpoint
Latest user: verify each feature, unlimited time; do not take user screen. Task started 2026-10-18 22:57:27 KST.

Current hosts: VA 40564, CA 28752, private desktop CodeAssistVerify-runtime (WinSta0). Never SwitchDesktop, SendInput, SetForegroundWindow or touch user clipboard. Escalated process reads required; non-escalated Get-Process falsely cannot see these PIDs.
Current LiveProbe: CodeAssist.LiveProbe.030.dll; background_client.py points030. Worker BackgroundOptionsV17.exe, background_steps.py pointsV17.

Critical new automation fixes:
- background_steps.setup now resolves actual RDT-backed view via module focus-target-info, focuses its own private WPF source HWND with worker focus-hwnd (AttachThreadInput+SetActiveWindow/SetFocus only same desktop), then focus-document-view. Requires HasAggregateFocus true, actual view file/sample/caret equals input. This fixed the earlier wrong/no-focus cases. focus_existing() added for returning to open buffers without replacing text.
- posted-text sends WM_CHAR to actual focused private WPF HWND, checks PID/file/focus; no global keyboard. Native VA Repair Case is not exercised by direct IOle TYPECHAR, but DOES work with this posted path. Physical keys still uncertified.
- arm-owner-focus-restore attaches only owned preview Window.Closed, restores private owner HWND+WPF view focus. Without this, inactive-desktop close left Keyboard.FocusedElement null and SDK GetActiveView returned previous header even though DTE ActiveDocument was CPP. With this, actual CA cross-file moves succeed. Do not call those old failures general product bugs.
- VA old capture service died. Restarted own VA only. Locked original test DLL left untouched. New distinct assembly CodeAssist.ParityRunner.Watchdog.dll built and registered Assembly+CodeBase in ONLY private VA profile yfm04dbq.au2 pkgdef. Original pkgdef/DLL backups in RUN/plugin-backups. Actual loaded service-build MVID 21a53576-f675-4156-8b45-28d6060bd051, VA40564; heartbeat advances. CA prior watchdog still works.
- background_client request atomic replace retries sharing races. module accepts _timeout for long preparation.
- compiler_contracts.build_run now preserves exact bytes, explicitly passed=False on compile failure.
- adjudicate_background.update now archives previous judgment in judgment-history before replacement.

Latest adjudication: 59/108 determinate (10 representative-match,49 different,49 unverified). Report last generated before newestF021/F073; regenerate when ready. F022 replaced with corrected focus-restored result; old judgment archived.
New determinate since previous checkpoint: F010 navigation back/forward both work but definition caret differs; F058/F059 context+definition fields differ; F014 static text vs native combos; F056 selected expression parentheses and Undo exact match; F090 same PDB target read both but native structured/filterable vs CA dump; F089 same RVA resolves PdbTarget103 both, VA module+RVA accepted but CA Invalid RVA; F073 type names both repaired, ordinary variable only VA repaired (Native compile/run4, CA undefined identifier failure); F021 CPP->header both move/run7/Undo, format differences. F022 header->CPP both move/run7/Undo after owner focus restore, formatting differences only.

Scripts: crossfile_move.py to-header|to-source is reusable and includes source prep, compiler actions, preview capture, owner-focus restore, compile/run, Undo, flags byte restore. Native single-line class MoveToSource produced invalid Native output; valid multiline counterpart worked. Use multiline fixtures.

Current in progress: background_related_menu.py session54009, on private hosts. Prepared related-symbols.cpp with RelBase103 and RelDerived103. CA index-reinitialize completed via public API temp owned root/back, saved owned project only; index-state confirms RelDerived103 type, correct root, no build/error. Script opens Native VAssistX.GotoRelated and CA263, collects menu-tree; may leave both menus/picker open for actual selection. Need poll session, inspect relevant rows, select same base class, observe destination, do not count menu opening as feature pass.

Other pending immediate rechecks: background_info_focus.py prepared clone using new setup and NEW output prefixes focus-quick-info/focus-parameter-info, not run yet. Avoid overwriting judged older files. F065/F067 may change with real focus; rerun and archive/update judgment.

Clipboard plan NOT EXECUTED: WinSta0 private desktop still shares user clipboard, so never paste/copy global. Microsoft docs confirm each window station has separate clipboard: https://learn.microsoft.com/en-us/windows/win32/winstation/window-stations . Could create separate noninteractive window station + desktop and smoke-test own clipboard before moving owned VS hosts there, but current guards/launcher assume WinSta0 and would require deliberate changes. No clipboard isolation claimed yet.

All settings transactions restored (repair-case-posted, repair-case-posted-undo, repair-variable-posted; column; flags cross-move). Current Related menus only. PDB/Address Resolver own tool windows remain, containing own debug-artifacts/PdbAudit103 EXE/PDB. Native PDB loading is ASYNC (Refresh label becomes Cancel), wait for loaded rows rather than declaring missing. Native WPF AutomationId often derives FrameworkElement.Name, not attached AutomationProperties; module Id() now handles both. UIA whole Native Browse window could hang; internal window-read avoids expanding trees. Do not use options-read on arbitrary file tree windows (it expands TreeView items).

No product source/installer version changed: still0.0.94. All mutations testtools/evidence/private test profiles. No git push requested. XERJ really used (ExpectedType, MoveSelection etc) and source-validated; REA MCP absent, do not claim REA tool invoked.

All final reports must include actual elapsed+finishKST and initial proposed first10min milestone/overall unlimited, XERJ use line. Continue remaining work, do not end merely with a checkpoint.

Checkpoint at 2026-10-19T03:55:32.778061+09:00