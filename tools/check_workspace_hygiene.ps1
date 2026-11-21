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

# Check 4: Manifest Completeness Invariant - All .vb files must be in docs/designs/mvp_erp_accounting.md
$specPath = Join-Path $workspaceRoot "docs/designs/mvp_erp_accounting.md"
if (Test-Path $specPath) {
    $specContent = [System.IO.File]::ReadAllText($specPath)
    $scanDirs = @("Merchandising_Accounting_App", "Merchandising_Accounting_App.Tests", "Merchandising_Accounting_App.PreviewTool")
    $missingFiles = @()
    foreach ($subDir in $scanDirs) {
        $fullSubDir = Join-Path $workspaceRoot $subDir
        if (Test-Path $fullSubDir) {
            $vbFiles = @(Get-ChildItem -Path $fullSubDir -Filter "*.vb" -File -Recurse | Where-Object {
                $_.FullName -notmatch '[\\/](bin|obj)[\\/]' -and $_.Name -cne "Application.Designer.vb"
            })
            foreach ($file in $vbFiles) {
                if ($specContent.IndexOf($file.Name, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
                    $relPath = $file.FullName.Substring($workspaceRoot.Length).TrimStart('\', '/')
                    $missingFiles += $relPath
                }
            }
        }
    }
    if ($missingFiles.Count -gt 0) {
        $missingList = $missingFiles -join ", "
        $violations += "[HYGIENE VIOLATION] Target file manifest in 'docs/designs/mvp_erp_accounting.md' is missing active source file(s): $missingList. All source files must be cataloged in the authoritative living specification manifest."
    }
} else {
    $violations += "[HYGIENE VIOLATION] Authoritative specification 'docs/designs/mvp_erp_accounting.md' not found."
}

# Check 5: Test Metric Parity Invariant - docs/README.md and docs/designs/mvp_erp_accounting.md must cite '217 tests'
$expectedSuiteCount = 217
$readmePath = Join-Path $workspaceRoot "docs/README.md"
if (Test-Path $readmePath) {
    $readmeContent = [System.IO.File]::ReadAllText($readmePath)
    if ($readmeContent.IndexOf("$expectedSuiteCount tests", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        $violations += "[HYGIENE VIOLATION] 'docs/README.md' must cite '$expectedSuiteCount tests'."
    }
}
if (Test-Path $specPath) {
    $specContent = [System.IO.File]::ReadAllText($specPath)
    if ($specContent.IndexOf("$expectedSuiteCount automated tests", [System.StringComparison]::OrdinalIgnoreCase) -lt 0 -and
        $specContent.IndexOf("$expectedSuiteCount tests", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        $violations += "[HYGIENE VIOLATION] 'docs/designs/mvp_erp_accounting.md' must cite '$expectedSuiteCount tests'."
    }
}

# Check 6: EF Core Dependency Version Parity Invariant - docs/designs/mvp_erp_accounting.md must cite 10.0.0 and no obsolete versions
if (Test-Path $specPath) {
    $specContent = [System.IO.File]::ReadAllText($specPath)
    if ($specContent.IndexOf("9.0.2 or .NET 10 preview", [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
        $violations += "[HYGIENE VIOLATION] 'docs/designs/mvp_erp_accounting.md' must not reference obsolete '9.0.2 or .NET 10 preview' EF Core packages."
    }
    if ($specContent.IndexOf("10.0.0", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        $violations += "[HYGIENE VIOLATION] 'docs/designs/mvp_erp_accounting.md' must reference official RTM '10.0.0' EF Core package."
    }
}

# Process results
if ($violations.Count -gt 0) {
    foreach ($violation in $violations) {
        [Console]::Error.WriteLine($violation)
    }
    exit 1
} else {
    if (-not $Hook) {
        Write-Host "Workspace hygiene check passed: Repository root is clean and conforms to Antigravity standards."
    }
}

if ($Hook) {
    # PostToolUse expects an empty JSON object {} on stdout
    Write-Output "{}"
}
