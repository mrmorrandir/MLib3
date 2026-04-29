# Deployment Wrapper for MLib3.MVVM.SourceGenerators
# Usage: .\deploy-nuget-mlib3-mvvm-sourcegenerators.ps1 -ApiKey "your-api-key-here"

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.SourceGenerators/MLib3.MVVM.SourceGenerators.csproj" -ApiKey $ApiKey
