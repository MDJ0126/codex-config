# Creation menus and cell input

- Build: zero warnings/errors. Source creation/persistence checks: 10 passed.
- Native Unity 6000.0.83f1 English menu inspected: three format choices; Standard Excel created a blank worksheet.
- Real Unity EditorWindow.SendEvent key sequence and IMGUI repaint processing: a → ab → abc → abcd, Enter advances one row; x → xy, Tab advances one column. Saved XLSX independently inspected: A2=abcd, A3=xy.
- Before correction the same probe produced bcd, then suppressed the next cell input. Explicit TextEditor.OnFocus/caret initialization and clearing editingTextField after commit fixed the recorded paths.
- Hardware keyboard delivery and IME composition were not tested. Computer Use typing activated the main Unity window, so it was not treated as a valid document input test.
- Test windows closed; temporary editor probe removed. User's unsaved Localized.xlsx preserved.
- Nine-language creation/input instructions rebuilt; pre-existing stale documentation captures are reported by VerifyLocalizedDocs, not marked current.
- XERJ MCP searched successfully, but its result did not identify the needed file; current source verified with rg. Local launcher status/recovery could not connect from sandbox.
