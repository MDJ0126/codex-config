param(
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][string]$SessionTitle,
    [Parameter(Mandatory=$true)][string]$Message,
    [switch]$DryRun
)
$ErrorActionPreference = 'Stop'
$SessionTitle = ($SessionTitle -replace '[\r\n]+', ' ').Trim()
if ([string]::IsNullOrWhiteSpace($SessionTitle)) { throw 'SessionTitle must not be blank.' }
$Message = '[' + $SessionTitle + "]`n" + $Message
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
    $null = Invoke-RestMethod -Method Post -Uri 'https://api.line.me/v2/bot/message/push' -Headers $headers -ContentType 'application/json; charset=utf-8' -Body ([Text.Encoding]::UTF8.GetBytes($body)) -TimeoutSec 20
    Write-Output 'API_ACCEPTED: LINE accepted the push request; device delivery is unconfirmed.'
} catch {
    $status = 'unknown'
    if ($_.Exception.Response) { $status = [int]$_.Exception.Response.StatusCode }
    # Never print the raw request, headers, token, recipient, or response body.
    throw "LINE_REQUEST_FAILED: HTTP $status; no automatic retry performed."
}
