[CmdletBinding()]
param(
    [switch]$Hook,
    [string]$RootPath = ""
)

$ErrorActionPreference = 'Stop'

# Determine repository root
if (-not [string]::IsNullOrWhiteSpace($RootPath)) {
    $workspaceRoot = (Resolve-Path $RootPath).Path
} else {
    $toolsDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    if ($toolsDir) {
        $workspaceRoot = Split-Path -Parent $toolsDir
    } else {
        $workspaceRoot = (Get-Location).Path
    }
}

# Fallback: search upward for Merchandising_Accounting_App.slnx or GEMINI.md
if (-not (Test-Path (Join-Path $workspaceRoot "Merchandising_Accounting_App.slnx"))) {
    $searchDir = (Get-Location).Path
    while ($searchDir) {
        if (Test-Path (Join-Path $searchDir "Merchandising_Accounting_App.slnx")) {
            $workspaceRoot = $searchDir
            break
        }
        $parent = Split-Path -Parent $searchDir
        if ($parent -eq $searchDir) { break }
        $searchDir = $parent
    }
}

$violations = @()

# Check 1: Root Markdown Invariant - GEMINI.md must be the only .md file in workspace root
if (Test-Path $workspaceRoot) {
    $rootMdFiles = @(Get-ChildItem -Path $workspaceRoot -Filter "*.md" -File | Where-Object { $_.Name -cne "GEMINI.md" })
    if ($rootMdFiles.Count -gt 0) {
        $offenderNames = ($rootMdFiles | ForEach-Object { $_.Name }) -join ", "
        $violations += "[HYGIENE VIOLATION] Unauthorized root markdown file(s): $offenderNames. Only GEMINI.md is permitted in the repository root. Direct designs to 'docs/designs/', plans to 'docs/plans/', and scratch files to artifact directories (GEMINI.md §1.2)."
    }
}

# Check 2: GEMINI.md Rule File Existence & Size Invariant (<= 24,000 bytes)
$geminiPath = Join-Path $workspaceRoot "GEMINI.md"
if (-not (Test-Path $geminiPath)) {
    $violations += "[HYGIENE VIOLATION] Mandatory rule file GEMINI.md is missing from repository root ($workspaceRoot)."
} else {
    $fileSize = (Get-Item $geminiPath).Length
    if ($fileSize -gt 24000) {
        $violations += "[HYGIENE VIOLATION] GEMINI.md size ($fileSize bytes) exceeds the 24,000 bytes limit (Antigravity agy-customizations truncation threshold)."
    }
}

# Check 3: Active Plans Invariant - No completed or merged plans in docs/plans/
$plansDir = Join-Path $workspaceRoot "docs/plans"
if (Test-Path $plansDir) {
    $planFiles = @(Get-ChildItem -Path $plansDir -Filter "*.md" -File -Recurse)
    if ($planFiles.Count -gt 0) {
        $terminalRegex = [regex]"(?i)\bStatus\b[\s\*_]*[:|=][\s\*""'`\[]*(Completed|Merged)\b"
        foreach ($planFile in $planFiles) {
            $planContent = [System.IO.File]::ReadAllText($planFile.FullName)
            $match = $terminalRegex.Match($planContent)
            if ($match.Success) {
                $relPath = $planFile.FullName.Substring($workspaceRoot.Length).TrimStart('\', '/')
                $violations += "[HYGIENE VIOLATION] Completed or merged plan found in active directory 'docs/plans/': $relPath (matched '$($match.Value.Trim())'). Per GEMINI.md §1.2, move to 'docs/archive/plans/YYYY-MM-DD_<name>/'."
            }
        }
    }
}

# Process results
if ($violations.Count -gt 0) {
    foreach ($violation in $violations) {
        [Console]::Error.WriteLine($violation)
    }
    if (-not $Hook) {
        exit 1
    }
} else {
    if (-not $Hook) {
        Write-Host "Workspace hygiene check passed: Repository root is clean and conforms to Antigravity standards."
    }
}

if ($Hook) {
    # PostToolUse expects an empty JSON object {} on stdout
    Write-Output "{}"
}
