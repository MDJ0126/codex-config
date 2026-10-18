# Codex 전역 지침

여러 Windows PC에서 사용하는 전역 AGENTS.md와 동기화 스크립트입니다. 세션·config.toml은 포함하지 않습니다. 사용자 승인된 LINE 전용 인증값은 비공개 설정 파일에 포함되므로 출력하거나 공개 저장소로 복사하지 마세요.

## 새 PC 연결

### 이 README를 읽는 Codex에 대한 설치 지시

사용자가 이 저장소 주소를 알려주고 설정을 요청하면, 이 README를 읽은 Codex가 아래 작업을 직접 끝까지 수행하세요. 사용자에게 로컬 경로나 실행 명령을 다시 지정하도록 요구하지 마세요. Git 및 이 비공개 저장소의 접근 인증이 필요합니다.

1. `CODEX_HOME`이 설정되어 있으면 해당 디렉터리를, 없으면 현재 Windows 사용자 홈의 `.codex`를 설정 루트로 사용하세요. 다른 PC의 사용자 이름이나 `C:\Users\ehdwn`을 그대로 사용하지 마세요.
2. 저장소는 반드시 설정 루트 아래 `config-repo`에 복제하세요. 이미 있으면 원격 주소와 로컬 변경을 확인하고 기존 체크아웃을 재사용하세요. 다른 원격 저장소나 충돌하는 파일을 덮어쓰지 마세요.
3. 해당 저장소의 `Sync.ps1 -Direction Pull`을 실행하여 전역 지침을 설치하고, `Initialize-LineBridge.ps1 -Activate`를 실행하여 LINE 수신 초기화까지 진행하세요. 실행 환경의 권한 승인이 필요하면 정상 승인 절차를 따르세요.
4. Node를 찾지 못하면 Codex의 `load_workspace_dependencies`로 번들 런타임을 확인하고 재시도하세요. 인증정보를 출력하지 말고, 설치 경로·지침 동기화·초기화의 실제 결과를 검증하여 보고하세요. 실패하면 오류를 확인하고 기존 파일을 보존하세요.

새 PC에서 Codex에 전달할 요청은 다음 한 문장이면 됩니다.

> https://github.com/MDJ0126/codex-config 의 README를 읽고 이 PC에 설정해 주세요.

아래는 저장소가 아직 없는 PC에서 수행할 PowerShell 절차입니다. 기존 체크아웃이 있으면 `git clone`을 반복하지 말고 위 지시에 따라 확인 후 재사용하세요.

```powershell
$codexDirectory = if ($env:CODEX_HOME) { $env:CODEX_HOME } else { Join-Path $env:USERPROFILE '.codex' }
New-Item -ItemType Directory -Force -Path $codexDirectory | Out-Null
git clone https://github.com/MDJ0126/codex-config.git (Join-Path $codexDirectory 'config-repo')
if ($LASTEXITCODE -ne 0) { throw 'Repository clone failed.' }
& (Join-Path $codexDirectory 'config-repo/Sync.ps1') -Direction Pull
& (Join-Path $codexDirectory 'config-repo/Initialize-LineBridge.ps1') -Activate
```

기존 전역 파일이 저장소와 다르면 안전을 위해 중단합니다. 기존 지침을 검토해 합치거나 별도로 보관한 뒤 설치하세요. 설치된 지침은 덮어쓰기 전에 instruction-backups에 백업합니다.

## 수정 및 푸시

```powershell
& "$HOME/.codex/config-repo/Sync.ps1" -Direction Push -Message 'Update global instructions'
```

CODEX_HOME을 별도로 사용하는 경우 해당 경로에서 스크립트를 실행하세요. Codex는 전역 지침 수정 시 검토·커밋·푸시하도록 AGENTS.md에 지시되어 있습니다. 상주 감시나 예약 작업은 설치하지 않습니다. 다른 PC에서는 Pull을 실행해야 변경 사항이 적용됩니다. 이미 열린 대화에 즉시 다시 로드되는 것은 보장하지 않으므로 새 대화에서 지침을 확인하세요.

Pull은 fast-forward만 허용하며 로컬 변경을 보존합니다. Push는 지정된 네 파일만 추가하고 강제 푸시하지 않습니다. 동시 수정으로 푸시가 거부되면 변경을 보존한 채 수동으로 병합하세요.

## LINE 양방향 연결

현재 구현은 LINE 알림 답장의 원본 메시지 ID를 이 PC의 세션 ID와 연결합니다. 일반 메시지는 이 PC에서 마지막으로 알림을 보낸 세션으로 전달합니다. 모르는 답장 원본, 다른 PC 알림, 로컬에 없는/보관된 세션에는 대응하지 않으며 다른 세션으로 대체하지 않습니다. 설치 이후 보낸 알림부터 답장 매핑이 생깁니다.

