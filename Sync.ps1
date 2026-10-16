param(
    [ValidateSet('Pull','Push')][string]$Direction='Pull',
    [string]$Message='Update global Codex instructions',
    [string]$CodexDirectory=$(if($env:CODEX_HOME){$env:CODEX_HOME}else{Join-Path $env:USERPROFILE '.codex'})
)
$ErrorActionPreference='Stop'
$repository=$PSScriptRoot
function Invoke-Git {
    param([string[]]$GitArguments)
    & git -C $repository @GitArguments
    if($LASTEXITCODE -ne 0){throw "git failed: $($GitArguments[0]) ($LASTEXITCODE)"}
}
$installed=Join-Path $CodexDirectory 'AGENTS.md'
$tracked=Join-Path $repository 'AGENTS.md'
if($Direction -eq 'Push'){
    if(-not (Test-Path -LiteralPath $installed)){throw 'Installed AGENTS.md is missing'}
    Copy-Item -LiteralPath $installed -Destination $tracked
    Invoke-Git -GitArguments @('add','--','AGENTS.md','Sync.ps1','README.md','.gitignore')
    & git -C $repository diff --cached --quiet
    if($LASTEXITCODE -eq 1){Invoke-Git -GitArguments @('commit','-m',$Message)}
    elseif($LASTEXITCODE -ne 0){throw 'Cannot inspect staged changes'}
    Invoke-Git -GitArguments @('push','-u','origin','HEAD')
}else{
    $dirty=Invoke-Git -GitArguments @('status','--porcelain')
    if($dirty){throw 'Repository has local changes; preserve or commit them before pulling'}
    # Refuse to overwrite installed instructions edited independently of this checkout.
    if((Test-Path -LiteralPath $installed) -and (Test-Path -LiteralPath $tracked) -and
       (Get-FileHash -LiteralPath $installed).Hash -ne (Get-FileHash -LiteralPath $tracked).Hash){
        throw 'Installed instructions differ from the checkout; push or reconcile them before pulling'
    }
    Invoke-Git -GitArguments @('pull','--ff-only')
    New-Item -ItemType Directory -Force -Path $CodexDirectory | Out-Null
    if(Test-Path -LiteralPath $installed){
        $backupDirectory=Join-Path $CodexDirectory 'instruction-backups'
        New-Item -ItemType Directory -Force -Path $backupDirectory | Out-Null
        Copy-Item -LiteralPath $installed -Destination (Join-Path $backupDirectory ('AGENTS.'+(Get-Date -Format 'yyyyMMdd-HHmmss-fff')+'.md'))
    }
    Copy-Item -LiteralPath $tracked -Destination $installed
}
if((Get-FileHash -LiteralPath $installed).Hash -ne (Get-FileHash -LiteralPath $tracked).Hash){throw 'Instruction copy verification failed'}
Write-Output "Global instructions synchronized: $Direction"
