param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM/MLib3.MVVM.csproj" -ApiKey $ApiKey
