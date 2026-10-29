> Historical record. Current minimal listing source: `Docs/Store/ListingSource.json`. Current still images: `Docs/StoreMedia/Delivery-2026-10-29`. This file does not establish current portal or package approval.

# Asset Store submission audit - 2026-10-20

Status: **release candidate, not yet verified ready for submission**.
The archive is a local candidate; nothing has been uploaded or submitted.

## Completed

- English is the default for new workspace settings. Existing saved language
  preferences and the explicit System option remain available.
- Nine offline HTML guides, each with the same 14 chapters, examples and limits:
  en, ko, ja, zh-Hans, zh-Hant, es, fr, de, pt-BR.
- English PDF companion and retained Korean development guide.
- Single Assets/DesignDataTable root, including local documentation and notices.
- Supplied Kenney CC0 license files retained; a top-level Third-Party Notices.txt
  lists the sample artwork and fonts identified by those notices.
- Replaced three deprecated FindObjectOfType calls with FindFirstObjectByType.
- Current Asset Store Tools 11.3.1 run: 29 checks, 28 Pass, 1 Warning, 0 Fail.
  The remaining warning flags the four XLSX sources by ZIP signature; see below.
- Latest source snapshot: 26,494 C# assertions, 7,816 native Unity checks,
  three workspace scenes and 23 clean-import checks passed. See
  Testing/2026-10-20/Goal100LookupReferenceRanges.json and lookup-reference-package/release-verification.json
  under the same testing directory. These checks do not replace the official
  validator or interactive acceptance.
- No source asset references outside the package root found by the serialized
  GUID audit. Seven package references are checked against installed uGUI metas.
- Asset paths are below the 150-character limit in the inventory.

## Required before submission

1. Complete the manual spreadsheet UI matrix. The latest read-only desktop check
   found the Unity workspace obscured by a Windows Security network permission
   request for UnrealPak. No input was attempted on that permission request.
   No successful drag, clipboard or shortcut tests are claimed from these attempts.
2. Capture unobscured feature screenshots in the English editor. Existing scene
   RenderTexture images are genuine game renders, not spreadsheet UI screenshots.
3. Review the remaining official archive warning. The local 11.3.1 validator was
   executed on the current candidate: 28 Pass, 1 Warning, 0 Fail. See
   Testing/2026-10-20/lookup-reference-official/summary.json. Future source changes require
   another validator run. Its compressed-file
   check detects the ZIP signatures of the four sample XLSX sources and has no
   XLSX exemption. A separate mixed-line-ending warning was fixed and cleared.
   The source workbooks were retained and the warning is documented in the
   distributed Verification.md. Unity has not granted an exemption or approval.
4. Review the listing's supported Unity versions, platforms, dependencies,
   limitations, screenshot dimensions, support email and website in Publisher Portal.
   Only Windows Unity 6000.0.83f1 execution is verified here.
5. Review translations as needed with native speakers. Structural/technical
   checks are completed; independent native-speaker editing is not claimed.
6. Confirm redistribution rights for any sample resource not covered by the
   retained Kenney notices. No missing rights are inferred, and ownership has not
   been certified by this code audit.
7. Review AI-assisted content disclosures in the Publisher Portal. Current
   guidelines require disclosure for functional content generated with AI.

## Publisher facts to use when editing the listing

- Typed native XLSX/CSV game-data editor; XLSX is the default new source format.
- Generated C# and binary runtime data; source files can share one folder.
- Separate registered game-data tables and editable ordinary XLSX/CSV workbooks.
- Formula subsets, relative/absolute and cross-sheet references, range and sparse clipboard,
  fill, row/column sizing, visibility, filters, frozen panes and validation.
- General workbooks support documented formulas, dynamic arrays, merged-cell
  editing, formatting, drafts, backups and bounded Undo/Redo recovery. Native CSV
  formulas calculate again on reopening; CRLF/LF/CR cell text is preserved.
- General XLSX supports basic chart preview and editing (bar, line, pie, area,
  doughnut, radar), PNG/JPEG images and basic shapes. Supported series ranges,
  colors and opacity can be edited. These are documented subsets, not full Excel
  chart, drawing or workbook compatibility. See the distributed English guide.
