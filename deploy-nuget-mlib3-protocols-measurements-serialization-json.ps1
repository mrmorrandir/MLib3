# Deployment Wrapper for MLib3.Protocols.Measurements.Serialization.Json
# Usage: .\deploy-nuget-mlib3-protocols-measurements-serialization-json.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization.Json/MLib3.Protocols.Measurements.Serialization.Json.csproj" -ApiKey $ApiKey
