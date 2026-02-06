param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Communication.MessageBroker.Abstractions/MLib3.Communication.MessageBroker.Abstractions.csproj" -ApiKey $ApiKey
