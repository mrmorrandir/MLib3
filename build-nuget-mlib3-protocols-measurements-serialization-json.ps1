param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization.Json/MLib3.Protocols.Measurements.Serialization.Json.csproj" -ApiKey $ApiKey
