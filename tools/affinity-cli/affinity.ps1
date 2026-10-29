# Uses the current Node runtime without opening a window or changing focus.
$ErrorActionPreference = 'Stop'
$nodeCommand = Get-Command node -ErrorAction SilentlyContinue
if (-not $nodeCommand) {
    $runtime = Join-Path $env:USERPROFILE '.cache/codex-runtimes/codex-primary-runtime/dependencies/node/bin/node.exe'
    if (-not (Test-Path -LiteralPath $runtime)) { throw 'Node.js 22+ is required. Install Node or run from the Codex bundled runtime.' }
    $nodeExecutable = $runtime
} else { $nodeExecutable = $nodeCommand.Source }
& $nodeExecutable (Join-Path $PSScriptRoot 'bin/affinity.mjs') @args
exit $LASTEXITCODE
