# Immediate thumbnail switching

When a custom thumbnail is already visible, moving to another exposed workbook taskbar entry now replaces its content in the same native host immediately. The transition does not hide and reopen the host. An inactive cached host does not bypass the initial 400ms delay. Leaving both card and entries through the existing grace period resets the session. Full document peek continues to require 700ms over the new card; it does not start merely from a taskbar switch.

Validation: .NET build passed with zero warnings/errors. All 81 taskbar hover assertions passed, including immediate repeated retargets, fresh-session delay, leave/reset, separate full-document dwell and existing restoration checks. No native interaction or visual flicker claim is established by these checks. No computer use was performed.

Nine localized guide descriptions were regenerated. Documentation verification found no errors beyond existing pending/stale screenshot capture requirements. Native capture remains pending under the user's computer-use prohibition. Historical screenshot/source hashes were preserved.

XERJ MCP located WorkbookHoverTiming; the current implementation and its shelf caller were read before editing.
