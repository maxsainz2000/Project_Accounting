<#
.SYNOPSIS
    Automated UI preview and screenshot capture launcher for Windows Forms.
.DESCRIPTION
    Wraps dotnet run on Merchandising_Accounting_App.PreviewTool to render
    ERP dashboard forms, all 11 tab views, interactive states, and modal dialogs
    offscreen to PNG without requiring Visual Studio.
.EXAMPLE
    .\preview_ui.ps1 --all
    .\preview_ui.ps1 --tabs
    .\preview_ui.ps1 --states
    .\preview_ui.ps1 --dialogs
    .\preview_ui.ps1 --form Purchasing
    .\preview_ui.ps1 --form "General Ledger" --mode restore
    .\preview_ui.ps1 --resolution 1920x1080
    .\preview_ui.ps1 --help
#>

$ErrorActionPreference = "Stop"

$projectPath = Join-Path $PSScriptRoot "Merchandising_Accounting_App.PreviewTool\Merchandising_Accounting_App.PreviewTool.vbproj"

if (-not (Test-Path $projectPath)) {
    Write-Error "PreviewTool project file not found at: $projectPath"
    exit 1
}

if ($args.Count -eq 0) {
    $dotnetArgs = @("run", "--project", $projectPath, "--", "--all")
} else {
    $dotnetArgs = @("run", "--project", $projectPath, "--") + $args
}

& dotnet @dotnetArgs
exit $LASTEXITCODE