- Imported XY scatter charts have a basic numeric-spacing preview; scatter series names and numeric X/Y ranges can be edited; marker-style scatter insertion supports up to 200 rows and 12 Y series; visible line/marker solid RGB and opacity editing is supported; advanced style rendering remains incomplete.
- XLS, macros, encrypted workbooks, complex/3D/combined charts and editing grouped
  child objects remain unsupported or restricted. Unsafe edits are blocked and
  supported preservation cases retain unrelated original package parts.
- Samples require com.unity.ugui and com.unity.2d.sprite. The package does not
  silently install or alter packages. The clean import used local installed caches.
- Third-party listing notice: “Asset uses Kenney sample artwork and fonts under
  CC0; see Third-Party Notices.txt in the package for details.”
- AI disclosure draft for publisher review: “OpenAI Codex assisted with editor
  code changes, sheet icons, tests, documentation and translations. The work was integrated
  into the existing product; automated checks and real Unity execution were used.
  Manual UI verification remains pending as documented.”

The publisher should write/review the final marketing description in their own
voice. This audit is not a generated replacement for publisher review or approval.

## Official requirements review (rechecked 2026-10-20)

[Unity Submission Guidelines](https://marketplace.unity.com/publishing/submission-guidelines),
updated May 20, 2026: 1.1.b (package errors/warnings), 1.1.c (dependencies),
1.1.h (approval not guaranteed), 1.2.a (third-party notices), 1.3.a (Unity 2022.3+),
1.6.a/b (AI assistance disclosure), 2.1 (organization/path length), 2.3 (offline
documentation), 2.5.1.e (no unsolicited package changes), 3.1.a (accurate limits).

[Official validation and upload workflow](https://docs.unity.com/en-us/asset-store/publishing/asset-packages/upload).
Final approval is Unity's decision; a local archive or passing tests is not approval.

- Scatter previews now read imported finite manual X/Y bounds and independent axis reversal by ID. Visible segments are clipped; out-of-range markers are omitted. The object panel edits finite bounds/reversal and resets to automatic; invalid edits roll back and unknown extension-bearing bound nodes are preserved by rejecting edits. Imported logarithmic scatter axes are previewed with source bases retained; the object panel edits independent log bases or switches to linear, with original metadata guards. Actual UI acceptance and smooth charts remain incomplete. See Testing/2026-10-20/Goal100LookupArrays.json.

- Fresh 2026-10-20 17:07 KST actual Unity window inspection remains obstructed by the Windows Security UnrealPak permission request. No input was sent. See Testing/2026-10-20/UIRecheck1707.json.

- Local chart source visibility follows plotVisOnly through a separate preview model. Hidden/filter data, empty-visible states, palette identity and raw chart-cache preservation were verified across seven basic types. External/unresolved visibility and actual Excel UI parity remain unverified. See Testing/2026-10-20/Goal100LookupArrays.json.

- General workbook File > Debugger and AI context exports read-only versioned JSON with explicit selected-content opt-in. Existing game/general operation logs and bug-report ZIP remain available. Nine localized guides and the 24-page English PDF were updated. Actual debugger popup interaction and macOS remain unverified.

- Registered game tables now expose current unsaved typed-schema diagnostics in the same themed AI context popup. Validation is bounded and explicitly marks partial results; source strings and numeric blanks are preserved. General workbook diagnostics remain available. Actual UI input is still blocked as confirmed in Testing/2026-10-20/UIRecheckGameAi.json; no permission-window input was sent.

- General XLOOKUP now supports sorted ascending/descending binary exact and next-smaller/larger lookups. Unsorted binary input is not checked; wildcard binary searches remain unsupported. Separate game-data formula behavior is preserved. See Testing/2026-10-20/Goal100LookupArrays.json.

- General XLOOKUP now returns matching rows/columns as dynamic arrays, including nested two-axis lookup, existing spill protections and saved metadata. Scalar formula text preservation passed regression after limiting future-function prefix serialization to dynamic storage. Lookup-value broadcasting and binary wildcard matching remain outside verified support. See Testing/2026-10-20/Goal100LookupArrays.json.

- Same-sheet XLOOKUP/INDEX/absolute-name colon reference ranges are implemented and verified, including preservation of unsupported 3D caches. Actual UI, Excel and macOS acceptance remain incomplete. See Testing/2026-10-20/Goal100LookupReferenceRanges.json.
