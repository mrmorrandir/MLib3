# Deployment Wrapper for MLib3.Protocols.Measurements.Serialization.Xml
# Usage: .\deploy-nuget-mlib3-protocols-measurements-serialization-xml.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization.Xml/MLib3.Protocols.Measurements.Serialization.Xml.csproj" -ApiKey $ApiKey
