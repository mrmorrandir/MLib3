# Deployment Wrapper for MLib3.Communication.MessageBroker
# Usage: .\deploy-nuget-mlib3-communication-messagebroker.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Communication.MessageBroker/MLib3.Communication.MessageBroker.csproj" -ApiKey $ApiKey
