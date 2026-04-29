# Deployment Wrapper for MLib3.MVVM.WPF.ViewLocators
# Usage: .\deploy-nuget-mlib3-mvvm-wpf-viewlocators.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.WPF.ViewLocators/MLib3.MVVM.WPF.ViewLocators.csproj" -ApiKey $ApiKey
