# Deployment Wrapper for MLib3.AspDotNet.ApiKeys.Abstractions
# Usage: .\deploy-nuget-mlib3-aspdotnet-apikeys-abstractions.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspDotNet.ApiKeys.Abstractions/MLib3.AspDotNet.ApiKeys.Abstractions.csproj" -ApiKey $ApiKey
