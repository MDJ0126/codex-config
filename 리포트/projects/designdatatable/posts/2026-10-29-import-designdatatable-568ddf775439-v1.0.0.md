---
title: Omitted shape outline fix
date: 2026-10-26T11:37:10.172Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 버그
category: 이전 기록
summary: Omitted shape outline fix — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-26/gradient-outline-fix/Verification.md
source_sha256: 79e941166558e4e07999231d37ff33aa133a6dec3efecd9df355771c20041e9a
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

WorkbookDrawings now marks a shape as NoLine when both its local a:ln and its xdr:style/a:lnRef are absent. Explicit outlines, explicit noFill and the existing inherited-style behavior are retained. Source XLSX packages were not edited.

Build: zero warnings/errors. Production reader/edit/save/reopen suite: 713 assertions passed, including all 13 borderless gradient shapes and retained authored catalog outlines. Six-file package audit passed. Production raster preview was regenerated and visually inspected: the gradient gallery is borderless. Native Unity UI and screenshot recapture were not performed under the existing computer-use prohibition. All nine guide descriptions were regenerated; documentation checks report only pending/stale native screenshot requirements. Historical screenshot/source hashes remain unchanged.

The inherited line-style resolver was not expanded by this focused fix.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-26/gradient-outline-fix/Verification.md)

- 원문 SHA-256: `79e941166558e4e07999231d37ff33aa133a6dec3efecd9df355771c20041e9a`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
