---
title: 게시물·게시판 링크 복사 기능
date: 2026-10-31T03:46:02.216Z
version: v1.0.2
status: 완료
state: complete
project: codex-config
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 보고서·QA 토론·작업 메뉴얼 목록과 상세에 링크 복사를 추가합니다.
format: tracefolio-v2
priority: normal
manual_id: copy-post-and-board-links
round: 1
manual_state: complete
payload: ../assets/submissions/2026-10-31-manual-copy-post-and-board-links-20261031034602216-v1.0.2.json
---

## 목표와 완료 기준

목록·상세에서 게시물 주소와 필터가 적용된 게시판 주소를 복사할 수 있도록 합니다.

완료 기준: 실제 클립보드·제한 시 대체 경로·필터 주소 복원·모바일·키보드·게시물 이동을 검증하고 공개 반영합니다.

최초 1회차 · 순차 진행 · 전체 3개 · 이번 실행 T01, T02, T03

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 공통 복사 기능과 유효한 목록·상세 버튼 | 링크 내부에 버튼을 중첩하지 않고 목록 행·토론/메뉴얼 카드·상세·게시판 상단에 버튼을 추가합니다. | 현재 게시물 또는 필터가 포함된 게시판 URL이 선택됩니다. | 생성 HTML과 실제 버튼·이동 링크를 대조합니다. | 완료 |
| T02 | 실제 복사·거부·주소 복원·모바일 QA | 실제 Clipboard API·대체 복사·직접 선택·필터 복원·키보드·화면 크기를 확인합니다. | 성공과 실패가 구분되고 복사 클릭으로 이동하지 않습니다. | 실제 클립보드 읽기와 거부 환경 모의, URL 새로열기,390px 넘침·필터 숨김을 검증합니다. | 완료 |
| T03 | 공개 배포와 실제 화면 보고 | 다른 로컬 변경을 제외한 격리 저장소에서 검증·푸시하고 공개 기능을 확인합니다. | 배포된 게시판과 글에서 복사가 동작하며 HTML 보고서에 실제 화면이 있습니다. | Actions 성공 및 외부 페이지·버튼·첨부 이미지 확인 | 완료 |

## QA 반복 기록

### Q01 · step · 2026-10-31T03:42:43.373Z

기존 템플릿 검사 최초 실행 실패의 원인 기록

- T02 r1 · fail: Windows 복제본의 기존 Markdown 템플릿 CRLF를 LF 전용 검사 정규식이 읽지 못해37개 중1개 실패했습니다. 기존 템플릿2개의 줄바꿈만 정규화한 뒤 재실행했습니다. · 근거: 초기 authoring.test.mjs TypeError match null 및 template-report.md/template-bug.md CRLF=true 확인; 제품 코드 변경 없음
### Q02 · regression · 2026-10-31T03:42:43.373Z

공통 기능·기존 단위·실제 브라우저 QA 통과

- T01 r1 · pass: 별도 복사 버튼을 공통 게시물 상세·보고서 목록·토론·메뉴얼 카드·게시판 상단에 추가하고 필터 주소 저장·복원을 구현했습니다. · 근거: assets/copy-links-2026-10-31/qa.json 27checks errors0; node --test 37/37 pass; build/verify 480pairs 17340links
- T02 r1 · pass: 실제 클립보드·대체 복사·수동 선택·URL 복원·키보드·390px 검사를27개 통과했습니다. 기존37개 단위 검사도 재실행 통과했습니다. · 근거: assets/copy-links-2026-10-31/qa.json 27checks errors0; node --test 37/37 pass; build/verify 480pairs 17340links
### Q03 · final · 2026-10-31T03:46:01.871Z

공통 기능·실제 클립보드·공개 배포 최종 QA

- T01 r1 · pass: 별도 복사 버튼을 공통 게시물 상세·보고서 목록·토론·메뉴얼 카드·게시판 상단에 추가하고 필터 주소 저장·복원을 구현했습니다. · 근거: assets/copy-links-2026-10-31/qa.json 28checks errors0; public-qa.json 10checks errors0; 단위37/37; Actions41165582316 success
- T02 r1 · pass: 실제 클립보드·대체 복사·수동 선택·URL 복원·키보드·390px 검사를27개 통과했습니다. 기존37개 단위 검사도 재실행 통과했습니다. · 근거: assets/copy-links-2026-10-31/qa.json 28checks errors0; public-qa.json 10checks errors0; 단위37/37; Actions41165582316 success
- T03 r1 · pass: Actions41165582316 성공 후 공개 사이트에서 실제 클립보드·카드·상세·필터 복원10개 검사를 확인했습니다. · 근거: assets/copy-links-2026-10-31/qa.json 28checks errors0; public-qa.json 10checks errors0; 단위37/37; Actions41165582316 success

## 남은 작업과 다음 반복

기능 구현·실행·공개 확인 완료. 권한 제한 시에는 주소 직접 선택 경로를 제공합니다.

이전 회차: [메뉴얼 열기](../../../projects/codex-config/reports/2026-10-31-manual-copy-post-and-board-links-20261031034243715-v1.0.1.html)

## 작업 시간

- 제안 시간: 5~8분
- 실제로 완료된 시간: 시작2026-10-31 12:38:41 KST · 공개 기능 확인 2026-10-31T03:46:01.871Z · 7.3분. 이 기록의 배포 확인은 최종 채팅에서 별도 표기합니다.
- XERJ: 미사용 — 제공된 XERJ 연결이 없어 rg와 현재 소스로 확인했습니다.
