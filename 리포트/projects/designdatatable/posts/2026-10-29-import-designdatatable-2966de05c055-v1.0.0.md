---
title: v2.0.1 Publisher draft preparation
date: 2026-10-26T18:25:38.669Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: v2.0.1 Publisher draft preparation — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/submission-201/Review.md
source_sha256: f0471baf009599d615ea7168034cf6c1e43c6ac78edfb423a160f808d9d92129
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Draft: https://publisher.unity.com/packages/1533746/edit/upload

The user changed the requested stopping point to **before submission**. No Submit
button was clicked and no computer shutdown was requested after that change.

## Verified

- Release version 2.0.1 and changelog saved; existing version history retained.
- Description includes support, feedback and an honest-review request.
- Fifteen new gallery images, one music promo, icon, card and cover uploaded.
  New cover moved to the first gallery position. Five existing images retained.
- 26 current native UI captures inspected. Shared images use English; the dedicated
  Game Data instruction image uses Korean. All nine guides rebuilt and verified
  with zero errors. This does not certify every language or DPI layout.
- English PDF regenerated and all 37 pages rendered for visual review.
- Actual Unity run: 11,003 checks passed; all three scenes executed.
- Final clean import: 23 checks passed with the declared uGUI/2D Sprite dependencies.
- Official Asset Store Tools 11.3.1: 27 pass, two warnings, zero failures. Current
  project uploader also reports two warnings. Warnings concern XLSX ZIP signatures
  and documentation JPEGs; no Unity exemption is claimed.
- Actual uploader export: 468 paths, byte-for-byte source and metadata comparison
  passed. SHA-256 ea965b043f9aa48bd7324d0422936a80e69ad945be3503123e652d14f784ac26.
- Unity uploader displayed Upload: Success, 8.05 MiB. Portal initially showed five
  files, then 295 as processing progressed. Final Portal count was 419 files,
  matching the 419 file payloads in the uploaded archive (468 paths with folders).

## Problems caught and corrected

- Missing ExternalCorpus test inputs were added to the isolated validation setup.
- Two old tests expected star5 to be unsupported. They now explicitly check star5
  support and retention of the malformed-group placeholder; native execution passed.
- PDF visual review caught repeated image mappings despite passing hash checks.
  The original native captures were intact. All 26 destination files were remapped
  directly to inspected originals; capture-file-comparison.json records provenance.
  Initial contact sheets remain historical failed-review evidence, not final output.
- An early native export included a test-only generated asset. Export now uses the
  explicit source inventory and final archive/source comparisons report no differences.
- The uploader's pre-export validation project omitted uGUI and failed preparation.
  The official From Assets Folder workflow validated the real project successfully
  and uploaded only Assets/DesignDataTable. No validation failure was marked passed.

## Limits

Server re-download returned a Unity-generated Google Storage URL error
(InvalidArgument: response-content-disposition) while processing was incomplete.
Another download was requested after the Portal reached 419 files; no downloaded
archive was obtained for a server-side SHA comparison. That round trip remains
unverified. Upload success, server size/file count and local export/source matching
are separately verified; none is presented as a successful server re-download.
No modified signed URLs, authentication bypass, or guessed storage paths were used.
Native Excel round trips, all platforms, every UI path and native-speaker translation
review are not certified by these checks. The draft remains unsubmitted for review.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/submission-201/Review.md)

- 원문 SHA-256: `f0471baf009599d615ea7168034cf6c1e43c6ac78edfb423a160f808d9d92129`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
