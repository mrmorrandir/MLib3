# Deployment Wrapper for MLib3.Communication.MessageBroker.Abstractions
# Usage: .\deploy-nuget-mlib3-communication-messagebroker-abstractions.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Communication.MessageBroker.Abstractions/MLib3.Communication.MessageBroker.Abstractions.csproj" -ApiKey $ApiKey
