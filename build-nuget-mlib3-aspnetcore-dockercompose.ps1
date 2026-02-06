param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore.DockerCompose/MLib3.AspNetCore.DockerCompose.csproj" -ApiKey $ApiKey
