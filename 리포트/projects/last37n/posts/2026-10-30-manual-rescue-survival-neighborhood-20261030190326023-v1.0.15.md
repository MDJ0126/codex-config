---
title: 동네 파밍·피난민/조난민 구조·피로/스트레스 생존
date: 2026-10-30T19:03:26.023Z
version: v1.0.15
status: 완료
state: complete
project: last37n
type: 작업 메뉴얼
report_kind: manual
category: 순차 작업 · 반복 QA
summary: 실제 동네 도로의 파밍과 선택 구조·호위, 주기 생존 수치와 스트레스를 서버 기준으로 연결합니다.
format: tracefolio-v2
priority: normal
manual_id: rescue-survival-neighborhood
round: 1
manual_state: complete
payload: ../assets/submissions/2026-10-30-manual-rescue-survival-neighborhood-20261030190326023-v1.0.15.json
---

## 목표와 완료 기준

동네를 돌아다니며 물자를 수색하고 선택 구조 이벤트를 수행하는 생존 루프를 구현합니다. 주변 채팅·이름표·말풍선도 추가합니다. 사용자 추가 요청에 따라 필수 군복/생존 장비12종과 실제 장착/교체도 구현합니다. 추가 요청: 생존 이동과 무기 사용에 필요한 authored 애니메이션 및 실제 행동 연동.

완료 기준: 파밍 장소의 실제 접근성, 물자 지원→NPC 호위→구호소 인계/중복 보상 방지, 주기 허기/갈증/활력/피로/스트레스와 휴식/아이템 효과, 저장/재접속/월드 격리, 실제 웹 UI·이동·상태 변화·기존 회귀/12MiB·로컬 적용·실제 화면/공식 보고 배포를 검증합니다. 실제 두 클라이언트의 이름과 말풍선·주변 채팅을 검증합니다. 필수12장비의 편집 원본·장착/해제·배낭 용량/방어·손전등·군복/NPC/다른 플레이어 외형·파밍/판매/저장과 웹 경량을 실제 검증합니다. 점프/착지·숙이기·검증 구조물 넘기/오르기·사격/장전/근접/사용의 클립/서버 상태와 실제 실행을 확인합니다.

최초 1회차 · 순차 진행 · 전체 12개 · 이번 실행 T01, T02, T03, T04, T06, T07, T08, T09, T10, T11, T12, T05

## 순차 작업 메뉴얼

