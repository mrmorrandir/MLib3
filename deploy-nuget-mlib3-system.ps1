# Deployment Wrapper for MLib3.System
# Usage: .\deploy-nuget-mlib3-system.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.System/MLib3.System.csproj" -ApiKey $ApiKey
