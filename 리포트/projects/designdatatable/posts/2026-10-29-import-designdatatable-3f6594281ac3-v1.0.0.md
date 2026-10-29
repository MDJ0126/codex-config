---
title: Expanded object display and package size — 2026-10-19
date: 2026-10-19T14:30:31.575Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Expanded object display and package size — 2026-10-19 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-19/ExpandedObjectsAndSize.md
source_sha256: 06c09f40634ff2fa38c11f42842f684eed9d0e4ba62ab6a256bd04266f6c917a
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Added read-only previews for area/doughnut/radar and stacked/100%-stacked charts. Positive and negative stacks accumulate independently. Added more polygon/arrow presets, ear-clipping triangulation for concave polygons, and group-child coordinate mapping for nonrotated/nonflipped groups with valid DrawingML transforms. Viewing does not modify originals; safe-save preservation and unsupported markers remain.

Full Google Sheets/Excel object compatibility is NOT complete. Combination and 3D charts, scatter/bubble/special charts, SmartArt, arbitrary custom geometry, rotated/flipped groups, all style/effect combinations and object editing still require implementation and validation. Actual interactive visual verification remains pending. These limits are not reported as supported or passed.

C# suite: 10,691 assertions passed. Native Unity: see expanded-objects/unity-session.jsonl. Current native export selects only original project asset paths and restores the project's registration asset after test scenarios, excluding probe resources and local tooling.

Package comparison script: Tools/Verification/MeasurePackage.py. Starting reference de67f8c is the initial checkout snapshot used for comparison, not a downloaded copy of the store release. Both snapshots are packed with identical Unitypackage-compatible deterministic tar/gzip rules without asset previews. This gives a consistent size delta. A separately created Unity-native package has a different compression/preview layout and is measured separately. Current store page labels the published version 790.5 KB; exact original store archive bytes were not available.

Name change: Unity's official manual documents changing Title in Metadata, and the current publisher documentation describes editing a published package via a new draft and submitting it. No publisher account or listing was changed by this task.
- https://docs.unity3d.com/kr/2022.3/Manual/AssetStorePkgDetails.html
- https://docs.unity.com/en-us/asset-store/publishing/asset-packages/update

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-19/ExpandedObjectsAndSize.md)

- 원문 SHA-256: `06c09f40634ff2fa38c11f42842f684eed9d0e4ba62ab6a256bd04266f6c917a`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
