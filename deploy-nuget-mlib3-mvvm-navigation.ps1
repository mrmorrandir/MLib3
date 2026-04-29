# Deployment Wrapper for MLib3.MVVM.Navigation
# Usage: .\deploy-nuget-mlib3-mvvm-navigation.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Navigation/MLib3.MVVM.Navigation.csproj" -ApiKey $ApiKey
