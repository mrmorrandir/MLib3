param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

& ./build-nuget-base.ps1 -ProjectPath "src/MLib3.MVVM.WPF.ViewLocators/MLib3.MVVM.WPF.ViewLocators.csproj" -ApiKey $ApiKey

