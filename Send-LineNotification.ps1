param(
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$SessionTitle,
    [Parameter(Mandatory=$true)][string]$Message,
    [string]$ProjectName,
    [string]$SessionId = $env:CODEX_THREAD_ID,
    [ValidateSet('완료','일부 완료','실패','조치 필요')][string]$Status = '완료',
    [switch]$DryRun
)
$ErrorActionPreference = 'Stop'
$SessionTitle = ($SessionTitle -replace '[\r\n]+', ' ').Trim()
if ([string]::IsNullOrWhiteSpace($SessionTitle)) { throw 'SessionTitle must not be blank.' }
if ([string]::IsNullOrWhiteSpace($Message)) { throw 'Message must not be blank.' }
$ProjectName = ($ProjectName -replace '[\r\n]+', ' ').Trim()
if ([string]::IsNullOrWhiteSpace($ProjectName)) { $ProjectName = '프로젝트 없는 대화' }
$statusIcons = @{ '완료' = '✅'; '일부 완료' = '🟡'; '실패' = '❌'; '조치 필요' = '🔔' }
$notificationTime = [DateTimeOffset]::UtcNow.ToOffset([TimeSpan]::FromHours(9)).ToString('yyyy-MM-dd HH:mm:ss')
$Message = @(
    ($statusIcons[$Status] + ' 작업 ' + $Status),
    ('📁 ' + $ProjectName),
    ('[' + $SessionTitle + ']'),
    '━━━━━━━━━━━━',
    $Message.Trim(),
    '━━━━━━━━━━━━',
    ('🕒 ' + $notificationTime + ' KST · 알림 시각')
) -join "`n"
# Require an editorial summary instead of silently cutting a result mid-sentence.
if ([string]::IsNullOrWhiteSpace($Message) -or $Message.Length -gt 4500) {
    throw 'Provide a nonempty summary of at most 4500 UTF-16 code units.'
}
if ($DryRun) {
    Write-Output 'DRY_RUN: message length valid; no API request made.'
    return
}
function Read-LineSetting([string]$Name) {
    $value = [Environment]::GetEnvironmentVariable($Name, 'Process')
    if ([string]::IsNullOrWhiteSpace($value)) {
        $value = [Environment]::GetEnvironmentVariable($Name, 'User')
    }
    return $value
}
$settingsPath = Join-Path $PSScriptRoot 'line-notification.private.json'
if (Test-Path -LiteralPath $settingsPath) {
    $settings = Get-Content -Raw -LiteralPath $settingsPath | ConvertFrom-Json
    $token = $settings.ChannelAccessToken
    $recipient = $settings.UserId
} else {
    $token = Read-LineSetting 'LINE_CHANNEL_ACCESS_TOKEN'
    $recipient = Read-LineSetting 'LINE_USER_ID'
}
if ([string]::IsNullOrWhiteSpace($token) -or $recipient -cnotmatch '^U[0-9a-f]{32}$') {
    throw 'LINE_NOT_CONFIGURED: set LINE_CHANNEL_ACCESS_TOKEN and LINE_USER_ID locally.'
}
$headers = @{ Authorization = 'Bearer ' + $token }
try {
    $quota = Invoke-RestMethod -Method Get -Uri 'https://api.line.me/v2/bot/message/quota' -Headers $headers -TimeoutSec 20
    $usage = Invoke-RestMethod -Method Get -Uri 'https://api.line.me/v2/bot/message/quota/consumption' -Headers $headers -TimeoutSec 20
    if ($quota.type -eq 'limited' -and $usage.totalUsage -ge $quota.value) {
        Write-Output 'NOT_SENT: monthly LINE message quota reached.'
        return
    }
    $body = @{ to = $recipient; messages = @(@{ type = 'text'; text = $Message }) } | ConvertTo-Json -Depth 5 -Compress
    $headers['X-Line-Retry-Key'] = [guid]::NewGuid().ToString()
    $pushResponse = Invoke-RestMethod -Method Post -Uri 'https://api.line.me/v2/bot/message/push' -Headers $headers -ContentType 'application/json; charset=utf-8' -Body ([Text.Encoding]::UTF8.GetBytes($body)) -TimeoutSec 20
    if ($SessionId -and (Test-Path -LiteralPath (Join-Path $PSScriptRoot 'bridge.mjs'))) {
        try {
            $receipt = @{threadId=$SessionId;title=$SessionTitle;project=$ProjectName;messageIds=@($pushResponse.sentMessages | ForEach-Object { $_.id })} | ConvertTo-Json -Compress
            . (Join-Path $PSScriptRoot 'Resolve-LineNode.ps1')
            $lineNode=Resolve-LineNode
            $receipt | & $lineNode (Join-Path $PSScriptRoot 'bridge.mjs') register
            if ($LASTEXITCODE -ne 0) { Write-Warning 'Notification sent, but reply route registration failed.' }
            if($env:CODEX_APP_TOOLS_PIPE_PATH){
                try{& (Join-Path $PSScriptRoot 'Initialize-LineBridge.ps1') -SkipSync}
                catch{Write-Warning 'Notification sent, but automatic receiver setup failed. Codex should inspect local bridge status.'}
            }
        } catch { Write-Warning 'Notification sent, but reply route registration failed.' }
    }
    Write-Output 'API_ACCEPTED: LINE accepted the push request; device delivery is unconfirmed.'
} catch {
    $status = 'unknown'
    if ($_.Exception.Response) { $status = [int]$_.Exception.Response.StatusCode }
    # Never print the raw request, headers, token, recipient, or response body.
    throw "LINE_REQUEST_FAILED: HTTP $status; no automatic retry performed."
}
