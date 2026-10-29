# Game Data / Settings ribbon change

- Settings now occupies the left tab slot after Help, formerly occupied by Game Data. It opens preferences without selecting another tool tab.
- Game Data is fixed at the far right with an orange background, white bold label and a selected indicator. Clicking it selects the existing Game Data tools (index 8), preserving command behavior and external-file registration restrictions.
- Its custom screen tip and selected panel use the localized instruction. Korean is exactly `현재 시트를 게임 데이터로 사용합니다` without extra punctuation. The Korean running-Unity capture verifies both occurrences. English is the default; translations are included for all nine supported languages.

Actual Unity 6000.0.83f1 UI was inspected in English and for the dedicated Korean instruction. Real clicks verified the relocated Settings button, preferences navigation and right-hand Game Data tab. No registration command, data edit or save/discard action was performed. Existing unsaved test.xlsx remained open. Verification fixtures were closed only when clean, temporary Assets probe removed, main Unity placement restored and language returned to English.

The 20 previously documented full-window images were recaptured with the new ribbon, plus one dedicated Korean instruction image. Capture-time source hashes are stored next to each PNG; manifest hashes and JPEGs were produced only after visual inspection. Previous guide images and manifest remain in `previous-docs/`; prior task PNGs remain under `../shape-catalog/`. Other language layouts were not visually certified. Auxiliary tools were manually positioned for guide captures.

BuildLocalizedDocs.py and VerifyLocalizedDocs.py completed for nine languages with no errors. The final build after removal of the temporary probe completed with zero warnings and zero errors. `git diff --check` passed. The existing external source / custom tooltip / localization regression command passed 36 assertions.

XERJ MCP index `designdatatable-code-v3-docs` located ribbon and localization code, checked against current files. Local Xerj.ps1 Status reported its separate node was not running; the functioning MCP index was used instead. No background watcher activation is claimed.
