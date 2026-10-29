# Store readiness audit

The published listing was read in the browser: DesignDataTable 1.0.1, released 2024-09-17. A local copy of that distributed package was not found in the Unity Asset Store cache. Git commit e8475bb is a comparison aid only, not an asserted copy of 1.0.1.

The 10:34 candidate was compared again with the current working tree using VerifyRelease.py. It failed with 42 byte-different archived source files and 17 reported errors, including missing additional assets. See release-verification.json for exact paths. Earlier 27-pass / 2-warning / 0-fail official validator results belong to the earlier candidate snapshot.

ScreenshotManifest.json was checked read-only: 12 images inspected by hash, 48 source/image association discrepancies. These are source hash discrepancies requiring recapture review, not 48 distinct images. No manifest hash was changed and no new actual UI capture was claimed.

Store artwork drafts are outside the Unity package under MarketImages/StoreRefresh. Four SVG sources and dimension-checked PNG review renders were prepared; cover, card and icon were visually inspected. Affinity was absent from the returned installed-app inventory. Affinity editing/export acceptance remains pending installation and first login by the user.

Current source readiness is not accepted. Next release gates: recapture affected UI and update nine-language documentation, rebuild current candidate, clean import/sample execution/official validator checks, review supported-platform claims and publisher disclosures. Native Excel, macOS, IL2CPP and full UI acceptance remain as documented in DevelopmentStatus.md.

XERJ MCP map succeeded, but WorkbookDocument retrieval returned no hits. Local search failed because the node data directory was locked and local endpoint unavailable. Current files were checked using rg; no active background watcher is claimed.
