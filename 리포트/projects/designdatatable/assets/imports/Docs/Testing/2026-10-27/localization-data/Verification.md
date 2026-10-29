# Localization Data

Separate localization workbook registration, generated runtime assets and typed Enum API implemented. Row 1 B1 onward contains Enum names, column A contains language names, body cells contain translations. The included sample contains nine language rows.

Build passed with zero warnings/errors. Headless checks: 42 localization, 10 source creation and 82 ribbon assertions passed. Native Unity 6000.0.83f1 checks passed for Application.systemLanguage lookup, explicit language overrides, fallback, change events, generated Enum facade compilation, Resources.Load, automatic source-reimport rebuild, and restored sample values. See native logs and the preserved test probe sources. Editor probes were removed after execution.

The final English UI and four creation-menu entries were captured and inspected. Nine-language guides include the shared English capture and translated workflow instructions. Existing unrelated/shared captures have source-hash staleness after common editor changes; VerifyLocalizedDocs reports these as incomplete rather than updating hashes without recapture. No player build was run.

User workbooks and pre-existing edits were preserved. No Git commit or push was requested for this feature.
