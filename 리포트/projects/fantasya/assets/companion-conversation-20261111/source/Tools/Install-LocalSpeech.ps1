param()
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
$projectRoot = Split-Path -Parent $PSScriptRoot
$speechDirectory = Join-Path $projectRoot '.utmp/LocalSpeech'
$runtimeDirectory = Join-Path $speechDirectory 'runtime'
New-Item -ItemType Directory -Force $speechDirectory, $runtimeDirectory | Out-Null
$archive = Join-Path $speechDirectory 'whisper-bin-x64.zip'
$release = Invoke-RestMethod 'https://api.github.com/repos/ggml-org/whisper.cpp/releases/tags/b5454'
$asset = $release.assets | Where-Object name -EQ 'whisper-bin-x64.zip'
if (-not $asset)
{
    throw 'Official Windows x64 speech runtime was not found.'
}
if (-not (Test-Path -LiteralPath $archive))
{
    Invoke-WebRequest $asset.browser_download_url -OutFile ($archive + '.download')
    Move-Item -LiteralPath ($archive + '.download') -Destination $archive
}
if ($asset.digest -and $asset.digest.StartsWith('sha256:'))
{
    if ((Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash.ToLowerInvariant() -ne $asset.digest.Substring(7))
    {
        throw 'Speech runtime checksum mismatch. Original download retained for inspection.'
    }
}
Expand-Archive -LiteralPath $archive -DestinationPath $runtimeDirectory -Force
$executable = Get-ChildItem -LiteralPath $runtimeDirectory -Filter 'whisper-cli.exe' -Recurse | Select-Object -First 1
if (-not $executable)
{
    throw 'whisper-cli.exe was not included in the official archive.'
}
$model = Join-Path $speechDirectory 'ggml-base.bin'
if (-not (Test-Path -LiteralPath $model))
{
    Invoke-WebRequest 'https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-base.bin' -OutFile ($model + '.download')
    Move-Item -LiteralPath ($model + '.download') -Destination $model
}
if ((Get-FileHash -LiteralPath $model -Algorithm SHA1).Hash.ToLowerInvariant() -ne '465707469ff3a37a2b9b8d8f89f2f99de7299dac')
{
    throw 'Multilingual base model checksum mismatch. Original retained.'
}
$dataDirectory = if ($env:FANTASYA_AGENT_DATA_DIR) { $env:FANTASYA_AGENT_DATA_DIR } else { Join-Path $projectRoot '.fantasya' }
New-Item -ItemType Directory -Force $dataDirectory | Out-Null
$configuration = @{ executable = $executable.FullName; model = $model; language = 'ko'; version = 'b5454' }
[IO.File]::WriteAllText((Join-Path $dataDirectory 'speech.json'), ($configuration | ConvertTo-Json))
Write-Output '한국어 로컬 음성 인식 준비 완료. 마이크 모드에서 녹음 후 문장을 확인하고 보내세요.'
Write-Output '설치 확인 중 마이크를 켜거나 녹음하지 않았습니다.'
