param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.SourceGenerators/MLib3.MVVM.SourceGenerators.csproj" -ApiKey $ApiKey
