---
title: 단축키 충돌 수정 0.0.93
date: 2026-10-18T07:37:45.524Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 버그
category: 이전 기록
summary: 단축키 충돌 수정 0.0.93 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/physical-shortcuts-093.html
source_sha256: 0e50985fd86333dbfc1932c27d942a146d693b62a9aa483d1aedcc5830b830e1
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

기존 전역 설정: 14/22 (63.64%). 임시 편집기 범위 적용: 21/22 (95.45%) 기대 CA 명령 수신. 전체 VA 기능 동등성을 뜻하지 않습니다.

명령 수신 미확인: Alt+Shift+R 이름 바꾸기. Alt+Right는 CA 명령을 수신했지만 이동 이력이 없어 안내 메시지가 열렸습니다. 기능 결과 통과로 계산하지 않습니다.

원인: VS 편집기 범위의 클립보드, 선택 확장·축소, 대응 파일, 자동 완성, 줄 이동 명령이 전역 CA 바인딩보다 우선했습니다. 실제 명령명·바인딩 근거는 충돌 기록입니다.

제품: VSCT에 편집기 범위 24개 추가(총 48개), 단축키 복원 설정에도 전역+편집기 범위 반영. VS2019/Modern 빌드 성공, 현대 VSIX 6개 바이너리·48개 키 등록 검증 통과. 실행 중 VS에는 새 DLL을 적용하지 않았으며, 임시 설정으로 동일 수정 원리를 검증했습니다.

설치 파일: installers/0.0.93/CodeAssist.vsix 및 CodeAssist.VS2019.vsix. 기존 사용자 프로필은 자동 강제 변경하지 않습니다. 설치 후 옵션의 단축키 복원 기능으로 적용할 수 있습니다.

| 키 | 입력 전송 | CA 수신 | 실행 오류 | 화면 |
| --- | --- | --- | --- | --- |
| ALT+K | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-1.png) |
| CTRL+SHIFT+V | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-2.png) |
| ALT+SHIFT+H | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-3.png) |
| ALT+SHIFT+] | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-4.png) |
| ALT+SHIFT+[ | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-5.png) |
| ALT+] | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-6.png) |
| ALT+[ | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-7.png) |
| CTRL+0 | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-8.png) |
| ALT+G | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-9.png) |
| ALT+SHIFT+F | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-10.png) |
| ALT+SHIFT+O | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-11.png) |
| ALT+SHIFT+S | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-12.png) |
| ALT+O | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-13.png) |
| ALT+M | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-14.png) |
| ALT+SHIFT+G | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-15.png) |
| ALT+LEFT | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-scoped-final/CA-scoped-16.png) |
| ALT+RIGHT | True | True | 피호출자가 호출을 거부했습니다. (예외가 발생한 HRESULT: 0x80010001 (RPC_E_CALL_REJECTED)) | 없음 |
| ALT+SHIFT+R | True | False |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-tail/CA-tail-1.png) |
| ALT+SHIFT+Q | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-tail/CA-tail-2.png) |
| ALT+SHIFT+C | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-tail/CA-tail-3.png) |
| ALT+DOWN | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-tail/CA-tail-4.png) |
| ALT+UP | True | True |  | [화면](../assets/imports/docs/validation/physical-shortcuts-093-tail/CA-tail-5.png) |

검사 설정 원복: 원본 27개 검증, 후속 10개 검증. WM_CLOSE는 자동 승인 검토에서 거부돼 실행하지 않았습니다. 실제 안내문·확인 버튼을 UI Automation으로 검증한 뒤 해당 정보창만 확인했습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/physical-shortcuts-093.html)

- 원문 SHA-256: `0e50985fd86333dbfc1932c27d942a146d693b62a9aa483d1aedcc5830b830e1`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