- 답장: 원하는 알림을 LINE에서 길게 누르고 답장을 선택해 요청합니다.
- 일반 메시지: 이 PC의 가장 최근 등록 세션으로 요청합니다.
- 수신 PC는 한 번에 한 대입니다. PC를 바꾸면 해당 PC에서 아래 시작 스크립트를 실행합니다.
- PC가 꺼져 있거나 브리지가 중지되면 수신하지 않습니다. 자동 시작은 설치하지 않았습니다.

### 시작과 중지

Node.js 24 이상, Codex CLI 로그인, PowerShell 7, 저장소 접근이 필요합니다. 이 PC에는 구성되어 있습니다.

```powershell
& "$HOME/.codex/config-repo/Start-LineBridge.ps1"
& "$HOME/.codex/config-repo/Start-LineBridge.ps1" -Stop
```

액세스 토큰·Channel secret·수신자 ID는 사용자 명시 승인에 따라 비공개 line-notification.private.json에 함께 저장하여 동기화합니다. 별도 키·환경 변수 등록은 필요하지 않습니다.

시작 스크립트는 공식 cloudflared 바이너리를 내려받고 숨겨진 서버와 Quick Tunnel을 실행한 뒤 LINE Webhook URL을 등록합니다. LINE Developers에서 Use webhook을 한 번 켜야 합니다. Quick Tunnel은 무료 임시 주소이며 재시작 시 주소가 바뀝니다. 중지 시 Webhook URL을 자동으로 삭제하지 않습니다.

요청 접수 안내는 Reply API, 처리 결과는 Push API를 사용합니다. 처리 결과는 월 500건 한도에 포함됩니다. 원본 메시지 ID·세션 ID·중복 이벤트 상태는 로컬 SQLite에만 저장합니다. 로그에는 입력 본문과 토큰을 기록하지 않습니다.

기존 Codex 세션은 CLI로 재개합니다. 데스크톱의 열려 있는 화면에 즉시 반영되거나 이미 실행 중인 작업과 동시 쓰기가 가능한지는 보장하지 않습니다. 승인이나 실행 오류는 우회하지 않습니다. 실패하면 결과 알림에 조치 필요로 표시합니다.

### 데스크톱 전달 수정

데스크톱이 열린 세션에 `codex exec resume`을 사용하면 `already has an active writer`로 실패하는 문제가 실제 재현되어, 데스크톱의 기존 `send_message_to_thread` 도구 경로로 교체했습니다. 이전 CLI 재개·브리지 결과 대행 설명보다 이 항목을 우선합니다. 브리지는 전달 접수까지만 담당하고, 실제 작업과 LINE 결과 발송은 해당 데스크톱 세션이 처리합니다. Codex 앱은 실행 중이어야 합니다. 앱이 재시작되면 새 작업 알림 한 번으로 로컬 접속 정보를 갱신합니다. 이는 Codex 앱 내부 도구 프로토콜을 사용하므로 앱 업데이트 후 호환성 확인이 필요합니다.

### 다른 PC 자동 초기화

전역 지침이 설치된 Codex는 새 세션 시작 시 `Initialize-LineBridge.ps1 -Activate`를 실행하도록 지시되어 있습니다. 이 스크립트는 변경 사항이 없는 저장소를 fast-forward 동기화하고 번들 Node 탐색, 서버·터널 시작, Webhook 전환, 앱 접속 정보 설정을 수행합니다. 발송 스크립트도 중지된 수신 서버를 자동 기동합니다. 키는 공유 설정에 있으므로 수동 등록하지 않습니다.

이는 Codex가 지침을 실행할 때의 자동화입니다. Windows 로그인 작업이나 부팅 서비스는 설치하지 않았습니다. 다른 PC에는 최초 Git 인증과 전역 지침 동기화가 필요하며, 첫 Codex 세션부터 자동 초기화합니다. 여러 PC가 동시에 시작하면 마지막으로 초기화한 PC가 수신합니다. 네트워크나 권한 오류를 성공으로 처리하지 않습니다.

### 질문과 결과를 함께 표시
발송 스크립트는 필수 인수 Question에 실제 사용자 질문, Message에 결과를 받습니다. 알림은 💬 요청 / 📝 결과 두 구역으로 표시합니다. 긴 질문은 요약 여부를 표시하고 비밀값을 생략하며 전체 길이를 4500 이내로 유지합니다.