| 순서 | 작업 | 실행 절차 | 완료 기준 | QA 절차 | 상태 |
| --- | --- | --- | --- | --- | --- |
| T01 | 생존 피로·스트레스 | 주기 생존 및 회복·상한·행동 제한을 설정 기반으로 구현하고 기존 상태를 호환합니다. | 온라인 변화·휴식/소비·제한·비접속 정책이 검증됩니다. | 도메인 시간 경과·임계값·복원 테스트 | 완료 |
| T02 | 동네 파밍·구조 서버 | 실제 도로의 파밍 지점과 구조 이벤트·소유권·물자/호위·인계/보상·DB를 구현합니다. | 접근 가능한 장소, 이벤트 주기/격리·중복/실패/재시작이 검증됩니다. | 지리/게임 규칙·실제 PostGIS/WS 검사 | 완료 |
| T03 | TPS 구조/생존 UI·채팅 이름표/말풍선 | 피난민·조난민·목적지·상태 패널·피로/스트레스와 지도 안내를 연결합니다. 주변 채팅에 서버 기준 이름/발신자와 머리 위 이름표/말풍선을 연결합니다. | 실제 웹에서 수색·지원·호위·인계·수치 확인이 가능합니다. 실제 두 클라이언트에서 이름·송수신·말풍선/만료가 확인됩니다. | Unity 빌드·실제 헤드리스 웹 입력/캡처 두 클라이언트 입력/서버 범위·이름 위조/중복 검사. | 완료 |
| T04 | 회귀·경량·기본 적용 | 회귀·상태 저장·필요 부하·12MiB와 기본 localhost 적용을 검증합니다. | 새 기능과 기존 전투/생존이 실행되고 기본 웹에서 확인됩니다. | 전체 서버/실제 Web·압축·로컬 기동 | 완료 |
| T06 | 필수 장비12종 경량 에셋 | 군복/신발/장갑/헬멧/조끼/가방2종/벨트/수통/구급파우치/손전등을 기존 리그/재질에 맞춰 제작합니다. | 편집 가능한 Blender 원본과 실사용 경량 에셋12종의 형태/결합이 확인됩니다. | Blender export·에셋 속성·Unity 실제 장착 렌더링 | 완료 |
| T07 | 서버 장착·용량/방어·파밍 | 장비 소유/슬롯/장착·해제·배낭 용량/방어와 파밍/상점·저장/중복을 연결합니다. | 소유/슬롯/무게/판매·저장/재접속과 실제 효과가 서버 기준으로 검증됩니다. | 게임/DB/WS 실제 효과·실패/중복 검사 | 완료 |
| T08 | 장착 UI·군복/NPC·손전등·회귀 | 배낭 장착/교체/해제, 로컬/다른 플레이어/NPC 외형, 손전등과 웹 경량·기존 회귀를 검증/적용합니다. | 실제 장착/외형·배낭·방어·빛·동기화·12MiB와 기본 로컬 동작이 확인됩니다. | 실제 웹 두 클라이언트/장비 장면·전체 회귀/로컬 적용 | 완료 |
| T09 | 서버 이동 행동·검증 구조물 | 점프/착지·숙이기/숙인 이동·넘기/오르기를 에너지/시간/높이/착지점 검증과 연결합니다. | 승인된 구조물과 일반 이동의 서버 상태/충돌/재접속이 검증됩니다. | 이동·임계값·중복/실패·실제 구조물 통과 테스트 | 완료 |
| T10 | 공용 리그 Blender 행동 클립 | 점프/착지·숙인 대기/걷기·넘기/오르기·소총/발사기·장전/근접/사용 클립을 제작합니다. | 편집 가능한 원본과 기존 리그에서 실제 관절이 움직이는 클립이 존재합니다. | Blender export·Unity 클립/본 동작/실제 프레임 검사 | 완료 |
| T11 | 무기·이동 애니/입력 연동 | 서버 행동/사격/장전 상태에 맞춰 전신/상체 클립과 장비·무기를 연결합니다. | 실제 점프/숙이기/넘기/오르기·사격/장전/근접/물품 사용이 표시됩니다. | 실제 웹 입력·서버 상태/본/장비/두 클라이언트 검사 | 완료 |
| T12 | 전체 행동·장비·기존 기능 회귀/적용 | 이동/무기·장비·구조/생존/채팅과 웹 경량을 회귀 검증하고 기본 로컬에 적용합니다. | 현재 전체 동작·저장·경량·기본 실행을 확인합니다. | 전체 서버/DB/WS·실제 Web/네이티브·12MiB·기본 로컬 | 완료 |
| T05 | 공식 보고·최종 QA | 실제 화면·검증 근거를 공식 v2 보고서와 완료 메뉴얼로 기록하고 게시합니다. 추가 장비12종의 실제 장착/용량/방어/빛/경량 검증도 보고합니다. 점프/착지·숙인 이동·넘기/오르기·무기 사용 클립과 서버 행동도 전체 결과에 포함합니다. | 전체 현재 버전 최종 QA·HTML/이미지/목록·외부 배포가 확인됩니다. 장비 추가를 포함한 전체8개 작업의 최종QA가 확인됩니다. 새 이동/무기 행동까지 현재12개 작업의 QA를 확인합니다. | 공식 생성/검사·레이아웃·Actions/HTTP200 | 완료 |

## QA 반복 기록

### Q01 · step · 2026-10-30T16:55:59.848Z

변경된 감소율에서 고정 HP 기대값 불일치

