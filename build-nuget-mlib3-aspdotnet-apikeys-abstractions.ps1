param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.AspDotNet.ApiKeys.Abstractions/MLib3.AspDotNet.ApiKeys.Abstractions.csproj" -ApiKey $ApiKey
