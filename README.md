# Codex 전역 지침

여러 Windows PC에서 사용하는 전역 AGENTS.md와 동기화 스크립트입니다. 인증값·세션·config.toml은 포함하지 않습니다.

## 새 PC 연결

PowerShell에서 실행합니다. Git 및 이 비공개 저장소의 접근 권한이 필요합니다.

```powershell
$codexDirectory = if ($env:CODEX_HOME) { $env:CODEX_HOME } else { Join-Path $env:USERPROFILE '.codex' }
git clone https://github.com/MDJ0126/codex-config.git (Join-Path $codexDirectory 'config-repo')
& (Join-Path $codexDirectory 'config-repo/Sync.ps1') -Direction Pull
```

기존 전역 파일이 저장소와 다르면 안전을 위해 중단합니다. 기존 지침을 검토해 합치거나 별도로 보관한 뒤 설치하세요. 설치된 지침은 덮어쓰기 전에 instruction-backups에 백업합니다.

## 수정 및 푸시

```powershell
& "$HOME/.codex/config-repo/Sync.ps1" -Direction Push -Message 'Update global instructions'
```

CODEX_HOME을 별도로 사용하는 경우 해당 경로에서 스크립트를 실행하세요. Codex는 전역 지침 수정 시 검토·커밋·푸시하도록 AGENTS.md에 지시되어 있습니다. 상주 감시나 예약 작업은 설치하지 않습니다. 다른 PC에서는 Pull을 실행해야 변경 사항이 적용됩니다. 이미 열린 대화에 즉시 다시 로드되는 것은 보장하지 않으므로 새 대화에서 지침을 확인하세요.

Pull은 fast-forward만 허용하며 로컬 변경을 보존합니다. Push는 지정된 네 파일만 추가하고 강제 푸시하지 않습니다. 동시 수정으로 푸시가 거부되면 변경을 보존한 채 수동으로 병합하세요.