- T01 r1 · fail: 허기/갈증 감소율을0.8/1.2분당으로 바꾼 뒤, 기존 테스트의 고정HP94.6 기대값이 실패했습니다. 현재 설정의 0통과 시각과 피해율로 기대값을 계산하도록 수정하고 재검증했습니다. · 근거: docs/qa/rescue/attempt-needs-01.txt
### Q02 · step · 2026-10-30T16:56:00.331Z

실제63개 서버/DB/WS 회귀 통과

- T01 r1 · pass: 실제 주기 허기/갈증·피로/스트레스·휴식/차 음료·활력 상한/달리기 제한·비접속·과거 저장 호환과 기존 생존 테스트가 통과했습니다. · 근거: docs/qa/rescue/server-tests.json; server/test/rescue-survival.test.ts
- T02 r1 · pass: 18개 실제 도로 파밍 장소·구조 물자/소유권·빵부스러기 호위·인계/보상·주기/격리 및 실제 PostGIS 중복1회/실패롤백/재시작을 통과했습니다. 전체63개 실패0/건너뜀0. DB 장소 저장을 묶어100접속 p95 76.49ms/오류0도 확인했습니다. · 근거: docs/qa/rescue/postgres.json; server-tests.json; docs/qa/load/sustained-100.json
### Q03 · step · 2026-10-30T17:32:59.392Z

실제 Web 한국어 입력 실패 확인

- T03 r2 · fail: 기존 IMGUI에서 한국어 문장이 ?만 전송되어 실제 양방향 채팅 검사가 실패했습니다. 실제 화면/메시지와 Unity6000.0 공식 IME 제한을 대조하고 HTML 입력/포커스/조합 Enter로 수정했습니다. · 근거: docs/qa/rescue/attempt-web-01; browser/result-social.json
### Q04 · step · 2026-10-30T17:33:02.536Z

호위 간격 조정 후 초기 경로 역행 재현

- T02 r1 · fail: NPC가 카메라 가까이에 머물러 간격을 모든 구간에 적용한 뒤, 실제 시간 경과 테스트에서 초기 경로를 역행해 이동 기대값이 실패했습니다. 사선이 열린 경로점만 제한적으로 건너뛰게 수정하여 벽 차단/간격을 재검증했습니다. · 근거: docs/qa/rescue/attempt-follow-gap-02.txt; attempt-follow-gap-01; server/test/rescue-survival.test.ts
### Q05 · step · 2026-10-30T17:33:02.856Z

수정 후65개와 실제 Web20개 확인

