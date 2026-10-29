---
title: Store readiness audit
date: 2026-10-22T05:37:10.995Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 검토
category: 이전 기록
summary: Store readiness audit — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-22/store-refresh-audit/Readme.md
source_sha256: 55c36e9a135a4fcf5011a7d6f1ab3ba223678e42992930136a0619b25e37258a
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

The published listing was read in the browser: DesignDataTable 1.0.1, released 2024-09-17. A local copy of that distributed package was not found in the Unity Asset Store cache. Git commit e8475bb is a comparison aid only, not an asserted copy of 1.0.1.

The 10:34 candidate was compared again with the current working tree using VerifyRelease.py. It failed with 42 byte-different archived source files and 17 reported errors, including missing additional assets. See release-verification.json for exact paths. Earlier 27-pass / 2-warning / 0-fail official validator results belong to the earlier candidate snapshot.

ScreenshotManifest.json was checked read-only: 12 images inspected by hash, 48 source/image association discrepancies. These are source hash discrepancies requiring recapture review, not 48 distinct images. No manifest hash was changed and no new actual UI capture was claimed.

Store artwork drafts are outside the Unity package under MarketImages/StoreRefresh. Four SVG sources and dimension-checked PNG review renders were prepared; cover, card and icon were visually inspected. Affinity was absent from the returned installed-app inventory. Affinity editing/export acceptance remains pending installation and first login by the user.

Current source readiness is not accepted. Next release gates: recapture affected UI and update nine-language documentation, rebuild current candidate, clean import/sample execution/official validator checks, review supported-platform claims and publisher disclosures. Native Excel, macOS, IL2CPP and full UI acceptance remain as documented in DevelopmentStatus.md.

XERJ MCP map succeeded, but WorkbookDocument retrieval returned no hits. Local search failed because the node data directory was locked and local endpoint unavailable. Current files were checked using rg; no active background watcher is claimed.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-22/store-refresh-audit/Readme.md)

- 원문 SHA-256: `55c36e9a135a4fcf5011a7d6f1ab3ba223678e42992930136a0619b25e37258a`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
