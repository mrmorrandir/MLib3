# Deployment Wrapper for MLib3.AspNetCore.DockerCompose
# Usage: .\deploy-nuget-mlib3-aspnetcore-dockercompose.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore.DockerCompose/MLib3.AspNetCore.DockerCompose.csproj" -ApiKey $ApiKey
