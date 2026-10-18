function Resolve-LineNode {
    $candidates=@()
    if($env:CODEX_MCP_NODE_PATH){$candidates+=$env:CODEX_MCP_NODE_PATH}
    $command=Get-Command node -ErrorAction SilentlyContinue
    if($command){$candidates+=$command.Source}
    $candidates+=(Join-Path $env:USERPROFILE '.cache/codex-runtimes/codex-primary-runtime/dependencies/node/bin/node.exe')
    foreach($candidate in $candidates | Select-Object -Unique){
        if(Test-Path -LiteralPath $candidate){
            $version=& $candidate --version 2>$null
            if($LASTEXITCODE -eq 0 -and $version -match '^v(\d+)\.' -and [int]$Matches[1] -ge 24){return $candidate}
        }
    }
    throw 'Node.js 24+ not found. Codex must locate its bundled runtime using load_workspace_dependencies and retry; no credential input is needed.'
}
