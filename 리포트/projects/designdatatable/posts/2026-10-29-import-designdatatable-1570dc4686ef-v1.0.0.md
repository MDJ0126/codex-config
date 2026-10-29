---
title: 사용 예제 카드 TC
date: 2026-10-29T09:14:16.448Z
version: v1.0.0
status: 기존 기록 이전
project: designdatatable
type: 작업 결과
category: 이전 기록
summary: 사용 예제 카드 TC — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/Docs/Testing/2026-10-29/usage-examples/TC-결과.ko.md
source_sha256: c362923a69ee9ad2657ddf2a140e165259784de2730c9c3315e14cb29488980a
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

| TC | 검증 | 실제 결과 |
|---|---|---|
| USE-01 | 홈 탭에서 사용 안내 노출 | Localization-Home.png에서 실제 카드 확인 |
| USE-02 | 선택한 번역 키 변경 | ArcadeTitle → ArcadeSubtitle에 따라 코드 변경 |
| USE-03 | 대체 언어의 의미 표시 | Fallback: English / 번역이 없을 때: English 표시 |
| USE-04 | 선택한 Id·헤더·빈 행 조회 예제 | 선택 Id 2, 헤더 선택 시 첫 유효 Id 1 사용 |
| USE-05 | 로컬 Enum 멤버·컬럼 변경 | 행과 컬럼을 바꾸면 실제 타입·멤버로 변경 |
| USE-06 | 전역 Enum namespace | 원본 Features의 실제 전역 namespace 사용 |
| USE-07 | 일반 스프레드시트 | 안내 카드와 추가 높이 없음 |
| USE-08 | Summary | 컬럼 동기화 설명 표시, 코드 복사 버튼 없음 |
| USE-09 | 720px 폭·9개 UI 언어 | 코드 줄바꿈 및 언어별 카드 실제 촬영 |
| USE-10 | 코드 복사 버튼 실제 클릭 | MouseDown/MouseUp으로 전체 코드 복사, Dirty 유지, 원래 클립보드 복원 |
| USE-11 | 표시된 코드 그대로 컴파일 | 예제 5개 포함 전체 C# 빌드 경고 0·오류 0 |
| USE-12 | 캐시·기존 UI 회귀 | 동일 선택 1000회 Enum 파싱 캐시 유지, 기존 잠금·상황별 리본·Summary 검사 통과 |
| USE-13 | 문서·스크린샷 | 32장 실제 재촬영, 9개 언어·37장 이미지 해시 오류 0, 브라우저 140개 통과 |

실제 Unity 6000.0.83f1의 격리 native HostView에서 실행했습니다. 전 게임·전 기능의 전체 QA를 의미하지 않습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/Docs/Testing/2026-10-29/usage-examples/TC-결과.ko.md)

- 원문 SHA-256: `c362923a69ee9ad2657ddf2a140e165259784de2730c9c3315e14cb29488980a`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
