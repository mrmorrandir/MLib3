param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements/MLib3.Protocols.Measurements.csproj" -ApiKey $ApiKey
