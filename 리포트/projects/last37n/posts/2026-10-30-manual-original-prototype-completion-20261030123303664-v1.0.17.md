---
title: 최초 기획 잔여 범위 전체 구현·실행·반복 QA 메뉴얼
date: 2026-10-30T12:33:03.664Z
version: v1.0.17
status: 완료
state: complete
project: last37n
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 최초 프로토타입12개 작업과 추가 UI·캠프·NPC·경계 터렛의 구현·실행·부하·보고서 외부 검증을 통과했습니다.
format: tracefolio-v2
priority: normal
manual_id: original-prototype-completion
round: 4
manual_state: complete
payload: ../assets/submissions/2026-10-30-manual-original-prototype-completion-20261030123303664-v1.0.17.json
---

## 목표와 완료 기준

수원 기반 Unity TPS 생존 샌드박스의 최초 프로토타입 잔여 요구사항을 로컬 환경에서 구현·적용·실행 검증합니다. 전투·세력·차량 등 명시된 후속 확장은 제외합니다.

완료 기준: 설정 기반 생존·수색·NPC·중립 제한, 실제 수원 원본 공간과 검증된 시작점, PostgreSQL/PostGIS 원자 저장·복원, 독립 월드·주변 채팅·재접속, Windows와 실제 브라우저 플레이, 100명 동시 접속 부하와 전체 회귀 QA, 실행 문서와 실제 화면을 포함한 외부 보고서 배포가 모두 확인되어야 합니다. 추가 범위: UI 재설계, 물자 소품·세이프존 시설·역할별 NPC·설정 가능한 경계 터렛을 실제 게임에 적용하고 검증합니다.

