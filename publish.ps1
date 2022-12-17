# Read the API ApiKey from the console
$apiKey = Read-Host -Prompt 'Enter your GitHub Access Token for the dotnet nuget source "github"'

# Try to build the solution
dotnet build --configuration Release
# If the build failed, exit the script
if ($LastExitCode -ne 0) {
    # Tell the user that the build failed
    Write-Host "Build failed - no packages will be published" -ForegroundColor Red
    exit $LastExitCode
}
# Tell the user that the build succeeded and that the unit tests will be run
Write-Host "Build succeeded - running unit tests" -ForegroundColor Green

# Run the unit tests
dotnet test --configuration Release
# If the unit tests failed, exit the script
if ($LastExitCode -ne 0) {
    # Tell the user that the unit tests failed
    Write-Host "Unit tests failed - no packages will be published" -ForegroundColor Red
    exit $LastExitCode
}
# Tell the user that the unit tests succeeded and that the packages will be published
Write-Host "Unit tests succeeded - publishing packages" -ForegroundColor Green

# Go through all subfolders in the ./src folder
$folders = Get-ChildItem -Path "./src" -Directory
# for each subfolder go to the "./bin/Release" folder and take all the files with the extension ".nupkg"
foreach ($folder in $folders) {
    $files = Get-ChildItem -Path "./src/$($folder.Name)/bin/Release" -Filter "*.nupkg"
    # for each file in the folder, publish it to the NuGet server
    foreach ($file in $files) {
        Write-Host "Publishing $($file.Name) to the GitHub NuGet server" -ForegroundColor Cyan
        # Publish the package to the NuGet server and skip the package if it already exists
        dotnet nuget push $file.FullName --source "github" --api-key $apiKey --skip-duplicate
    }
}