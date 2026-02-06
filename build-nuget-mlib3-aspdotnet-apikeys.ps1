param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.AspDotNet.ApiKeys/MLib3.AspDotNet.ApiKeys.csproj" -ApiKey $ApiKey
