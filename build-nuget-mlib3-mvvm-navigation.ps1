param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Navigation/MLib3.MVVM.Navigation.csproj" -ApiKey $ApiKey
