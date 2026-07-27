param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.Logging/MLib3.Logging.csproj" -ApiKey $ApiKey
