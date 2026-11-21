[CmdletBinding()]
param()

$rawInput = [Console]::In.ReadToEnd()
$json = if (-not [string]::IsNullOrWhiteSpace($rawInput)) { $rawInput | ConvertFrom-Json } else { $null }

if ($json -and $json.continuationActive -eq $true) {
    Write-Output '{"decision": "allow"}'
    exit 0
}

if ($json -and $json.terminationReason -ne "NO_TOOL_CALL") {
    Write-Output '{"decision": "allow"}'
    exit 0
}

$toolsDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$hygieneScript = Join-Path $toolsDir "check_workspace_hygiene.ps1"

$output = & powershell -NoProfile -ExecutionPolicy Bypass -File $hygieneScript 2>&1
if ($LASTEXITCODE -ne 0) {
    $cleanReason = ($output -join "; ").Replace('"', '\"')
    Write-Output "{`"decision`": `"continue`", `"reason`": `"[GOVERNANCE GATE FAILURE] Turn completion blocked: $cleanReason. You must fix documentation/manifest drifts before finishing.`"}"
    exit 0
}

# Check for uncommitted work before completing turn
$uncommitted = git status --porcelain
if (-not [string]::IsNullOrWhiteSpace($uncommitted)) {
    Write-Output '{"decision": "continue", "reason": "[GOVERNANCE GATE FAILURE] Turn completion blocked: You have uncommitted changes in your working tree. Per GEMINI.md Section 1.4, stage and commit all verified changes before ending the turn."}'
    exit 0
}

Write-Output '{"decision": "allow"}'
