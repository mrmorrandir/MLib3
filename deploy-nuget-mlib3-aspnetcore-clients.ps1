param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore.Clients/MLib3.AspNetCore.Clients.csproj" -ApiKey $ApiKey
