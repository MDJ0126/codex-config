# Column-based local Enums

- Each column defines one local Enum. Headers are type names; members start on row 2 as Name = integer with optional // descriptions.
- Explicit numbers, names, descriptions and all game-table values preserved in native migration.
- EnumGear and EnumEnemies runtime binaries remained byte-identical.
- Both tables compile using the same shared local Element C# type and load data in Unity.
- Definition navigation selects the relevant column. Inspector and type-picker links retained.
- Add creates a column; member or type removal warns dependent values; Undo restores validity.
- Old sheet-per-type and EnumId-grouped formats remain readable and convertible.
- Global Enum providers unchanged.
- 428 automated assertions passed; .NET build: 0 errors, 0 warnings.
- Final Unity 6000.0.83f1 UI renders visually inspected: table, selected column, Inspector and type picker. English only; no physical keyboard/mouse input.
- Native sandbox Editor sources match production byte for byte.
- XERJ unavailable: server startup refused a locked data directory. Searches continued with rg; no lock deleted.
- Historical screenshot source mismatches and pending physical Create-menu capture remain outside this change; full documentation verification not claimed.
