---
title: Formula implementation progress
date: 2026-10-22T01:29:44.345Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: Formula implementation progress — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-20/FormulaImplementationProgress.md
source_sha256: d7b387998a2e1ad13d6cbe3af0e00b8ad6b2a79d9aac458cc90167de892e8738
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

Source counts only. Native Excel, UI and release acceptance remain separate.

| Family | Source implemented | Percent | Verified | Pending |
|---|---:|---:|---:|---:|
| date_time | 14/14 | 100.0% | 0 | 0 |
| dynamic_arrays | 1/1 | 100.0% | 0 | 0 |
| financial | 5/5 | 100.0% | 0 | 0 |
| logical | 3/3 | 100.0% | 0 | 0 |
| math | 21/21 | 100.0% | 0 | 0 |
| references | 3/3 | 100.0% | 0 | 0 |
| statistics | 11/11 | 100.0% | 0 | 0 |
| text | 11/11 | 100.0% | 0 | 0 |

Total: 69/69 (100.0%). Verified: 0/69.

## Pending names

## Implemented source limits

- CEILING: Finite scalar/elementwise arrays; legacy sign directions and MATH optional significance/mode; explicit omission unsupported; decimal/overflow/coercion native fidelity unverified.
- CEILING.MATH: Finite scalar/elementwise arrays; optional significance/mode when not supplied, and explicit final mode omission uses default 0. Explicit middle significance omission remains unsupported; decimal/overflow/coercion/native execution unverified.
- FLOOR: Finite scalar/elementwise arrays; legacy sign directions and MATH optional significance/mode; explicit omission unsupported; decimal/overflow/coercion native fidelity unverified.
- FLOOR.MATH: Finite scalar/elementwise arrays; optional significance/mode when not supplied, and explicit final mode omission uses default 0. Explicit middle significance omission remains unsupported; decimal/overflow/coercion/native execution unverified.
- MROUND: Finite scalar and elementwise arrays; opposite signs NUM, zero returns zero, nearest multiple with half away from zero. Decimal midpoint and native precision unverified.
- TRUNC: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- QUOTIENT: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- SUMSQ: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- EXP: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- LN: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- LOG: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- LOG10: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- SIN: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- COS: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- TAN: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- ASIN: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- ACOS: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- ATAN: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- ATAN2: Finite numeric scalar/elementwise arrays; ASIN/ACOS bounded to -1..1, ATAN2 uses Excel x,y order and excludes -pi endpoint. Large-angle and native precision unverified.
- DEGREES: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- RADIANS: Finite numeric inputs and existing elementwise array broadcasting; TRUNC digits restricted to integer -15..15; native precision/coercion not verified.
- STDEV: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- STDEV.S: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- STDEV.P: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- VAR: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- VAR.S: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- VAR.P: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- PERCENTILE: Inclusive sorted-data interpolation with scalar/array percentile arguments; quartile indices truncate; source-error data and unions guarded; native floating-point boundaries unverified.
- PERCENTILE.INC: Inclusive sorted-data interpolation with scalar/array percentile arguments; quartile indices truncate; source-error data and unions guarded; native floating-point boundaries unverified.
- QUARTILE: Inclusive sorted-data interpolation with scalar/array percentile arguments; quartile indices truncate; source-error data and unions guarded; native floating-point boundaries unverified.
- QUARTILE.INC: Inclusive sorted-data interpolation with scalar/array percentile arguments; quartile indices truncate; source-error data and unions guarded; native floating-point boundaries unverified.
- RANK: Finite numeric aggregates with direct/reference coercion distinction; source errors in reference areas guarded unsupported; RANK aliases RANK.EQ; native precision/coercion unverified.
- NOW: Local clock snapshot shared with TODAY per calculation instance, 1900/1904 serial conversion. Manual workbook recalculation API, View menu and F9 grid shortcut added. Automatic date formatting, volatile timing consistency across spill/cache/display phases and native behavior remain unverified.
- TIME: Finite scalar/elementwise arrays, TIME nonnegative truncated components 0..32767 and daily wrap; component numeric serials and invariant clock/English AMPM text. Locale/date text and native fractional-second fidelity unverified.
- HOUR: Finite scalar/elementwise arrays, TIME nonnegative truncated components 0..32767 and daily wrap; component numeric serials and invariant clock/English AMPM text. Locale/date text and native fractional-second fidelity unverified.
- MINUTE: Finite scalar/elementwise arrays, TIME nonnegative truncated components 0..32767 and daily wrap; component numeric serials and invariant clock/English AMPM text. Locale/date text and native fractional-second fidelity unverified.
- SECOND: Finite scalar/elementwise arrays, TIME nonnegative truncated components 0..32767 and daily wrap; component numeric serials and invariant clock/English AMPM text. Locale/date text and native fractional-second fidelity unverified.
- WEEKDAY: Numeric serial/elementwise arrays. WEEKDAY return types 1,2,3,11..17; WEEKNUM 1,2,11..17,21; ISO rollover and optional default. Pre-March-1900 week numbers unsupported; locale/fractional/native fidelity unverified.
- WEEKNUM: Numeric serial/elementwise arrays. WEEKDAY return types 1,2,3,11..17; WEEKNUM 1,2,11..17,21; ISO rollover and optional default. Pre-March-1900 week numbers unsupported; locale/fractional/native fidelity unverified.
- ISOWEEKNUM: Numeric serial/elementwise arrays. WEEKDAY return types 1,2,3,11..17; WEEKNUM 1,2,11..17,21; ISO rollover and optional default. Pre-March-1900 week numbers unsupported; locale/fractional/native fidelity unverified.
- EDATE: Serial/ISO date text, month truncation, end-day clamp, 1900/1904 date-system paths and elementwise arrays. 1900 serial zero unsupported; phantom leap day/locale/native coercion unverified.
- EOMONTH: Serial/ISO date text, month truncation, end-day clamp, 1900/1904 date-system paths and elementwise arrays. 1900 serial zero unsupported; phantom leap day/locale/native coercion unverified.
- DAYS: Numeric subtraction retains fractions per official remarks; ISO/English-month date text removes time, elementwise arrays, range checks. Other locale text unsupported; native coercion/boundaries unverified.
- NETWORKDAYS: Saturday/Sunday exclusion, inclusive signed NETWORKDAYS, WORKDAY binary search, deduplicated weekday holidays, date-array broadcasting. 100k holidays/10k outputs. Before March1900 unsupported; native text/coercion/limits unverified.
- WORKDAY: Saturday/Sunday exclusion, inclusive signed NETWORKDAYS, WORKDAY binary search, deduplicated weekday holidays, date-array broadcasting. 100k holidays/10k outputs. Before March1900 unsupported; native text/coercion/limits unverified.
- YEARFRAC: Bases 0..4, integer truncation, sorted dates, scalar/elementwise arrays, actual-year/leap and mean-year paths. 30/360 February-end and early1900 unsupported. Cross-year/locale/native edge semantics unverified.
- TEXT: Existing numeric/date/time/fraction formatter with strict success result, elementwise arrays, General/@ and empty format, color ignored. Locale directives and unsupported grammar retain cache. Native rounding/text-section semantics unverified. Fourth text sections and named color directives now supported as text output; indexed palette colors now resolve through workbook context; rich-text precedence remains unsupported/unverified.
- EXACT: Scalar/elementwise arrays; ordinal exact comparison; ASCII 0..31 cleaning; bounded repeat; Unicode scalar conversion. UNICHAR fractional codes/XML-unsafe controls guarded until ST_Xstring integration; native version boundaries unverified.
- FIND: Scalar/elementwise BMP text; FIND ordinal, SEARCH culture-invariant case-insensitive wildcard regex with 4096-pattern/100ms guard, REPLACE bounded insertion/removal. Surrogate inputs guarded pending compatibility-version handling; native locale/empty-text boundaries unverified.
- SEARCH: Scalar/elementwise BMP text; FIND ordinal, SEARCH culture-invariant case-insensitive wildcard regex with 4096-pattern/100ms guard, REPLACE bounded insertion/removal. Surrogate inputs guarded pending compatibility-version handling; native locale/empty-text boundaries unverified.
- REPLACE: Scalar/elementwise BMP text; FIND ordinal, SEARCH culture-invariant case-insensitive wildcard regex with 4096-pattern/100ms guard, REPLACE bounded insertion/removal. Surrogate inputs guarded pending compatibility-version handling; native locale/empty-text boundaries unverified.
- REPT: Scalar/elementwise arrays; ordinal exact comparison; ASCII 0..31 cleaning; bounded repeat; Unicode scalar conversion. UNICHAR fractional codes/XML-unsafe controls guarded until ST_Xstring integration; native version boundaries unverified.
- CLEAN: Scalar/elementwise arrays; ordinal exact comparison; ASCII 0..31 cleaning; bounded repeat; Unicode scalar conversion. UNICHAR fractional codes/XML-unsafe controls guarded until ST_Xstring integration; native version boundaries unverified.
- CHAR: ASCII scalar/elementwise arrays, CHAR truncated 1..255, CODE first character; extended Windows current-culture ANSI/Mac10000 runtime encoding paths. Missing providers/multibyte/surrogates unsupported; native OS charset fidelity unverified.
- CODE: ASCII scalar/elementwise arrays, CHAR truncated 1..255, CODE first character; extended Windows current-culture ANSI/Mac10000 runtime encoding paths. Missing providers/multibyte/surrogates unsupported; native OS charset fidelity unverified.
- UNICHAR: Integer Unicode scalar codes supported, including XML-unsafe controls through cell ST_Xstring escaping. Partial surrogates produce #N/A; fractional code points guarded. Chart cache unsafe-text refresh retained; native behavior unverified.
- UNICODE: Scalar/elementwise first Unicode code point; validates complete UTF-16 input and returns VALUE for an unpaired surrogate anywhere. Native coercion/error precedence and execution remain unverified.
- IFS: IFS/SWITCH scalar and bounded broadcast arrays via conditional engine; XOR parity aggregate with reference unions. Native evaluation order, recursion and omitted argument coercion unverified.
- SWITCH: IFS/SWITCH scalar and bounded broadcast arrays via conditional engine; XOR parity aggregate with reference unions. Native evaluation order, recursion and omitted argument coercion unverified.
- XOR: IFS/SWITCH scalar and bounded broadcast arrays via conditional engine; XOR parity aggregate with reference unions. Native evaluation order, recursion and omitted argument coercion unverified.
- ADDRESS: A1/R1C1, absolute modes 1..4, optional defaults, sheet name quoting/apostrophe escaping, elementwise arrays; empty sheet text unsupported; native fractional/coercion/quoting fidelity unverified.
- INDIRECT: Scalar A1/range/whole-reference and defined names, absolute/relative R1C1 rectangles, caller context, boundaries. External/structured/array-text references unsupported; native volatility/coercion unverified.
- OFFSET: Single contiguous worksheet reference, signed scalar offsets, optional positive height/width, reference identity and dynamic-array projection; negative dimensions/array offsets unsupported. Native coercion/recalc/structural fidelity unverified.
- PMT: Finite scalar/elementwise arrays, zero-rate branch, optional fv/pv/type defaults, omitted payment with other value, type 0 or 1, tiny-rate series. Native precision/domain/underflow fidelity unverified.
- FV: Finite scalar/elementwise arrays, zero-rate branch, optional fv/pv/type defaults, omitted payment with other value, type 0 or 1, tiny-rate series. Native precision/domain/underflow fidelity unverified.
- PV: Finite scalar/elementwise arrays, zero-rate branch, optional fv/pv/type defaults, omitted payment with other value, type 0 or 1, tiny-rate series. Native precision/domain/underflow fidelity unverified.
- NPV: Scalar or up to 10000 rate positions; ordered numeric cash flows, area100k/total1m and rate-count times flow-count <=1m. Rate worksheet errors localized. Cash-flow error ranges remain unsupported; native fidelity unverified.
- IRR: Ordered numeric cash flows including zero; ignores text/bools/blanks; scale-normalized Horner Newton, 20 iterations, 100k entries, guess default .1 and >-1. Error ranges unsupported; native root selection/convergence unverified.
- EXPAND: Scalar/matrix expansion, omitted rows/columns, scalar padding default NA, worksheet errors retained, output 100000 cells. Fractional dimensions/native evaluation fidelity unverified.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-20/FormulaImplementationProgress.md)

- 원문 SHA-256: `d7b387998a2e1ad13d6cbe3af0e00b8ad6b2a79d9aac458cc90167de892e8738`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
