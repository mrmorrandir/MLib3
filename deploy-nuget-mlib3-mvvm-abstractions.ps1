# Deployment Wrapper for MLib3.MVVM.Abstractions
# Usage: .\deploy-nuget-mlib3-mvvm-abstractions.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Abstractions/MLib3.MVVM.Abstractions.csproj" -ApiKey $ApiKey