후속 4회차 · 순차 진행 · 전체 12개 · 이번 실행 T12

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 기준점과 전체 실행 범위 보존 | 현재 소스를 로컬 Git 기준점으로 보존하고 최초 기획·기존 검토·실행 메뉴얼 지침을 대조합니다. | 복원 가능한 기준 커밋과 원래 전체 범위를 유지한 유효 메뉴얼이 존재합니다. | 기준 커밋을 실제 조회하고 JSON 계약 검사를 실행합니다. | 완료 |
| T02 | PostgreSQL·PostGIS 설치와 공간 쿼리 | 프로젝트 전용 바이너리·DB·앱 계정을 구성하고 PostGIS를 활성화합니다. | 실제 앱 접속·좌표 변환이 성공하며 앱 계정은 슈퍼유저가 아닙니다. | db-doctor.ts를 실행해 버전·ST_Transform·계정 권한을 확인합니다. | 완료 |
| T03 | 생존·휴식·수색·NPC·월드 재고 규칙 | 미정 수치를 provisional 설정으로 분리하고 활력·고갈 효과·거래·대사·월드별 물자를 구현합니다. | 수치 변화와 행동 제한, 무게·재고·거래·중립 제한이 서버에서 일관되게 판정됩니다. | 고정 시간·재현 가능한 난수로 정상/경계/실패·중복·월드 분리 테스트를 실행합니다. | 완료 |
| T04 | 트랜잭션 저장·중복 방지·복원 | PostGIS 저장소를 서버에 연결하고 기존 JSON 상태를 보존·이관합니다. | 아이템·재고·영수증이 함께 커밋/롤백되며 재시작과 재시도에도 보존됩니다. | 실제 DB에서 실패 유도·동일 ID 재전송·서버 재시작·공간 거리·동시 요청을 검증합니다. | 완료 |
| T05 | 수원 원본 지리 데이터와 안전한 시작점 | 라이선스를 확인한 실제 수원 경계·도로·건물을 확보하고 출처를 기록합니다. | 가상 격자가 아닌 원본 좌표가 사용되고 시작점이 범위·건물·중립/위험 조건을 통과합니다. | 원본 OSM 식별자·좌표와 결과를 대조하고 폴리곤·충돌·스폰 테스트를 실행합니다. | 완료 |
| T06 | Unity 실제 공간과 생존 플레이 흐름 | 지리 형상·건물·수색 장소·캠프를 표시하고 생존·상점·치료·휴식·채팅 UI를 연결합니다. | 탐색→수색→소비/회복→캠프 거래/치료가 실제 TPS에서 동작합니다. | Windows 빌드 후 실제 서버 연동 플레이·조작·모션·HUD를 실행 검증하고 실제 화면을 캡처합니다. | 완료 |
| T07 | 멀티 월드·접속 관리·관심 영역 | 월드별 100명 상한과 NPC/아이템 상태 분리, 재접속·주변 채팅·근처 플레이어 동기화를 완성합니다. | 월드 간 상태가 섞이지 않고 연결 교체·이탈·복원·관심 영역이 정확합니다. | 서로 다른 세션·두 월드에서 실제 클라이언트와 네트워크 통합 QA를 수행합니다. | 완료 |
| T08 | Unity Web 모듈과 브라우저 통신 | 동일 Unity 버전의 Web 빌드 환경과 WebSocket JS 연결 계층을 설치·구현합니다. | 실제 Web 빌드가 생성되고 네트워크·저장·맵 경로가 브라우저 제약에 맞습니다. | Web 빌드·자바스크립트 메시지·세션 복원 테스트를 실행합니다. | 완료 |
| T09 | TPS UI·세이프존 시설·NPC·터렛과 실제 브라우저 검증 | 로컬 HTTP 환경에서 Web 클라이언트를 실행하고 Windows 클라이언트와 같은 월드에 접속합니다. 사용자 추가 요청에 따라 HUD/배낭/상점 UI를 재설계하고 아이템 소품·구호소 시설·역할별 NPC·서버 상태 기반 경계 터렛을 구현합니다. | 브라우저 이동·수색·채팅·접속 복원과 서로의 상태가 실제 화면에서 확인됩니다. 시설과 NPC 역할이 3D 화면에서 구분되고 UI가 생존·아이템·거래·중립 경계를 읽기 쉽게 표시합니다. 터렛 발포는 미정 설정으로 분리하며 민간인에게 임의 피해를 주지 않습니다. | 실제 브라우저 입력·거래·치료·복구와 동시 Windows 접속을 검증합니다. Affinity 파일 접근이 거부되면 권한을 우회하지 않고 편집 가능한 SVG를 렌더링합니다. Blender 모델·HUD·배낭·상점·구호소·터렛의 실제 화면과 서버 상태를 대조합니다. | 완료 |
| T10 | 100명 부하와 장애 회귀 검증 | 월드별 부하와 지연/연결 종료/재시작 시나리오를 실행하고 발견 문제를 수정합니다. | 100명 동시 입력·상태 갱신이 검증되고 지연·오류·복원 결과가 측정됩니다. | 100개 실제 WebSocket 클라이언트로 위치·채팅·거래·월드 격리·재접속을 검사하고 재QA합니다. | 완료 |
| T11 | 재현 가능한 실행 도구와 문서 | 로컬 설치·기동·중지·복원·빌드 도구와 README 구현 현황을 최종 결과에 맞춥니다. | 다음 실행에서 소스와 설명을 따라 Windows/브라우저/DB를 기동할 수 있습니다. | 문서의 명령을 실제 실행하고 라이선스·설정·비밀정보 제외·결과 화면을 확인합니다. | 완료 |
| T12 | 전체 최종 QA와 아카이브 배포 | 전체 현재 revision을 함께 회귀 QA하고 실제 화면·시간·남은 제한을 보고합니다. | 모든 현재 작업 QA와 전체 final QA가 통과하고 보고서 외부 URL도 확인됩니다. | 전체 자동 검사·Windows/브라우저 실행·DB 복원·100명 결과를 재검증하고 Pages 배포 및 링크를 확인합니다. | 완료 |

## QA 반복 기록

### Q01 · step · 2026-10-30T09:35:20.375Z

기준점과 메뉴얼 전체 범위 검사

