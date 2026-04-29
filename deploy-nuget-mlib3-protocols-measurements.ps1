# Deployment Wrapper for MLib3.Protocols.Measurements
# Usage: .\deploy-nuget-mlib3-protocols-measurements.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements/MLib3.Protocols.Measurements.csproj" -ApiKey $ApiKey
