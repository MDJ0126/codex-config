---
title: Commit and push preflight — 2026-10-27
date: 2026-10-26T17:04:53.465Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Commit and push preflight — 2026-10-27 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/commit-preflight/Verification.md
source_sha256: 74e853e7f160a1644296788a73a69f491a6fe1a4dba62f78a1bdec833d06afbf
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The latest working change gives registered sheets the localized Game Data label and registered/edit help, while ordinary sheets retain Switch to Game Data. Both workbook ribbon callers supply the registration state. Nine generated guide descriptions match that behavior.

Executed in this preflight:

- `dotnet build Tools/Verification/Verification.csproj --no-restore -v minimal`: passed, zero warnings/errors.
- `Verification.dll --ribbon-chrome-only`: 82 successful assertions.
- `Verification.dll --external-ui-contract`: 36 successful assertions.
- `BuildLocalizedDocs.py`: regenerated all nine language guides and entry pages.
- `VerifyLocalizedDocs.py --output Docs/Testing/2026-10-27/commit-preflight/LocalizedDocs.json`: not passed. All 95 reported errors concern existing pending native recaptures or source hashes that require recapture. No screenshot/source hash was rewritten to hide that backlog.
- `git diff --check`: passed.
- Latest marketing delivery: all recorded artifact hashes matched, PNGs decoded, 16 Affinity and 16 layered PSD files were present, and the ScreenToGif archive passed CRC with an 80,000 ms timeline. The final delivery ZIP passed CRC.

The marketing manifest previously included `01-Cover.af~lock~`, an Affinity session lock that disappears when the document closes. That transient entry was removed, all remaining hashes were rechecked, and the local ZIP was regenerated without session locks. Actual editable documents were retained.

No Unity UI interaction or documentation recapture was performed during this Git task. This is a commit/push verification record, not a release-readiness certification. Earlier execution evidence remains historical and is not reported as rerun here.

Git scope: source/docs and targeted checks; historical verification reports/review images; and the latest editable marketing delivery. Raw recordings, intermediate PSDs, duplicate deliveries/ZIPs, raster buffers, generated release archives and private cache-cleanup inventories remain local. The Unity export scope stays under Assets/DesignDataTable.

Historical captured `.txt` output is kept verbatim, including trailing blank lines. The scoped Git whitespace attribute exempts those raw output files; source code and authored documentation retain normal whitespace checks.

XERJ: local status and GameTabKey definition search succeeded; results were checked against current sources. No watcher or index refresh is claimed.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/commit-preflight/Verification.md)

- 원문 SHA-256: `74e853e7f160a1644296788a73a69f491a6fe1a4dba62f78a1bdec833d06afbf`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