- T01 r1 · pass: 기준 커밋 존재, 최초 검토 보고서 및 전체 12개 실행/QA 기준 보존, JSON 계약 검사 성공 · 근거: git rev-list --max-parents=0 HEAD; validateReport(manual, project registry)
### Q02 · step · 2026-10-30T09:35:20.551Z

실제 DB 설치·공간 연산·권한 검사

- T02 r1 · pass: PostGIS 좌표 변환이 POINT를 반환하고 app_is_superuser=false입니다. · 근거: node server/tools/db-doctor.ts; 로컬 증거 docs/qa/database-install.json (원문에는 로컬 경로가 있어 공개 첨부하지 않음)
### Q03 · step · 2026-10-30T09:40:35.721Z

현재 생존·수색·NPC 도메인 QA

- T03 r1 · pass: node --test server/test/game.test.ts: 16개 통과, 실패 0. 서버/DB 통합 QA를 이 결과에 포함하지 않습니다. · 근거: 현재 server/test/game.test.ts 실제 실행 결과와 server/src/game.ts 및 config/content 파일 대조
### Q04 · step · 2026-10-30T09:50:58.646Z

실제 DB 저장·오류·재시도·이관 QA

- T04 r1 · pass: TypeScript 검사 성공, 현재 전체 27개 테스트 통과. 실제 PostgreSQL WebSocket 프로세스 재시작과 JSON 백업 동일성·원문 토큰 제외를 확인했습니다. 초기 TypeScript 검사에서 미사용 자체 WebSocket 파일의 TS2345가 있었고 표준 ws 교체 후 해당 파일 제거·재검사를 수행했습니다. · 근거: node --test server/test/*.test.ts; pnpm -C server exec tsc --noEmit; docs/qa/postgres-transactions.json; docs/qa/database-runtime.json
### Q05 · regression · 2026-10-30T09:50:59.352Z

DB 연결 이후 생존·거래 회귀 QA

- T03 r1 · pass: 전체 27개 테스트 실행에 현재 생존·수색·NPC 도메인 16개가 포함되어 모두 통과했습니다. · 근거: server/test/game.test.ts 및 동일 전체 회귀 실행 결과
### Q06 · step · 2026-10-30T09:52:38.221Z

수원 경계 원본 첫 요청 실패

- T05 r1 · fail: Overpass 주 서버가 HTTP 502를 반환했고 모든 query worker가 사용 중이며 queue가 가득 찼다고 응답했습니다. 지리 데이터는 아직 확보하지 못했습니다. · 근거: node server/tools/overpass.ts 실제 HTTP 502 응답. 공식 공개 인스턴스 목록과 이용 제한을 확인한 대체 접근을 진행합니다.
### Q07 · step · 2026-10-30T10:13:11.265Z

실제 수원 원본 전환 중 회귀 실패와 수정 기록

- T05 r1 · fail: 가상 블록 좌표를 전제한 이동 테스트 2개와 구 렌더러 잔여 코드의 Unity 컴파일 오류를 실제 실행으로 확인했습니다. 테스트 경로를 실제 개방 도로로 바꾸고 잔여 코드를 제거한 뒤 서버 29개 통과 및 Windows 빌드 성공을 재확인했습니다. 물가·제한 영역을 추가 추출하여 최종 공간 QA를 진행합니다. · 근거: node --test server/test/*.test.ts: 29 pass, 0 fail; .runtime/build-windows.log exit 0 after initial compile failure
### Q08 · step · 2026-10-30T10:14:50.526Z

원본 대안 확보·공간 최종 QA 및 회귀 통과

- T05 r1 · pass: 수원 행정 경계·도로·건물·물가·접근 제한 및 보행 다리를 서버에서 실제 조회하고 검사했습니다. 시작 후보·수색 장소·NPC 모두 수원 내부의 개방 보행 도로 위이며 건물/물가/제한 영역을 피합니다. 원본은 공식 체크섬과 일치하고 파생 데이터 출처·임시 높이를 기록했습니다. 전체 34 tests pass, 0 fail, 0 skipped 및 tsc 통과. · 근거: server/world-data/suwon/provenance.json; server/test/geography.test.ts 5개; docs/qa/geography-runtime.json; 실제 node --test server/test/*.test.ts
### Q09 · step · 2026-10-30T10:16:33.370Z

공간 HTTP 실행 근거 시각 정정 및 재검증

- T05 r1 · pass: Q08 작성 직전 제한된 셸의 로컬 HTTP 요청은 소켓 접근 거부로 실패했으므로 그 시점의 HTTP 완료 표현은 조급했습니다. 허용된 환경에서 10:15:05Z 실제 재요청하여 PG health와 건물579·도로454·영역5의 원본 기반 구역 응답을 확인하고 docs/qa/geography-runtime.json을 실제 응답으로 갱신했습니다. 공간 5개 및 전체34개 테스트도 모두 실제 통과했습니다. · 근거: docs/qa/geography-runtime.json checkedAt 2026-10-30T10:15:05.2399785Z
### Q10 · step · 2026-10-30T10:28:51.058Z

실제 클라이언트 첫 회차 실패 보존

- T06 r1 · fail: 실제 캡처에서 건물 벽이 검게 나왔으며 앞뒤 삼각형의 법선 상쇄를 코드로 확인했습니다. 첫 market 실행은 이동 완료 null 처리 차이로 검사기가 timeout되어 실패했습니다. batchmode의 자체 ScreenCapture 파일도 생성되지 않았습니다. 실패 JSON과 실제 화면을 보존했습니다. · 근거: docs/qa/attempt-01/market.json 및 market-initial.png; .runtime/client-qa/market.log 이력
### Q11 · step · 2026-10-30T10:28:51.059Z

Unity 실제 TPS·생존·NPC 재QA와 상점 HUD 확인

- T06 r1 · pass: 법선·이동 완료 검사 수정 후 실제 Windows 플레이어 market/merchant/medic/guard/survivor 다섯 시나리오가 모두 종료 코드0 및 passed=true입니다. 걷기2.42m·달리기6.57m, 수색·동일요청재시도·소비·휴식·채팅·구매·판매·치료·대화를 DB 연결 서버 상태로 검증했습니다. 비표시 Windows 데스크톱에서 실제 상점 HUD PNG를 생성하고 직접 열어 한글·재고·가격·대화·생존 바 표시를 확인했습니다. · 근거: docs/qa/runtime/*.json; merchant-dialogue-hud.png; tools/verify-windows.ps1; tools/verify-offscreen.ps1
### Q12 · step · 2026-10-30T10:40:07.172Z

실제 멀티플레이·재접속·월드·접속 한도 통과

- T07 r1 · pass: Windows Unity 세 클라이언트 동시 실행에서 같은월드 위치/채팅과 다른월드 격리 확인. 테스트 서버 실제 재시작 후 세 캐릭터 모두 자동 복구. offline명령한번처리·동일ID중복방지·새월드독립인벤토리·원월드복귀 통과. 실제100접속/101번째거부/재접속슬롯/다른월드허용 및 원거리관심영역 테스트 통과. 전체36 tests pass, 0 fail, 0 skipped; tsc pass. 실제 월드목록4개를 읽는 HUD 캡처를 직접 확인했습니다. · 근거: docs/qa/network/{alpha,beta,gamma}.json; docs/qa/runtime/lobby.json; lobby-world-list-hud.png; server/test/transport.test.ts
### Q13 · step · 2026-10-30T10:51:03.127Z

Web 실제 화면의 IL2CPP 구성요소 누락 발견

- T08 r1 · fail: 공식 Web 모듈 설치와 Web 빌드는 성공했으나 실제 headless Edge 화면·Unity 콘솔에 BoxCollider 클래스 누락 오류가 표시되어 브라우저 실행 QA는 실패입니다. 첫 smoke 도구가 JS 예외만 확인해 passed=true를 썼던 결과는 review.json에서 무효화했습니다. Unity6000 공식 CreatePrimitive 문서의 stripping 조건과 코드의 동적 생성 방식을 대조하여 link.xml 보존을 추가하고 대상 프로그램 오류도 검사하도록 수정했습니다. 재빌드 중입니다. · 근거: docs/qa/browser/attempt-01/{first-run.png,smoke.json,review.json}; Assets/link.xml; Unity6000 GameObject.CreatePrimitive 공식 문서
### Q14 · step · 2026-10-30T11:00:32.131Z

추가 엔진 코드 제거 오류와 보존 조치

- T08 r1 · fail: 두 번째 실제 Web 실행은 누락 클래스 ID115·serialization layout·NullReferenceException을 대상 콘솔에서 확인해 실패했습니다. 엔진의 안내에 따라 Strip Engine Code를 끄고 CleanBuildCache로 재빌드했습니다. 실패 화면과 JSON은 attempt-02에 보존했습니다. · 근거: docs/qa/browser/attempt-02; Assets/Editor/PrototypeBuild.cs stripEngineCode=false
### Q15 · step · 2026-10-30T11:00:32.132Z

실제 headless 브라우저 Web 실행 재QA 통과

- T08 r1 · pass: 공식 Unity6000.0.83f1 Web 모듈 설치exit0, Web 실제 빌드exit0. Headless Edge156.0.4314.50에서 브라우저 JS WebSocket으로 기존 QA세션 인증·인벤토리 복원·PostGIS 서버 연결·수원579건물/454도로 구역 로딩을 확인했습니다. 강화된 검사에서 JS 및 Unity 콘솔 오류0, 실제 화면에서 한글HUD/캐릭터/건물과 오류창 부재를 직접 확인했습니다. 사용자 화면·마우스·키보드·포커스 조작 없이 실행했습니다. · 근거: docs/qa/browser/smoke.json passed=true errors=[]; first-run.png; .runtime/build-web.log
### Q16 · step · 2026-10-30T11:11:12.550Z

브라우저 실제 입력 첫 회차 실패 보존

- T09 r1 · fail: 실제 headless 브라우저 가상키 입력 후 검사 도구가 이동량0m를 관찰하여 실패했습니다. 게임 캡처에는 캐릭터 주변 장면 변화와 활력 변화가 있어, 상태 갱신 시점과 입력/관찰 동기화를 실제 서버 기록과 대조 중입니다. 원인을 아직 단정하지 않았습니다. 함께 기다리던 전용 무그래픽 Windows 검사 프로세스만 종료했습니다. · 근거: docs/qa/browser/integration.json observed=0 및 integration-failed.png; InputManager Horizontal/Vertical 매핑 확인
### Q17 · step · 2026-10-30T11:20:19.372Z

캔버스 포커스 보정 후 실제 입력 성공·채팅 Enter 결함 발견

- T09 r1 · fail: 실제 캔버스 클릭/포커스 후 브라우저 이동2.898m(전송입력4개), 휴식·수색·소비·월드 선택 및 Windows→브라우저 채팅은 통과했습니다. 반대 방향 채팅은 서버 영수증에 없고 화면의 채팅 입력칸에 후속 이동키가 붙은 것을 확인했습니다. 텍스트필드에 포커스가 있을 때 Enter가 전송되지 않는 결함이며, OnGUI 키 이벤트에서 전송하도록 수정하고 재빌드 중입니다. 전체 상호운용 회차는 실패로 보존했습니다. · 근거: docs/qa/browser/attempt-integration-02; 실제 QA DB chat receipts 대조; ThirdPersonWorld.OnGUI 수정
### Q18 · step · 2026-10-30T11:32:09.513Z

브라우저/Windows 복구 통과·첫 NPC 입력 순서 결함 발견

- T09 r1 · fail: 브라우저 실제 이동·수색·소비·휴식·월드 선택·양방향 채팅·상대 접근·서버 재시작·offline 명령 한번처리·원래 월드 캐릭터/인벤토리 복원은 통과했고 실제 Windows 상호운용 JSON도 passed=true입니다. 이후 상점 첫 E 입력이 Unknown command로 실패했습니다. 별도 실제 브라우저 probe에서 첫 입력의 type/target이 빈 문자열이고 두 번째는 npc/merchant/talk임을 확인했습니다. Update에서 대상 탐색이 입력 뒤에 있어 최초 프레임에 빈 기본값을 쓰는 코드 순서를 수정하고 재빌드합니다. · 근거: docs/qa/browser/attempt-integration-03; attempt-npc-01/npc-input-probe.json; ThirdPersonWorld.Update의 Nearest 순서 수정
### Q19 · step · 2026-10-30T12:08:08.982Z

UI 실제 화면 결함 수정과 Affinity 접근 제한

- T09 r2 · fail: Affinity 3.3.1.4940 연결은 확인했지만 job 파일 접근이 PERMISSION_DENIED로 실패했습니다. 실제 Web 캡처에서 패널 tint가 흰색으로 출력되는 결함도 확인했습니다. 권한을 우회하지 않고 SVG 원본에서 PNG atlas를 렌더링했고 DrawTexture 색상 인자를 수정해 어두운 패널을 실제 화면에서 재확인했습니다. · 근거: art/ui/SurvivalIcons.svg; docs/qa/browser/first-run.png; SurvivalHud.Panel
### Q20 · step · 2026-10-30T12:11:41.302Z

새 UI·캠프·상호운용·100명 부하·실행 도구 통과

- T09 r3 · pass: 새 HUD/배낭/상점/치료 UI를 실제 Web 입력으로 검증했습니다. Windows와 Web 양방향 채팅·상대 접근·서버 재시작·동일 캐릭터 복구·offline 명령 한번처리·월드 인벤토리 격리를 통과했습니다. 경비병·모래주머니·터렛·의료 천막 입구·야전 침대·물자를 실제 캡처로 확인했습니다. 최종 화면/엔진 오류0. MCP에서 게임 탭과 배낭 표시도 확인했습니다. · 근거: docs/qa/browser/integration.json; docs/qa/camp/visual-run.json; docs/qa/camp/*-facility.png; docs/qa/camp/turret-state.json
- T10 r1 · pass: 실제 PostgreSQL 서버와100개 WebSocket 클라이언트의90초 지속 부하: 실제 이동80명, 거래/채팅,10명 재접속, 명령9900개, 응답900개, p95 83.90ms, 최대 스냅샷 간격220.10ms, 오류0.100명 Unity 렌더링 시험은 아닙니다. 별도 전체 회귀37개가 월드 격리/용량 초과 거부/DB 재시작을 포함해 통과했습니다. · 근거: docs/qa/load/sustained-100.json; server/test/transport.test.ts 실행; server/test/postgres.test.ts 실행
- T11 r1 · pass: README/아키텍처를 실제 지리·PostGIS·Web·NPC/UI 상태와 맞췄습니다. start-local→stop-local→start-local을 실제 실행하여 DB 체크포인트 후 서버 종료·다시 기동·HTTP200을 확인했습니다. PostgreSQL은 유지하고 다른 프로젝트를 종료하지 않았습니다. 전체 테스트37 pass/0 fail/0 skip, pnpm TypeScript 검사 통과. · 근거: README.md; docs/ARCHITECTURE.md; tools/start-local.ps1; tools/stop-local.ps1; docs/qa/local-lifecycle.json
### Q21 · final · 2026-10-30T12:33:03.188Z

전체 현재 버전 실제 회귀와 최종 보고서 외부 배포 검증 통과

- T01 r1 · pass: 원래12개 작업과 실패 이력을 유지했고 기준 커밋a5919c39 및 구현 보존 커밋96c2c4bf를 실제 조회했습니다. · 근거: git cat-file; git log; docs/qa/final-source-checks.json
- T02 r1 · pass: 실제 PostgreSQL17.11/PostGIS3.6.2에서 좌표 변환과 비관리자 앱 계정을 재확인했습니다. · 근거: docs/qa/database-install.json
- T03 r1 · pass: 최종37개 서버 테스트에서 생존·휴식·수색·거래·치료·중립 제한·재충전과 기본 비접속 규칙을 통과했습니다. · 근거: server/test/game.test.ts 실제 실행; docs/qa/final-regression.json
- T04 r1 · pass: 실제 PostGIS 트랜잭션 강제 실패 롤백·중복 동시 요청·지속 영수증·캐시 퇴출·프로세스 재시작 검사 통과. · 근거: server/test/postgres.test.ts 및 transport.test.ts 실제 실행
- T05 r1 · pass: 실제 수원 경계·도로·건물·물가/접근 제한·다리 및 시작점/시설 위치 검사 통과. gzip/원문 SHA-256 일치와 출처/ODbL 확인. · 근거: docs/qa/final-source-checks.json; server/test/geography.test.ts
- T06 r1 · pass: 최종 Windows5개 시나리오와 실제 Web 화면에서 캐릭터·도시·생존·수색·아이템·NPC·시설을 확인했습니다. · 근거: docs/qa/runtime/*.json; docs/qa/camp/*-facility.png
- T07 r1 · pass: 실제 WebSocket 용량100/101거부·월드 격리·주변 채팅, 실제 Windows/Web 양방향 표시·채팅·재접속·인벤토리 복원 통과. · 근거: docs/qa/browser/integration.json; windows-interop.json; docs/qa/network
- T08 r1 · pass: 동일 소스 Windows/Web 빌드 성공. 최종 Web18개 검사와 엔진 오류0, Windows 상호운용5개 검사 통과. · 근거: docs/qa/final-regression.json의 실제 빌드 해시; browser/integration.json
- T09 r3 · pass: 새 HUD·배낭·거래·의료 UI 실제 입력 통과. Blender 물자·천막/침대·모래주머니·역할 NPC·터렛 화면 확인. 실제 Unity에서 Yaw 축과32.05도 회전·중립 정책 통과. Affinity 거부는 SVG 렌더링 대안으로 처리. · 근거: docs/qa/runtime/guard.json; docs/qa/browser/integration.json; docs/QA_NOTES.md
- T10 r1 · pass: 100명 약92초 지속 부하에서 이동80명·거래/채팅·10명 재접속·9900명령,900응답,p95 83.90ms,상태 최대220.10ms,오류0. · 근거: docs/qa/load/sustained-100.json
- T11 r1 · pass: README 명령 frozen install·시작·DB저장 후 종료·재기동·타입 검사 실제 통과. 라이선스·알려진 비밀정보·파일크기 제외 검사 완료. · 근거: docs/qa/local-lifecycle.json; docs/qa/final-source-checks.json
- T12 r1 · pass: 전체 현재 버전 QA를 대조했습니다. 최종 보고서 Actions41055814526 success, 외부 HTML/실제 이미지 HTTP200. 데스크톱1600/모바일390 레이아웃·4개 이미지,379개 게시물/13530링크/필터 조합 검사 통과. · 근거: docs/qa/final-deployment.json; docs/qa/progress-report-layout-final.json

## 남은 작업과 다음 반복

현재 초기 프로토타입 작업 없음. 평면 지형·임시 높이·관찰 전용 터렛·localhost 인증·운영 부하 한계와 후속 확장은 최종 보고서에 명시했습니다.

이전 회차: [메뉴얼 열기](../../../projects/last37n/reports/2026-10-30-manual-original-prototype-completion-20261030121150966-v1.0.16.html)

## 작업 시간

- 제안 시간: 전체 잔여 범위 4~10시간, 최초 DB·생존 실행 단계 30~60분 예상으로 18:17에 안내했습니다.
- 실제로 완료된 시간: 전체 최종 QA 통과 2026-10-30 21:33:03 KST(Asia/Seoul). 최초 시작 후 3시간 15분 33초, 추가 UI/시설 범위 48분 42초. 이 메뉴얼 새 버전 게시와 외부 조회 기록은 최종 전달 검사에 별도 보존합니다.
- XERJ: 미사용 — XERJ MCP/검색 스크립트가 제공되지 않아 rg와 현재 코드·설정·실행 결과를 대조합니다.
