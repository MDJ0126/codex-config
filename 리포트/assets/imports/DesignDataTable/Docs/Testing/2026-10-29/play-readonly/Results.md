# Play-mode game workbook protection

- Game workbooks (including Enum/Summary dependencies) and global Enum providers are read only during Play and transitions.
- Viewer mutation, direct apply, queued popup callbacks, save, Save As, recovery and backend game save are guarded.
- Previously entered cell/formula drafts are retained without committing.
- Serialized game-data classification persists while the document is temporarily absent during restore, preventing the fallback general-green classification.
- Native checks: existing serialized window and newly opened window lock during Play; ordinary spreadsheet still edits/saves; game editing returns after Play.
- Native capture shows bright table colors and a read-only message; no global GUI disable overlay.
- Final production UI/guard sources match the capture project byte for byte.
- Native physical keyboard input was not injected; callbacks and backend writes were directly exercised.
- Nine-language guides rebuilt. New capture passes manifest checks; historical stale screenshots and pending Create-menu capture remain separate unverified documentation items.
- XERJ status reported server not running; rg used for current source inspection.
