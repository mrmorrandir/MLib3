# Deployment Wrapper for MLib3.MVVM
# Usage: .\deploy-nuget-mlib3-mvvm.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM/MLib3.MVVM.csproj" -ApiKey $ApiKey
