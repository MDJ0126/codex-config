param([switch]$SkipSync,[switch]$CheckOnly,[switch]$Activate)
$ErrorActionPreference='Stop'
if(-not $SkipSync){
    $changes=& git -C $PSScriptRoot status --porcelain
    if($LASTEXITCODE -ne 0){throw 'Cannot inspect configuration repository.'}
    if(-not $changes){
        & git -C $PSScriptRoot pull --ff-only
        if($LASTEXITCODE -ne 0){throw 'Configuration sync failed; local files preserved.'}
        if(-not $CheckOnly){& (Join-Path $PSScriptRoot 'Sync.ps1') -Direction Pull}
    }else{Write-Warning 'Local configuration changes preserved; using current checkout without pulling.'}
}
. (Join-Path $PSScriptRoot 'Resolve-LineNode.ps1')
$node=Resolve-LineNode
$settings=Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'line-notification.private.json') | ConvertFrom-Json
if(-not $settings.ChannelAccessToken -or -not $settings.ChannelSecret -or $settings.UserId -cnotmatch '^U[0-9a-f]{32}$'){
    throw 'Shared LINE settings are incomplete. Synchronize the private repository; do not print credential values.'
}
if($CheckOnly){Write-Output 'LINE_BOOTSTRAP_CHECK_PASSED: bundled runtime and all shared credentials available.';return}
$codexRoot=if($env:CODEX_HOME){$env:CODEX_HOME}else{Join-Path $env:USERPROFILE '.codex'}
$state=Join-Path $codexRoot 'line-bridge'
New-Item -ItemType Directory -Force -Path $state | Out-Null
if($env:CODEX_APP_TOOLS_PIPE_PATH -and $env:CODEX_THREAD_ID){
    @{pipePath=$env:CODEX_APP_TOOLS_PIPE_PATH;callerThreadId=$env:CODEX_THREAD_ID} | ConvertTo-Json -Compress | Set-Content -LiteralPath (Join-Path $state 'app-context.json') -Encoding utf8
}
$healthy=$false
try{$healthy=(Invoke-RestMethod -Uri 'http://127.0.0.1:17843/health' -TimeoutSec 2) -eq 'ok'}catch{}
if(-not $healthy){& (Join-Path $PSScriptRoot 'Start-LineBridge.ps1');return}
if($Activate -and (Test-Path -LiteralPath (Join-Path $state 'endpoint.txt'))){
    $endpoint=(Get-Content -Raw -LiteralPath (Join-Path $state 'endpoint.txt')).Trim()
    $headers=@{Authorization='Bearer '+$settings.ChannelAccessToken}
    try{
        $current=Invoke-RestMethod -Uri 'https://api.line.me/v2/bot/channel/webhook/endpoint' -Headers $headers -TimeoutSec 15
        if($current.endpoint -ne $endpoint){
            $null=Invoke-RestMethod -Method Put -Uri 'https://api.line.me/v2/bot/channel/webhook/endpoint' -Headers $headers -ContentType 'application/json' -Body (@{endpoint=$endpoint}|ConvertTo-Json) -TimeoutSec 15
        }
    }catch{throw 'Webhook activation failed; credentials omitted.'}
}
Write-Output 'LINE_BOOTSTRAP_READY: receiver already running on this PC.'
