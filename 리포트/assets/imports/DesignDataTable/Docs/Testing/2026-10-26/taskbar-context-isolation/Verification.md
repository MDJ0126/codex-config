# Taskbar context menu isolation

User observation: right-clicking a minimized entry opened its document, the menu and a thumbnail at the same time.

Source findings:
- ShowContext supplied the minimized document as WorkbookPopup.Show's owner.
- That generic path attached and raised the document when the popup opened.
- Hover polling had no menu-open suppression, allowing a new thumbnail/peek while the menu was active.

Changes:
- Taskbar menus take a separate unowned popup route; they do not call Attach or RaiseForPopup for their minimized document. Automatic inherited-owner assignment explicitly excludes active taskbar menus.
- The menu tracks its active native EditorWindow lifecycle.
- Opening context blocks hover and cancels the current card/fade/peek. While a taskbar menu is open, UpdateHover cancels and returns before input/peek processing.
- After dismissal, the hover gate requires the pointer to leave the taskbar entry before a fresh preview session can begin.
- Restore/maximize/close actions remain explicit menu commands. Ordinary tool popups retain their document ownership.

Actual right-click/native-window interaction remains pending user verification; no computer use was performed. Existing workbook data and dirty edits were not saved or closed by tools. All nine guide descriptions were regenerated; historical screenshot/source hashes were preserved, and native screenshot verification remains pending.

XERJ located WorkbookTaskbarMenu; current menu, shelf and popup ownership implementations were read before editing.

Validation: build succeeded with zero warnings/errors. 95 hover and 153 taskbar assertions passed, including taskbar-vs-ordinary-popup ownership routing and menu/open/dismiss/leave/re-entry suppression. Documentation verification found no issues beyond pending/stale native screenshot requirements. Actual native interaction remains unverified.
