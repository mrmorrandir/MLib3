# Deployment Wrapper for MLib3.AspDotNet.ApiKeys
# Usage: .\deploy-nuget-mlib3-aspdotnet-apikeys.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspDotNet.ApiKeys/MLib3.AspDotNet.ApiKeys.csproj" -ApiKey $ApiKey
