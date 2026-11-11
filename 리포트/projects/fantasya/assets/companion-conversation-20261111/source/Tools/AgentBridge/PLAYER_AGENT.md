# Fantasya PC 플레이 에이전트

이 문서는 코드 작업이 아니라 실제 게임 동료 참여 지침입니다. 사용자가 참여를 요청한 경우에만 제어권을 받습니다. MCP 서버 이름은 `fantasya`입니다.

1. `list_companions`, `observe_companion`으로 ID·성격·기억·약속·online과 실제 game 상태를 읽습니다. 없는 기억이나 실행 결과를 만들어 말하지 않습니다.
2. 요청받은 동료를 `claim_companion`으로 5~120초 맡습니다. 다른 에이전트의 제어권을 빼앗지 않습니다. 여러 에이전트는 서로 다른 동료 ID를 선택합니다.
3. `wait_for_companion_events`로 최대 20초씩 게임의 대화를 받습니다. `pendingPlayerMessages`와 기억을 읽고 **자신의 모델로** 답하여 `say_as_companion`을 호출합니다. 응답한 마지막 메시지 ID를 `replyToMessageId`로 전달합니다. 나중에 들어온 메시지는 다음에 답합니다. 플레이어 발언용 `talk_to_companion`을 자기 답변에 사용하지 않습니다.
4. 실제 관측 안에서 `act_companion`으로 follow / guard / hold / retreat / attack 중 선택합니다. 공격 targetId는 지금 관측된 대상만 사용합니다. 행동은 1~6초입니다. 지속 약속과 긴급 생존 대응이 우선입니다.
5. 반환된 queued는 접수입니다. `observe_companion`의 실제 `game.receipt`가 commandId를 포함해야 적용 확인입니다. 만료·거절·오프라인을 성공으로 말하지 않습니다.
6. 사용자만 요청한 약속·기억·성격 변경을 저장합니다. 유리한 행동을 위해 약속을 임의 해제하지 않습니다.
7. 참여를 계속하는 동안 제어권을 만료 전에 갱신합니다. 끝나면 `release_companion`합니다. 만료되면 로컬 모델로 돌아갑니다. 대기 중인 외부 메시지는 연결 종료를 기록하고 최근 대화에 남습니다.

자체 모델을 실행하는 PC 에이전트의 MCP 호출이므로 게임용 별도 유료 API 키는 필요하지 않습니다. 호스트 앱의 기존 계정·승인·요금이 적용됩니다. 앱이나 대화를 종료하면 자동 상시 플레이가 계속되는 것은 아닙니다.

설정: 프로젝트 루트에서 `node Tools/AgentBridge/configure.mjs codex` 또는 `claude` 또는 `cursor`. 그 뒤 앱에서 MCP를 새로 연결하고 이 문서를 읽도록 요청합니다. 표준 stdio를 지원하는 앱은 Node 실행 파일 + `Tools/AgentBridge/mcp.mjs` 절대 경로를 등록할 수 있습니다. 이 PC 전용 경로·개인 기억·토큰을 공개 저장소에 넣지 않습니다.
