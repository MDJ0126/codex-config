---
title: Localization Data
date: 2026-10-27T13:04:58.662Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Localization Data — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-27/localization-data/Verification.md
source_sha256: 61344eeeef781ce180b5c0d435efb0bb5f36c6cb541eb7c6c271cd9d507c4994
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Separate localization workbook registration, generated runtime assets and typed Enum API implemented. Row 1 B1 onward contains Enum names, column A contains language names, body cells contain translations. The included sample contains nine language rows.

Build passed with zero warnings/errors. Headless checks: 42 localization, 10 source creation and 82 ribbon assertions passed. Native Unity 6000.0.83f1 checks passed for Application.systemLanguage lookup, explicit language overrides, fallback, change events, generated Enum facade compilation, Resources.Load, automatic source-reimport rebuild, and restored sample values. See native logs and the preserved test probe sources. Editor probes were removed after execution.

The final English UI and four creation-menu entries were captured and inspected. Nine-language guides include the shared English capture and translated workflow instructions. Existing unrelated/shared captures have source-hash staleness after common editor changes; VerifyLocalizedDocs reports these as incomplete rather than updating hashes without recapture. No player build was run.

User workbooks and pre-existing edits were preserved. No Git commit or push was requested for this feature.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-27/localization-data/Verification.md)

- 원문 SHA-256: `61344eeeef781ce180b5c0d435efb0bb5f36c6cb541eb7c6c271cd9d507c4994`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
