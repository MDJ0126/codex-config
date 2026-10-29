---
title: 참조 검색 문맥 검증 — 0.0.121
date: 2026-10-23T06:20:31.422Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 참조 검색 문맥 검증 — 0.0.121 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/query-context-121/index.html
source_sha256: 9fe5fef94876cd4c66bd4b00f443674dcf86523219989070f1c3c3227e0717e1
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

참조 검색 문맥 검증 0.0.121

## 참조 검색 문맥 검증 — 0.0.121

검색 시작 당시의 솔루션 경로·인덱스 루트를 보관하고, 비동기 검색 완료·새로고침 전후·결과 창 열기 전후·출력 서비스 대기 뒤에 다시 확인합니다. 문맥이 바뀌거나 새 검색으로 취소된 최초 검색 결과를 게시하지 않도록 보강했습니다. 결과 창 대기 중 전역 last가 바뀌어도 캡처한 지역 스냅샷을 사용합니다.

- 문맥 검사 6/6: 솔루션 변경·인덱스 변경·대소문자·닫힌 문맥·늦은 공급자 결과

- 전체 Core 346/346 통과

- VS2019·Modern 빌드 완료, Modern 바이너리 6개 확인

Computer Use 미사용, 새 VA 대조 0건. 실제 VS 프로젝트 전환 및 동일 경로 닫기/재열기는 검증하지 않았습니다. 동일 경로 재열기를 별도 세대로 식별하는 기능은 없습니다. 실행 CA는 0.0.116이고 0.0.121은 설치 전입니다.

전체 TRX · 문맥 TRX · 패키지 검사 · 빌드 로그 · 결과 JSON

Modern VSIX · VS2019 VSIX

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/query-context-121/index.html)

- 원문 SHA-256: `9fe5fef94876cd4c66bd4b00f443674dcf86523219989070f1c3c3227e0717e1`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
