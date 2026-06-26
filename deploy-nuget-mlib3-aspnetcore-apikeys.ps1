param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore.ApiKeys/MLib3.AspNetCore.ApiKeys.csproj" -ApiKey $ApiKey
