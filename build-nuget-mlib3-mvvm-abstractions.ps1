param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.Abstractions/MLib3.MVVM.Abstractions.csproj" -ApiKey $ApiKey