- T01 r1 · pass: 현재 설정의 허기/갈증·피로/스트레스·휴식/차·피격 스트레스/100상한과 과거 저장·비접속 정책을 포함한 전체65개 실패0/건너뜀0. · 근거: docs/qa/rescue/server-tests.json
- T02 r1 · pass: 현재 호위 간격/경로와 실제 건물 관통 방지·물자/보상 중복·PostGIS 늦은 실패 롤백/재시작·주기/월드 격리를 통과했습니다. 실제 구호소 경로의 일반 입력 시뮬레이션625m도 성공했고 실시간 웹 경로 검증 중입니다. · 근거: docs/qa/rescue/postgres.json; route-simulation.json; server-tests.json
- T03 r2 · pass: 최신 Web20개 검사 통과:18개 파밍·피난민 물자/재접속/호위/인계·조난민 의료/중단·주기 생존/휴식/차·이름표/한글 양방향 채팅/조합 Enter/말풍선 만료/이름 생성·실제 NPC3·엔진 오류0. · 근거: docs/qa/rescue/browser/result.json; browser/*.png
### Q06 · step · 2026-10-30T17:33:03.177Z

기존 회귀 수색 fixture 공유 실패 조사

- T04 r1 · fail: 기존 Native와 Web이 같은 캐릭터 fixture를 사용한 회귀에서 수색 응답 대기가 실패했습니다. DB 거리0m와 쿨다운 종료17:11:12.719Z가 요청 추정17:11:06.908Z보다 늦음을 확인했습니다. Native 종료 후 새 스키마와 초기 상태 검사를 사용해 기존 Web3개 회귀를 통과했습니다. · 근거: docs/qa/rescue/attempt-regression-01.json; regression-fixture-probe.json; regression/optimized-browser.json
### Q07 · regression · 2026-10-30T17:46:05.378Z

구조/채팅 기존 범위의 실제 구호소·기본 웹 적용 확인

- T04 r1 · pass: 실제625.0m 구호소 경로406초·휴식1회·NPC 인계거리4.35m·순간이동/시간가속0·오류0. 전체65개·기존Web전투14/생존3·Windows4/5·100접속·12MiB와 기본8788의18장소/3이벤트/한글이름/채팅·엔진오류0을 확인했습니다. · 근거: docs/qa/rescue/full-route.json; live.json; server-tests.json; docs/qa/combat/browser/combat.json
### Q08 · step · 2026-10-30T18:18:06.486Z

필수 장비 실제 렌더·서버/DB·웹 장착과 로컬 적용

- T06 r1 · pass: Blender5.2.3 편집 원본/11개 신규 FBX·기존 조끼/스킨 군복 구성. 실제 Unity14개 부착·헬멧/부츠/가방 좌표·앞뒤 렌더를 확인했습니다. · 근거: art/EssentialEquipment.blend; art/essential-equipment-export.json; docs/qa/equipment/runtime/equipment.json; front.png; back.png
- T07 r1 · pass: 전체74개 서버/실제 PostGIS 테스트 통과. 장착 중복/슬롯·용량·방어 실피해·판매·손전등·늦은 실패 롤백·재시작을 검증했습니다. · 근거: server/test/equipment.test.ts; equipment-postgres.test.ts; .runtime/equipment-server-tests.log
- T08 r1 · pass: 헤드리스 실제 웹9개 검사:12카탈로그/11장착/32kg·30%방어/가방26kg교체/손전등on-off/재접속/다른 플레이어 장비/엔진오류0. Web압축10,702,490바이트<12MiB. 기본8788 적용·소유 서버 저장 재시작 완료. 전체 최종 회귀는 T12에서 추가 수행합니다. · 근거: docs/qa/equipment/browser/result.json; docs/qa/equipment/runtime/front.png; .runtime/local-processes.json
### Q09 · step · 2026-10-30T18:24:12.468Z

서버 이동 행동·장전 및 실제 DB 재시작 검증

- T09 r1 · pass: 전체82개 통과. 점프 시간/비용/중복·숙인 속도·검증 방벽/발판 실제 지도 접근성·충돌/넘기/오르기/내려오기·저장/오프라인 완료·장전 지연/공격 제한·PostGIS 영수증 중복/재시작 자세 보존을 실행했습니다. 초기 DB 테스트의 동시 소유/중복 close 오류는 테스트 수명 관리를 수정해 재검증했습니다. · 근거: server/test/actions.test.ts; actions-postgres.test.ts; .runtime/actions-server-tests.log
### Q10 · step · 2026-10-30T18:32:59.174Z

Blender 행동10개·Unity 본 바인딩·실제 런타임 렌더

- T10 r1 · pass: SurvivalActions.blend의10개 편집 클립을 Unity로 내보냈습니다. 실제 캐릭터에10클립 모두 시간별 관절 변화가 확인되고, 장비14개/숙인 머리높이0.419m 감소·발높이0.041m를 포함한23개 런타임 검사가 통과했습니다. 원본 곡선은 있었으나 애니메이션 전용 FBX가 CharacterArmature 경로를 생략한 바인딩 실패를 동일 계층 export로 수정했습니다. 비어 있는 중첩 JSON으로 인한 초기 런타임 오류도 수정 후 예외0으로 재검증했습니다. · 근거: art/SurvivalActions.blend; art/survival-actions-export.json; docs/qa/actions/clip-import.txt; docs/qa/actions/runtime/actions.json; .runtime/actions-native.log
### Q11 · regression · 2026-10-30T18:42:01.737Z

수정 버전2 이동 곡선·클립·실제 웹 행동14개

- T09 r2 · pass: 전체82개 서버/실제 DB 통과. 넘기 루트는0.48m 상승+클립 발 들기, 오르기는 높이를 먼저 확보한 뒤 수평 진입하도록 수정했습니다. 지리/접근/에너지/저장/중복·NPC 장애물 경로와 현재 Web 실제 착지/플랫폼1.4m가 통과했습니다. · 근거: server/test/actions.test.ts; .runtime/actions-server-tests.log; docs/qa/actions/browser/result.json
- T10 r2 · pass: 10개 클립의 공용 리그 바인딩·23개 Native 검사와 예외0을 재확인했습니다. 넘기 상체 기울임/발 들기, 오르기 끌어올리기, 무기 손가락 그립을 보완하고 실제 렌더를 확인했습니다. · 근거: docs/qa/actions/clip-import.txt; runtime/actions.json; runtime/Vault.png; runtime/RifleFire.png
- T11 r1 · pass: 실제 Web14개 통과: 점프/착지·숙인 이동/재접속·방벽/발판 실제 애니메이터/높이·서버 장전/소총·발사기 탄약과 사격 클립·소비/근접 피해와 클립·다른 플레이어 동기화·엔진 오류0. 조작 안내와 넘기/오르기 중 무기 수납도 적용했습니다. · 근거: docs/qa/actions/browser/result.json; jump.png; crouch.png; vault.png; climb.png; reload.png; melee.png
### Q12 · regression · 2026-10-30T18:54:41.413Z

최종 적용·전체 회귀27개 근거 게이트

- T12 r1 · pass: 현재 서버82개·Web 구조20/장비9/행동14/전투14/기존 수색·거래·치료3·Native 전투4/생존5/장비/10클립·실제 양방향 구조물2장면이 통과했습니다.100접속92초9900명령900응답·이동80/재접속10·p95 66.98ms/오류0. 초기11,020,608바이트<12MiB, 기본8788 파일 해시 동일·실제 새 월드 구조3/장비12/구조물2·한글/점프/숙이기·엔진 오류0. 실패는 별도 보존했습니다. · 근거: docs/qa/actions/final-integration.json; docs/qa/rescue/live.json; docs/qa/load/sustained-100.json; docs/qa/actions/payload-budget.json
### Q13 · final · 2026-10-30T19:03:25.871Z

12개 현재 작업 버전의 실제 게임/서버/DB/렌더·로컬 적용·외부 HTML/게임 이미지 대조 완료

- T01 r1 · pass: 주기 허기/갈증·휴식/차·피로/스트레스·활력 상한/달리기 제한: 현재 서버82개와 실제 웹 주기/회복 검사가 통과했습니다. · 근거: docs/qa/rescue/server-tests.json; docs/qa/rescue/browser/result.json
- T02 r1 · pass: 현재 실제 구조 지원/호위/인계·저장/중복 테스트 통과. 실제 구호소625.02m/406.24초·NPC 인계4.35m·순간이동0/시간가속0 경로를 완료했습니다. · 근거: docs/qa/rescue/full-route.json; docs/qa/rescue/browser/result.json; server/test/rescue-postgres.test.ts
- T03 r2 · pass: 현재 Web20개:18파밍·피난민/조난민·한글 양방향 채팅/이름표/말풍선/만료/조합 Enter·이름 생성·모델3·엔진 오류0을 확인했습니다. · 근거: docs/qa/rescue/browser/result.json
- T04 r1 · pass: 현재 Native 전투4/생존5와 Web 전투14·기존수색/거래/치료3, 현재 기본8788의구조3/한글과 회귀를 통과했습니다. · 근거: docs/qa/actions/final-integration.json; docs/qa/rescue/regression/optimized-browser.json
- T06 r1 · pass: Blender 편집 원본과 신규11 FBX·기존군복/조끼를 조합한 필수12장비,14개 실제 부착·앞뒤 렌더/헬멧·부츠·가방 좌표를 확인했습니다. · 근거: art/EssentialEquipment.blend; docs/qa/equipment/runtime/equipment.json
- T07 r1 · pass: 실제 PostGIS 장비 중복1회·늦은 실패 롤백·재시작 슬롯/용량/조명, 소유/과적/판매·방어 실피해를 통과했습니다. · 근거: docs/qa/equipment/postgres.json; server/test/equipment.test.ts
- T08 r1 · pass: 현재 Web9개:12카탈로그·11부위장착·32kg/30%방어·가방26kg교체·손전등on/off·재접속·다른 플레이어 장비·엔진 오류0. 기본8788에도 적용했습니다. · 근거: docs/qa/equipment/browser/result.json; docs/qa/rescue/live.json
- T09 r2 · pass: 수정버전2 서버시간/높이/접근·점프비용·숙인속도·방벽충돌/넘기·발판선상승후진입/높이1.4m·저장/중복/장전, NPC 장애물 경로를 현재82개 테스트와 Web14개로 확인했습니다. · 근거: server/test/actions.test.ts; actions-postgres.test.ts; docs/qa/actions/browser/result.json
- T10 r2 · pass: 수정버전2 공용리그10클립 본 변화·실제 Native23검사·장비14부착·숙인머리0.419m/발0.041m, 몸숙임/발들기/무기손가락과 렌더를 확인했습니다. · 근거: art/SurvivalActions.blend; docs/qa/actions/clip-import.txt; docs/qa/actions/runtime/actions.json
- T11 r1 · pass: 현재 Web14개 실제 입력/서버/애니메이터·소총/발사기·장전·근접피해·소비·원격행동, 양방향 실제traverse 시선1.0·엔진 예외0을 확인했습니다. · 근거: docs/qa/actions/browser/result.json; docs/qa/actions/scene/vault.json; docs/qa/actions/scene/climb.json
- T12 r1 · pass: 현재 실제 근거27개 통합게이트 통과.100접속92초/9900명령/900응답/이동80/재접속10/p95 66.98ms/오류0.11,020,608bytes<12MiB·기본8788 파일해시동일·새월드 구조3/장비12/구조물2/점프/숙이기/한글/엔진오류0. · 근거: docs/qa/actions/final-integration.json; docs/qa/load/sustained-100.json; docs/qa/rescue/live.json
- T05 r3 · pass: 공식v2 결과1건/해결버그2건·순차버전/실패이력 보존·목록/링크428문서14877링크검사·실제게임이미지10개(각200KB미만)/1600·390레이아웃통과. 외부 고유HTML200·내용대조·이미지10개바이트동일·메인링크를 확인했습니다. Actions API표시는 queued로 별도기록하며 CI성공으로 선언하지 않습니다. · 근거: docs/qa/rescue/final-deployment.json; docs/qa/rescue/report-layout-final.json; docs/qa/rescue/image-compression-final.json

## 남은 작업과 다음 반복

현재 요청의 구현/로컬 적용/실행 검증/보고서 외부 제공을 완료했습니다. 모든 건물 자유 등반·정수/내구도·모바일/저사양·100 Unity 동시 렌더링/인터넷 운영은 후속 범위입니다. Actions API 상태 표시는 실제 공개 HTML/이미지 검증과 구분해 기록했습니다.

이전 회차: [메뉴얼 열기](../../../projects/last37n/reports/2026-10-30-manual-rescue-survival-neighborhood-20261030185441628-v1.0.14.html)

## 작업 시간

- 제안 시간: 최초60~100분(01:35 KST 시작). 장비 추가35~60분, 이후 이동/무기 애니메이션 추가40~70분 예상. 사용자 추가 범위 포함 전체 약150~230분 범위.
- 실제로 완료된 시간: 기록 2026-10-31 04:03:25 KST. 시작2026-10-31 01:35:00 KST.
- XERJ: 미사용 — XERJ 도구 미제공, rg·현재 소스·실제 실행으로 대체.
