---
title: 추천 순위 계산 최적화 — 0.0.119
date: 2026-10-23T06:11:46.568Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 추천 순위 계산 최적화 — 0.0.119 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/completion-ranking-119/index.html
source_sha256: e47b9dd8277c9346004a32eaf87ebbe742cd98f97a768f76f104a202664a4cec
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

추천 순위 계산 최적화 0.0.119

## 추천 순위 계산 최적화 — 0.0.119

글로벌·멤버 추천이 같은 후보 순위를 필터와 정렬에서 각각 계산하던 중복을 제거했습니다. 한 번 계산한 순위를 값 형식으로 보관합니다.

- 동일한 4,500개 심볼, 검색 160회 묶음, 7개 표본

- 결과 후보·순서 및 결과 SHA256 일치

- 중앙 시간 222.79 → 159.11ms (28.6% 감소)

- 평균 임시 할당량 100,271,195 → 89,571,661바이트 (10.7% 감소)

- Core 검사 336/336 통과, VS2019·Modern 빌드 완료

파싱·컴파일러·WPF·실제 키·IDE 전체 성능은 측정 범위 밖입니다. Computer Use 미사용, 새 VA 실행 비교 0건. 실행 중인 CA는 0.0.116이고 0.0.119는 설치 전입니다.

변경 전 원시 측정 · 변경 후 원시 측정 · Core TRX · 패키지 검사 · 빌드 로그 · 결과 JSON

Modern VSIX · VS2019 VSIX

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/completion-ranking-119/index.html)

- 원문 SHA-256: `e47b9dd8277c9346004a32eaf87ebbe742cd98f97a768f76f104a202664a4cec`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
