param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Navigation.SourceGenerated/MLib3.MVVM.Navigation.SourceGenerated.csproj" -ApiKey $ApiKey
