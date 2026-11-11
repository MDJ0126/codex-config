param([switch]$WarmModel)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$client = Join-Path $PSScriptRoot 'AgentBridge/client.mjs'
& node $client list_companions '{}' setup | Out-Null
if ($LASTEXITCODE -ne 0)
{
    throw 'The local companion service could not be started.'
}
if ($WarmModel)
{
    & (Join-Path $PSScriptRoot 'Start-LocalAgentModel.ps1')
}
Write-Output '동료 서비스 준비 완료. Playground에서 Play 후 동료에게 말을 걸어 주세요.'
Write-Output 'Codex: node Tools/AgentBridge/configure.mjs codex'
Write-Output 'Claude: node Tools/AgentBridge/configure.mjs claude'
Write-Output 'Cursor: node Tools/AgentBridge/configure.mjs cursor'
