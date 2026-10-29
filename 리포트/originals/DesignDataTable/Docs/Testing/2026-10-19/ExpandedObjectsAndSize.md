# Expanded object display and package size — 2026-10-19

Added read-only previews for area/doughnut/radar and stacked/100%-stacked charts. Positive and negative stacks accumulate independently. Added more polygon/arrow presets, ear-clipping triangulation for concave polygons, and group-child coordinate mapping for nonrotated/nonflipped groups with valid DrawingML transforms. Viewing does not modify originals; safe-save preservation and unsupported markers remain.

Full Google Sheets/Excel object compatibility is NOT complete. Combination and 3D charts, scatter/bubble/special charts, SmartArt, arbitrary custom geometry, rotated/flipped groups, all style/effect combinations and object editing still require implementation and validation. Actual interactive visual verification remains pending. These limits are not reported as supported or passed.

C# suite: 10,691 assertions passed. Native Unity: see expanded-objects/unity-session.jsonl. Current native export selects only original project asset paths and restores the project's registration asset after test scenarios, excluding probe resources and local tooling.

Package comparison script: Tools/Verification/MeasurePackage.py. Starting reference de67f8c is the initial checkout snapshot used for comparison, not a downloaded copy of the store release. Both snapshots are packed with identical Unitypackage-compatible deterministic tar/gzip rules without asset previews. This gives a consistent size delta. A separately created Unity-native package has a different compression/preview layout and is measured separately. Current store page labels the published version 790.5 KB; exact original store archive bytes were not available.

Name change: Unity's official manual documents changing Title in Metadata, and the current publisher documentation describes editing a published package via a new draft and submitting it. No publisher account or listing was changed by this task.
- https://docs.unity3d.com/kr/2022.3/Manual/AssetStorePkgDetails.html
- https://docs.unity.com/en-us/asset-store/publishing/asset-packages/update
