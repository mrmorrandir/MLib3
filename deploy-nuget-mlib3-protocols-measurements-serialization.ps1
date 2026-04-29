# Deployment Wrapper for MLib3.Protocols.Measurements.Serialization
# Usage: .\deploy-nuget-mlib3-protocols-measurements-serialization.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization/MLib3.Protocols.Measurements.Serialization.csproj" -ApiKey $ApiKey
