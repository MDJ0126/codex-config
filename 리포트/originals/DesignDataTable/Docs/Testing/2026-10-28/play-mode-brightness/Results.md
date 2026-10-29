# Spreadsheet Play-mode brightness

- Reproduced native Play-mode GUI.color = (0.8, 0.8, 0.8, 1), GUI.enabled = true.
- White cell pixels before fix: RGB (204, 204, 204).
- WorkbookViewer temporarily sets GUI.color to white only while drawing its contents and restores the previous value in finally.
- White cell pixels after fix in actual Play mode: RGB (255, 255, 255).
- Final Edit and Play captures have identical white background, header and type-row pixel colors at recorded sample points.
- Successful Play capture used WorkbookViewer.cs identical to production source.
- .NET build: 0 errors, 0 warnings. No input or saving policy changes.
- Actual non-presented Unity 6000.0.83f1 HostViews rendered and images visually inspected. No desktop input or preference changes.
- Nine language guides rebuilt and new screenshot manifest checks pass. Full documentation verification retains historical unrelated stale images and pending physical Create-menu capture; no overall pass claimed.
- Unity 6000.0 reference source confirms IMGUIContainer applies playModeTintColor to GUI.color: https://github.com/Unity-Technologies/UnityCsReference/blob/6000.0/Modules/UIElements/Core/IMGUIContainer.cs
- XERJ not running; current source inspected using rg after status check.
