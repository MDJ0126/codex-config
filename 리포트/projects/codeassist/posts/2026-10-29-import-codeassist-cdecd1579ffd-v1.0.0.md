---
title: 멤버 추천 옵션 연결 — 0.0.118
date: 2026-10-23T06:08:06.523Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 멤버 추천 옵션 연결 — 0.0.118 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/member-matching-118/index.html
source_sha256: 7a365177749df45f5d7e3ea98e90844e99bb0efc4d6c5979800e0b6895ae32a4
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

멤버 추천 옵션 연결 0.0.118

## 멤버 추천 옵션 연결 — 0.0.118

일반 전역 추천과 달리 멤버 추천은 약어/Shorthand 옵션을 받지 않고 항상 부분 일치를 허용했습니다. 이제 CA 인덱스 멤버 추천 및 Super fallback에 실제 옵션을 전달합니다. 심볼 찾기의 독립적인 fuzzy 검색은 유지합니다.

- 옵션 네 조합: 상속 멤버·무관한 타입 제외·접두어·약어·비접두어·부분 문자열 검사 통과

- 전체 Core 336/336 통과

- VS2019·Modern 빌드 및 Modern 바이너리 6개 확인

Computer Use 미사용. 설치된 CA는 0.0.116입니다. 0.0.118은 설치 전이며, 0.0.117의 새로고침 격리 수정도 포함합니다. VS 자체/컴파일러 추가 후보와 WPF 선택·실제 키 동작은 새로 검증하지 않았습니다. VA 동기화율을 올려 계산하지 않았습니다.

Core TRX · 패키지 검사 · 빌드 로그 · 결과 JSON

Modern VSIX · VS2019 VSIX

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/member-matching-118/index.html)

- 원문 SHA-256: `7a365177749df45f5d7e3ea98e90844e99bb0efc4d6c5979800e0b6895ae32a4`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
