# 검증 상태

버전: 1.0.0 / 기록 날짜: 2026-10-29 KST

## 구현 및 자동 검증

- JSON stdout·종료 코드, PowerShell 래퍼와 Codex 번들 Node 24.19.0 실행 확인.
- 자체 HTTP+SSE 및 Streamable HTTP 서버와 실제 CLI 자식 프로세스를 사용하는 통합 테스트 포함.
- 연결·도구 조회·동일 연결의 SDK preamble 읽기·스크립트 호출·렌더링 이미지 저장 검증.
- 잘못된 인자, 파일 덮어쓰기, 대상 문서 고정, 글꼴 미설치, 중복 레이어, 부분 실패, 연결 끊김·시간 초과 검증.
- PNG 파일의 크기·바이트·SHA-256 확인 및 base64 출력 생략 검증.
- JSON 디자인 예제의 CLI dry-run 통과.
- 네이티브 작업의 제어 흐름 테스트는 SDK 대역을 사용합니다. Affinity 자체 실행 검증을 의미하지 않습니다.

실행: `node --test tools/affinity-cli/test/*.test.mjs`. 테스트 수·최종 결과는 실제 실행 결과를 기준으로 확인합니다.

## 실제 Affinity에서 확인한 내용

- Windows 설치 패키지: Canva.Affinity 3.3.1.4940.
- 해당 앱 프로세스 실행 확인.
- 공식 SDK 사이트와 JSLib에서 3.3.1 버전 확인 및 사용 함수 소스 대조.
- 기본 MCP 주소 localhost·IPv6·IPv4의 6767 포트 연결은 ECONNREFUSED.
- CLI `doctor --timeout 2000`도 실제 서버 연결 거부를 오류 JSON으로 반환했습니다.

## 남은 실행 검증

실제 MCP 연결은 아직 열리지 않았으므로 아래 항목은 **미확인**입니다.

1. 설치된 서버의 실제 도구 스키마·스크립트 출력 형식과 CLI 호환.
2. 실제 문서 생성·도형·텍스트·이미지·레이어 조작.
3. 실제 네이티브 파일 저장·PNG 등 내보내기·결과물 렌더링과 시각 품질.
4. 각 작업 전후 OS 화면 포커스 비간섭.

사용자에게 MCP 서버 활성화를 요청한 상태입니다. 앱 설정을 임의로 변경하거나 프로그램을 재시작하지 않았습니다. `doctor` 연결이 성공하면 실제 SDK 문서를 같은 연결에서 읽고 전용 테스트 문서로 위 항목을 검증·수정해야 합니다. 기존 사용자 문서로 무작정 시험하지 않습니다.

## 공식 참조

- https://sdk.affinity.studio/latest/
- https://sdk.affinity.studio/latest/js/
- https://www.affinity.studio/features/scripting
- https://claude.com/marketplace/connectors/affinity-106b90
