$ErrorActionPreference='Stop'
$repo=Split-Path $PSScriptRoot
$settingsPath=Join-Path $repo 'line-notification.private.json'
$settings=Get-Content -Raw -LiteralPath $settingsPath | ConvertFrom-Json
. (Join-Path $repo 'Resolve-LineNode.ps1')
$node=Resolve-LineNode
$wrangler=Join-Path $PSScriptRoot 'node_modules/wrangler/bin/wrangler.js'
if(-not(Test-Path -LiteralPath $wrangler)){throw 'Install central/package.json dependencies first.'}
if(-not $settings.BridgeKey){$settings|Add-Member -NotePropertyName BridgeKey -NotePropertyValue ([Convert]::ToHexString([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))) -Force}
Push-Location $PSScriptRoot
try{
    $deployOutput=& $node $wrangler deploy 2>&1
    $deployExit=$LASTEXITCODE
    $deployOutput|Write-Output
    if($deployExit -ne 0){throw 'Worker deployment failed.'}
    $match=[regex]::Match(($deployOutput -join "`n"),'https://[a-z0-9.-]+\.workers\.dev')
    if(-not $match.Success){throw 'Worker URL missing from deployment output.'}
    $url=$match.Value
    $secretFile=Join-Path ([IO.Path]::GetTempPath()) ('codex-line-secrets-'+[guid]::NewGuid().ToString()+'.json')
    try{
        $secrets=@{LINE_TOKEN=$settings.ChannelAccessToken;LINE_SECRET=$settings.ChannelSecret;LINE_USER_ID=$settings.UserId;BRIDGE_KEY=$settings.BridgeKey}|ConvertTo-Json
        [IO.File]::WriteAllText($secretFile,$secrets,[Text.UTF8Encoding]::new($false))
        & $node $wrangler secret bulk $secretFile
        if($LASTEXITCODE -ne 0){throw 'Cloudflare Secret upload failed.'}
    }finally{if(Test-Path -LiteralPath $secretFile){Remove-Item -LiteralPath $secretFile}}
    $settings|Add-Member -NotePropertyName CentralUrl -NotePropertyValue $url -Force
    $settings|ConvertTo-Json|Set-Content -LiteralPath $settingsPath -Encoding utf8
    Write-Output ('CENTRAL_DEPLOYED: '+$url)
}finally{Pop-Location}
