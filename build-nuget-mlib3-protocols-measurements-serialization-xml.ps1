param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Protocols.Measurements.Serialization.Xml/MLib3.Protocols.Measurements.Serialization.Xml.csproj" -ApiKey $ApiKey
