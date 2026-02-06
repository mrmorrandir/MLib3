param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization/MLib3.Protocols.Measurements.Serialization.csproj" -ApiKey $ApiKey
