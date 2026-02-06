param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.Communication.MessageBroker/MLib3.Communication.MessageBroker.csproj" -ApiKey $ApiKey
