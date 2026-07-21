param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.AspNetCore.Application/MLib3.AspNetCore.Application.csproj" -ApiKey $ApiKey
