# Reverse import verification — 2026-10-19

Implemented `TableImport`: exported XLSX/native CSV, TSV, typed JSON/XML and generated ScriptableObject can populate the current table through Import. Input files are read only; the current table name and registered destination sheet are retained. Data is validated before a single undoable replacement. Saving remains explicit.

JSON/XML require the Design Data Table export schema, not arbitrary third-party layouts. TSV uses a column-name header and infers types; it cannot recover metadata absent from TSV. JSON/XML/TSV/ScriptableObject restore calculated values rather than original formulas. Generated ScriptableObject assets must be inside this project's Assets folder. Imported data is not automatically registered as a new source.

- Full C# regression: **10,247 assertions passed**, compiling all project C# against Unity assemblies.
- Real isolated Unity: **130 checks passed**, including generated ScriptableObject binary roundtrip for all nine supported types and three sample scenes in Play Mode.
- JSON/XML roundtrips preserve typed binary values and column order, including Int64.MaxValue and UTC DateTime.
- TSV preserves quoted multiline Unicode text and exact Int64 values.
- Invalid schemas, duplicate JSON keys, trailing JSON and XML external entities are rejected.
- Actual menu/file-picker mouse operation remains unverified. Native validation used Unity APIs in an isolated project; the open original editor was not controlled.
- Evidence: `data-import/unity-session.jsonl`.
