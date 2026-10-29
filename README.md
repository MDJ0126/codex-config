# Codex 전역 지침

## 결과 보고서 사이트

`리포트/index.html`을 열면 날짜·버전별 보고서를 게시글 목록으로 볼 수 있습니다. 원본은 `리포트/posts/`의 Markdown이며 `node 리포트/build.mjs`로 HTML을 갱신합니다. 작성 규칙은 [리포트/README.md](리포트/README.md)를 참고하세요. 사이트는 로컬 파일로 제공되며 인터넷에 배포한 상태는 아닙니다.

여러 Windows PC에서 사용하는 전역 AGENTS.md와 동기화 스크립트입니다. 세션·config.toml은 포함하지 않습니다.

## 새 PC 연결

### 이 README를 읽는 Codex에 대한 설치 지시

사용자가 이 저장소 주소를 알려주고 설정을 요청하면, 이 README를 읽은 Codex가 아래 작업을 직접 끝까지 수행하세요. 사용자에게 로컬 경로나 실행 명령을 다시 지정하도록 요구하지 마세요. Git 및 이 비공개 저장소의 접근 인증이 필요합니다.

1. `CODEX_HOME`이 설정되어 있으면 해당 디렉터리를, 없으면 현재 Windows 사용자 홈의 `.codex`를 설정 루트로 사용하세요. 다른 PC의 사용자 이름이나 `C:\Users\ehdwn`을 그대로 사용하지 마세요.
2. 저장소는 반드시 설정 루트 아래 `config-repo`에 복제하세요. 이미 있으면 원격 주소와 로컬 변경을 확인하고 기존 체크아웃을 재사용하세요. 다른 원격 저장소나 충돌하는 파일을 덮어쓰지 마세요.
3. 해당 저장소의 `Sync.ps1 -Direction Pull`을 실행하여 전역 지침을 설치하세요. 실행 환경의 권한 승인이 필요하면 정상 승인 절차를 따르세요.
4. 인증정보를 출력하지 말고, 설치 경로와 지침 동기화의 실제 결과를 검증하여 보고하세요. 실패하면 오류를 확인하고 기존 파일을 보존하세요.

새 PC에서 Codex에 전달할 요청은 다음 한 문장이면 됩니다.

> https://github.com/MDJ0126/codex-config 의 README를 읽고 이 PC에 설정해 주세요.

아래는 저장소가 아직 없는 PC에서 수행할 PowerShell 절차입니다. 기존 체크아웃이 있으면 `git clone`을 반복하지 말고 위 지시에 따라 확인 후 재사용하세요.

```powershell
$codexDirectory = if ($env:CODEX_HOME) { $env:CODEX_HOME } else { Join-Path $env:USERPROFILE '.codex' }
New-Item -ItemType Directory -Force -Path $codexDirectory | Out-Null
git clone https://github.com/MDJ0126/codex-config.git (Join-Path $codexDirectory 'config-repo')
if ($LASTEXITCODE -ne 0) { throw 'Repository clone failed.' }
& (Join-Path $codexDirectory 'config-repo/Sync.ps1') -Direction Pull
```

기존 전역 파일이 저장소와 다르면 안전을 위해 중단합니다. 기존 지침을 검토해 합치거나 별도로 보관한 뒤 설치하세요. 설치된 지침은 덮어쓰기 전에 instruction-backups에 백업합니다.

## 수정 및 푸시

```powershell
& "$HOME/.codex/config-repo/Sync.ps1" -Direction Push -Message 'Update global instructions'
```

CODEX_HOME을 별도로 사용하는 경우 해당 경로에서 스크립트를 실행하세요. Codex는 전역 지침 수정 시 검토·커밋·푸시하도록 AGENTS.md에 지시되어 있습니다. 상주 감시나 예약 작업은 설치하지 않습니다. 다른 PC에서는 Pull을 실행해야 변경 사항이 적용됩니다. 이미 열린 대화에 즉시 다시 로드되는 것은 보장하지 않으므로 새 대화에서 지침을 확인하세요.

Pull은 fast-forward만 허용하며 로컬 변경을 보존합니다. Push는 지정된 네 파일만 추가하고 강제 푸시하지 않습니다. 동시 수정으로 푸시가 거부되면 변경을 보존한 채 수동으로 병합하세요.
