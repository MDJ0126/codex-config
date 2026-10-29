# Hover activation fix

The user reported neither thumbnail nor temporary peek appeared.

Observed in Unity: main native outer bounds were (1912,-8)-(3848,1040), whereas GetMainWindowPosition returned client (1920,43), 1920x989. The old strict outer-rectangle comparison failed. Matching now also checks client origin and DWM visible-frame bounds, retaining process and ambiguity guards. The read-only probe confirmed the corrected main handle resolves and the exposed condition becomes true.

A second live blocker was a persistent pressedEntry after domain reload, with no active drag. Transient gesture fields now opt out of reload serialization and OnEnable clears them. A 200ms recovery cancels an abandoned press if native mouse-up was missed. The post-reload probe confirmed pressedEntry is empty. The user manually retried and confirmed successful operation, then asked to finish.

Validation: .NET build against Unity 6000.0.83f1 assemblies passed with zero warnings/errors. 49 hover/geometry and 153 overflow assertions passed; git diff --check passed. Native state inspection was read-only: no mouse/keyboard automation, focus changes, screenshots or window activation were driven by tools. Screenshot recapture remains unperformed under the existing computer-use prohibition. No new native visual documentation claims were made.

Temporary Assets/Editor/DDTHoverReadOnlyProbe.cs, its meta and request marker were removed. Runtime evidence retained here includes only Unity records. Previous test evidence is preserved.

XERJ: used the project MCP index to locate WorkbookMinimizedShelf, then inspected current source and actual Unity runtime state.
