---
title: 검사 CA 0.0.121 설치·로드 확인
date: 2026-10-23T06:33:02.140Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 검사 CA 0.0.121 설치·로드 확인 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/validation/profile-update-121/index.html
source_sha256: e3547ff38dc138a8a75ab3ce7227704e70782e01f368cb7bd945704cb4eaa6a6
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

CA 0.0.121 프로필 갱신·로드 검증

## 검사 CA 0.0.121 설치·로드 확인

승인된 검사 CA만 종료·갱신·재시작했습니다. Computer Use·키 입력·화면 촬영을 사용하지 않았습니다. VA PID 22044는 종료하지 않았습니다.

- 최초 종료 전 솔루션 및 열린 문서 35개 저장 확인

- 설치 패키지 바이너리 6개 일치

- 재시작한 CA PID 31280의 CLR 모듈 경로·파일 SHA256 일치

- 초기 재시작에서 CodeAssist 로드 오류 46건 발생: 캐시를 삭제하지 않고 보존한 뒤 검사 프로필 구성 재생성

- 복구 후 CodeAssist 로드 오류 0건, 자동 검사 6/6 및 Core 346/346 통과

- 활성 테스트 파일 내용 보존 확인

초기 실패를 통과로 바꾸지 않고 별도 로그·검사 결과로 남겼습니다. 구성 명령의 종료 코드는 확보하지 못했으며, 이후 실제 DLL 로드·프로필·파일 해시로 복구를 확인했습니다. 다른 VS 구성 오류는 로그에 남아 있습니다. 새 VA UI·기능 대조는 0건으로 유지합니다.

[초기 실패 검사](../assets/imports/docs/validation/profile-update-121/gate/index.html) · [복구 후 검사](../assets/imports/docs/validation/profile-update-121/gate-recovered/index.html) · 초기 시작 오류 · 복구 후 시작 로그 · 구성 갱신 로그 · 설치 로그

종료 전 문서 상태 · 재시작 후 상태 · 설치 해시 · 실제 CLR 로드 근거 · 결과 JSON

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/validation/profile-update-121/index.html)

- 원문 SHA-256: `e3547ff38dc138a8a75ab3ce7227704e70782e01f368cb7bd945704cb4eaa6a6`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
