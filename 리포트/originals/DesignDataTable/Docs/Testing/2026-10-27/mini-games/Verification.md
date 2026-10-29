# Five independent game scenes with distinct artwork

Open an `Example - ...` scene under `Assets/DesignDataTable/Sample/Scenes` and press Play. Each starts independently. The hub is optional. Coin Dash uses pixel dungeon sprites, Meteor Dodge illustrated spacecraft, Brick Breaker pastel puzzle art, Road Dash top-down racing assets and Memory Match animal illustrations. The existing Neon Runner scene filename is retained for stable references.

51 selected sprites from five official Kenney CC0 packs are included with original licenses and a file/source/hash manifest. Four backdrops are sample compositions. The models remain separate from rendering and use the real encrypted MiniGameBalance table.

All five new games and the original survival/shop scenes use ArcadeText localization. Its 35 stable Enum IDs run down column A, with nine languages across row 1. Every game has a language picker and AUTO follows the device language. Original UI Text components refresh on LanguageChanged; source data and number formatting are retained.

36 gameplay/sample assertions and 43 localization assertions passed. Five new saved scenes rendered in native Unity Play mode. Both original scenes verified actual live UI Text against all nine source languages. English new-game and Korean/French original-scene captures were visually inspected. Simulation was slowed only to stabilize captures; Memory Match reveals use model input for artwork inspection. The Windows build and native player checks are recorded in Result.json and FinalPlayerBuild.txt.

The long French restart label found during actual player testing now uses measured text width to fit the button. Final captures and build follow that correction. Temporary development probes are archived here and removed from Assets. Existing Build Settings and user workbook changes are preserved. Automatically generated ProjectSettings files are backed up under the ignored verification Temp directory before restoration.

Nine guide editions include current game images and language instructions. Older common editor screenshots still have stale source hashes; LocalizedDocs.json reports those failures honestly. Current game screenshots are not implicated in those failures.
