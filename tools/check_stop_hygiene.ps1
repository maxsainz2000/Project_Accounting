[CmdletBinding()]
param()

$rawInput = [Console]::In.ReadToEnd()
$json = if (-not [string]::IsNullOrWhiteSpace($rawInput)) {
    try { $rawInput | ConvertFrom-Json } catch { $null }
} else { $null }

if ($json -and $json.continuationActive -eq $true) {
    Write-Output '{"decision": "allow"}'
    exit 0
}

if ($json -and $json.terminationReason -ne "NO_TOOL_CALL") {
    Write-Output '{"decision": "allow"}'
    exit 0
}

$toolsDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
$hygieneScript = Join-Path $toolsDir "check_workspace_hygiene.ps1"

$output = & powershell -NoProfile -ExecutionPolicy Bypass -File $hygieneScript 2>&1
if ($LASTEXITCODE -ne 0) {
    $cleanReason = ($output | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) -join "; "
    $response = [ordered]@{
        decision = "continue"
        reason = "[GOVERNANCE GATE FAILURE] Turn completion blocked: $cleanReason. You must fix documentation/manifest drifts before finishing."
    }
    Write-Output ($response | ConvertTo-Json -Compress)
    exit 0
}

# Check for uncommitted work before completing turn
$uncommitted = git status --porcelain
if (-not [string]::IsNullOrWhiteSpace($uncommitted)) {
    $response = [ordered]@{
        decision = "continue"
        reason = "[GOVERNANCE GATE FAILURE] Turn completion blocked: You have uncommitted changes in your working tree. Per GEMINI.md Section 1.4, stage and commit all verified changes before ending the turn."
    }
    Write-Output ($response | ConvertTo-Json -Compress)
    exit 0
}

Write-Output '{"decision": "allow"}'
