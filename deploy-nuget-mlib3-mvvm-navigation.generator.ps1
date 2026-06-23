param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& .\deploy-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Navigation.Generator/MLib3.MVVM.Navigation.Generator.csproj" -ApiKey $ApiKey
