param([switch]$Stop)
$ErrorActionPreference='Stop'
$codexRoot=if($env:CODEX_HOME){$env:CODEX_HOME}else{Join-Path $env:USERPROFILE '.codex'}
$state=Join-Path $codexRoot 'line-bridge'
New-Item -ItemType Directory -Force -Path $state | Out-Null
$pidFile=Join-Path $state 'processes.json'
if(Test-Path -LiteralPath $pidFile){
    $old=Get-Content -Raw -LiteralPath $pidFile | ConvertFrom-Json
    foreach($entry in $old.processes){
        $proc=Get-Process -Id $entry.id -ErrorAction SilentlyContinue
        if($proc -and $proc.StartTime.ToUniversalTime().Ticks.ToString() -eq $entry.started){Stop-Process -Id $proc.Id}
    }
}
if($Stop){Write-Output 'Bridge stopped on this PC.';return}
$shared=Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'line-notification.private.json') | ConvertFrom-Json
if(-not $shared.ChannelSecret){throw 'ChannelSecret missing from shared private settings; sync the repository.'}
. (Join-Path $PSScriptRoot 'Resolve-LineNode.ps1')
$node=Resolve-LineNode

$tunnelBin=Join-Path $state 'cloudflared.exe'
if(-not(Test-Path -LiteralPath $tunnelBin)){
    Invoke-WebRequest -Uri 'https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe' -OutFile $tunnelBin
}
if(Test-Path -LiteralPath (Join-Path $state 'tunnel.log')){Clear-Content -LiteralPath (Join-Path $state 'tunnel.log')}
$server=Start-Process -FilePath $node -ArgumentList ('"'+(Join-Path $PSScriptRoot 'bridge.mjs')+'"') -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $state 'server.out.log') -RedirectStandardError (Join-Path $state 'server.err.log')
$tunnel=Start-Process -FilePath $tunnelBin -ArgumentList @('tunnel','--url','http://127.0.0.1:17843','--no-autoupdate','--logfile',('"'+(Join-Path $state 'tunnel.log')+'"')) -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $state 'tunnel.out.log') -RedirectStandardError (Join-Path $state 'tunnel.err.log')
@{processes=@(@{id=$server.Id;started=$server.StartTime.ToUniversalTime().Ticks.ToString()},@{id=$tunnel.Id;started=$tunnel.StartTime.ToUniversalTime().Ticks.ToString()})} | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $pidFile -Encoding utf8
$endpoint=$null
for($i=0;$i -lt 40;$i++){
    Start-Sleep -Milliseconds 500
    $text=Get-Content -Raw -LiteralPath (Join-Path $state 'tunnel.log') -ErrorAction SilentlyContinue
    if(($text -match 'https://[a-z0-9-]+\.trycloudflare\.com') -and $text.Contains('Registered tunnel connection')){$endpoint=$Matches[0]+'/webhook';break}
}
if(-not $endpoint){throw 'Tunnel URL not ready; inspect local tunnel logs.'}
$settings=Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot 'line-notification.private.json') | ConvertFrom-Json
$headers=@{Authorization='Bearer '+$settings.ChannelAccessToken}
try{
    try { $previous=Invoke-RestMethod -Uri 'https://api.line.me/v2/bot/channel/webhook/endpoint' -Headers $headers }
    catch { if($_.Exception.Response -and [int]$_.Exception.Response.StatusCode -eq 404){$previous=@{active=$false;endpoint=''}}else{throw} }
    $previous | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $state 'previous-endpoint.json') -Encoding utf8
    $body=@{endpoint=$endpoint} | ConvertTo-Json
    $registered=$false
    for($attempt=0;$attempt -lt 5;$attempt++){
        try{
            $null=Invoke-RestMethod -Method Put -Uri 'https://api.line.me/v2/bot/channel/webhook/endpoint' -Headers $headers -ContentType 'application/json' -Body $body -TimeoutSec 15
            $registered=$true;break
        }catch{if($attempt -eq 4){throw};Start-Sleep -Seconds 1}
    }
    if(-not $registered){throw 'Webhook registration did not complete.'}
    $endpoint | Set-Content -LiteralPath (Join-Path $state 'endpoint.txt')
    Write-Output ('Webhook endpoint: '+$endpoint)
    Write-Output ('Previous webhook active: '+$previous.active)
}catch{throw 'Webhook registration failed; credentials and response omitted.'}
