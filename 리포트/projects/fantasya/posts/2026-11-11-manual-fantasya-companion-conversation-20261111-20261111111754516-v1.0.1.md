---
title: 동료 대화·PC 에이전트 참여·채팅과 마이크 입력 작업 기록
date: 2026-11-11T11:17:54.516Z
version: v1.0.1
status: QA 중
state: verification
project: fantasya
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 실제 Unity와 사용자 PC Codex로 대화·약속·행동을 확인하고 음성 입력을 분리합니다.
format: tracefolio-v2
priority: normal
manual_id: fantasya-companion-conversation-20261111
round: 1
manual_state: qa
payload: ../assets/submissions/2026-11-11-manual-fantasya-companion-conversation-20261111-20261111111754516-v1.0.1.json
---

## 목표와 완료 기준

같은 동료의 성격·기억·약속을 게임 안팎에서 공유하고 PC 에이전트와 채팅·음성으로 상호작용한다.

완료 기준: 실제 Codex MCP 대화·Unity 적용, 대기·추종·입력 차단, 음성 검토 후 전송, 모체 공용화, 문서와 공개 근거 확인을 구분한다.

최초 1회차 · 순차 진행 · 전체 6개 · 이번 실행 T01, T02, T03, T04, T05, T06

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 공유 성격·기억·약속 서비스 | 동료 ID와 저장소·명시 명령을 구성한다. | 재시작·동료 분리·약속 우선 검증 | 기억 재시작·약속·인증 테스트와 실제 서비스 재시작을 확인한다. | 완료 |
| T02 | 게임 대화창과 실제 행동 | 저장된 UI와 행동 이유, 입력 차단을 연결한다. | 대기·추종·입력 차단이 실제 Unity에서 동작 | 실제 버튼·위치·receipt·전후 카메라 출력을 비교한다. | 완료 |
| T03 | PC 에이전트 MCP 참여 | 설정기와 stdio·CLI·제어권·수신함을 연결한다. | 실제 Codex가 자기 모델로 답하고 행동 적용 확인 | 설치된 Codex CLI로 observe·claim·wait·say·act·receipt·release를 실행한다. | 완료 |
| T04 | 채팅·마이크 모드 분리 | 명시 녹음·로컬 인식·검토·확인 전송·취소를 구성한다. | 인식만으로 약속이 바뀌지 않으며 확인 후 반영 | 물리 마이크 없이 합성 한국어를 실제 Unity 인코더와 Whisper로 처리한다. | 완료 |
| T05 | 공용 인코더 모체 반영 | 독립 Game.Audio로 분리하고 승인 범위만 반영한다. | 동일 소스·실행 검증과 모체 문서 배포 | 소스 해시·문서 검사·모체 main·Actions·공개 본문과 Mermaid를 확인한다. | 완료 |
| T06 | 문서·보고·공개 근거 정리 | README와 Dosc, JSON v2 결과·소스·화면을 정리한다. | 문서 링크와 개별 공개 본문·첨부 확인 | 정적 링크 검사와 아카이브 빌드·배포·외부 접근을 확인한다. | QA 대기 |

## QA 반복 기록

### Q01 · step · 2026-11-11T10:19:57.106Z

배치 가상 입력 첫 시도 실패 보존

- T02 r1 · fail: 가상 W를 보내도 리더 위치가 변하지 않아 대기 중 리더 이동 검증으로 인정하지 않았습니다. · 근거: 06-wait-before-walk.json, 07-wait-after-walk.json
### Q02 · step · 2026-11-11T11:07:07.356Z

실제 실행·통합 검사·모체 공개 결과 대조

- T01 r1 · pass: 지속 약속과 동료별 기억을 확인했습니다. · 근거: 재시작 기억·약속·입력검사 Node 테스트, 03-memory-after-service-restart.json
- T02 r1 · pass: 대기 중 리더 3.465m 이동, 따라와 뒤 루나 이동, 입력 차단을 확인했습니다. · 근거: 10-virtual-walk-after.json, 18-follow-after.json, 13-typing-after.json
- T03 r1 · pass: 실제 Codex의 게임 대화 응답 6483ms와 Unity 명령 적용을 확인했습니다. · 근거: 15-native-observed.json와 실제 Codex 도구 실행
- T04 r1 · pass: 인식 검토·확인 뒤 hold 적용·취소 뒤 결과 폐기를 확인했습니다. · 근거: 34-shared-speech-preview.json, 36-final-acknowledged.json, 31-cancellation-after-wait.json
- T05 r1 · pass: c7b40e5 main 푸시·문서 배포·공개 Mermaid 표시를 확인했습니다. · 근거: shared-module-public.json, 소스 해시 CAC63EF4AD6896BD12FD75ACE2167F9DB76A34C051B1F8854F7E7FA3D3886391
### Q03 · step · 2026-11-11T11:17:52.800Z

공개 결과 검증과 아카이브 카드 제한 기록

- T06 r1 · fail: 보고서·본문 이미지·Mermaid·코드 강조와 30개 URL 접근은 통과했습니다. 목록 카드에는 대표 이미지가 없어 사용자 썸네일 기준 전체를 통과하지 못했습니다. · 근거: public-verification.json; 공개 브라우저에서 카드별 이미지 0개 관찰. 본문 이미지 전부 로딩, 도표 4개 렌더링, 코드 7개 강조 확인.

## 남은 작업과 다음 반복

물리 마이크 실제 발화와 Claude/Cursor 앱 내부 접속은 미확인입니다. 사용자 PC에서 직접 녹음 시작을 눌러 확인할 수 있도록 구현했습니다. 목록 카드 썸네일은 아카이브 템플릿 미지원이므로 T06을 QA 상태로 유지합니다. 본문 대표 이미지·전후 이미지·도표·첨부 접근은 확인했습니다. Fantasya 기존 미커밋 작업을 보존했으며 게임 빌드와 Fantasya 저장소 푸시는 실행하지 않았습니다.

이전 회차: [메뉴얼 열기](../../../projects/fantasya/reports/2026-11-11-manual-20261111110848696-v1.0.0.html)

## 작업 시간

- 제안 시간: 18:47 KST 시작, 최초 70~100분. 채팅·마이크 모드 추가 요청으로 20~35분 추가 예상(총 90~135분).
- 실제로 완료된 시간: 구현·게임 실행·공용화·모체 푸시·배포 2026-11-11 20:04 KST까지 약 77분. 공개 보고서 배포·브라우저·HTTP 검증과 최종 기록 2026-11-11T11:17:52.800Z(UTC), 시작부터 90.0분. 이후 작업용 자원 정리와 기록 게시 결과를 최종 응답에서 구분합니다.
- XERJ: 미사용 — 사용자 전역 지침에 따라 XERJ에 연결하지 않고 rg·현재 소스·로그·실제 Unity 결과를 확인했습니다.
