param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore/MLib3.AspNetCore.csproj" -ApiKey $ApiKey
