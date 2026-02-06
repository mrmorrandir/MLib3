# NuGet Package Build Base Script
# This script builds NuGet packages with proper versioning, testing, and git tagging
# Use wrapper scripts instead of calling this directly

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectPath,
    
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
    
    [Parameter(Mandatory=$false)]
    [string]$Registry = "https://baget.wittag.net/v3/index.json",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipGitCheck
)

$ErrorActionPreference = "Stop"

# ============================================================================
# HELPER FUNCTIONS
# ============================================================================

function Get-PackageNameFromProject {
    param([string]$ProjectPath)
    
    # Read .csproj file
    [xml]$projectXml = Get-Content -Path $ProjectPath
    
    # Try to get PackageId first, then fall back to project file name
    $packageId = $projectXml.Project.PropertyGroup.PackageId
    
    if ([string]::IsNullOrWhiteSpace($packageId)) {
        # Use file name without extension
        $packageId = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)
    }
    
    return $packageId
}

function Get-VersionFromProject {
    param([string]$ProjectPath)
    
    # Read .csproj file
    [xml]$projectXml = Get-Content -Path $ProjectPath
    
    # Look for Version in PropertyGroup
    $version = $projectXml.Project.PropertyGroup.Version
    
    if ([string]::IsNullOrWhiteSpace($version)) {
        Write-ColorOutput "❌ ERROR: No <Version> found in .csproj file" "Red"
        Write-ColorOutput "   Add <Version>1.0.0</Version> to your .csproj" "Yellow"
        exit 1
    }
    
    return $version
}

function Test-SemanticVersion {
    param([string]$Version)
    
    # Validate semantic versioning format: MAJOR.MINOR.PATCH[-prerelease]
    # Allow: 1.2.3 or 1.2.3-preview-20260206.1
    $pattern = '^\d+\.\d+\.\d+(-preview-\d{8}\.\d+)?$'
    
    if ($Version -notmatch $pattern) {
        Write-ColorOutput "❌ ERROR: Invalid version format: $Version" "Red"
        Write-ColorOutput "   Valid formats:" "Yellow"
        Write-ColorOutput "   - Stable: 1.2.3" "Gray"
        Write-ColorOutput "   - Prerelease: 1.2.3-preview-20260206.1" "Gray"
        exit 1
    }
    
    return $true
}

function Get-ComponentNameForGitTag {
    param([string]$PackageName)
    
    # Convert package name to lowercase and replace dots with hyphens
    # Example: Herkules.Shared.Library -> herkules-shared-library
    $componentName = $PackageName.ToLower() -replace '\.', '-'
    
    return $componentName
}

function Test-IsPrerelease {
    param([string]$Version)
    
    return $Version -match '-preview-'
}

function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

function Get-ProjectDirectory {
    param([string]$ProjectPath)
    
    return Split-Path -Parent $ProjectPath
}

function Test-PackageExistsInRegistry {
    param(
        [string]$PackageName,
        [string]$Version,
        [string]$Registry
    )
    
    try {
        # Extract base URL from registry (remove /v3/index.json)
        $baseUrl = $Registry -replace '/v3/index\.json$', ''
        
        # BaGet/NuGet package URL format: {baseUrl}/v3/registration/{packageId}/index.json
        $packageId = $PackageName.ToLower()
        $registrationUrl = "$baseUrl/v3/registration/$packageId/index.json"
        
        Write-ColorOutput "   Checking registry: $baseUrl" "Gray"
        
        # Query the registry
        $response = Invoke-RestMethod -Uri $registrationUrl -Method Get -ErrorAction SilentlyContinue
        
        # Check if the specific version exists in the items
        $versionExists = $false
        foreach ($page in $response.items) {
            if ($page.items) {
                foreach ($item in $page.items) {
                    if ($item.catalogEntry.version -eq $Version) {
                        $versionExists = $true
                        break
                    }
                }
            }
            if ($versionExists) { break }
        }
        
        return $versionExists
    }
    catch {
        # If package doesn't exist at all, or registry error, return false
        # This could mean the package has never been published, which is fine
        Write-ColorOutput "   (Could not query registry - assuming package doesn't exist)" "Gray"
        return $false
    }
}

# ============================================================================
# VALIDATION
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "NuGet Package Build Script" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

# Validate project file exists
if (-not (Test-Path $ProjectPath)) {
    Write-ColorOutput "❌ ERROR: Project file not found: $ProjectPath" "Red"
    exit 1
}

if ([System.IO.Path]::GetExtension($ProjectPath) -ne ".csproj") {
    Write-ColorOutput "❌ ERROR: Not a .csproj file: $ProjectPath" "Red"
    exit 1
}

Write-ColorOutput "📄 Project: $ProjectPath" "Gray"
Write-Host ""

# Get package name from project
$packageName = Get-PackageNameFromProject -ProjectPath $ProjectPath
Write-ColorOutput "📦 Package Name: $packageName" "Cyan"

# Get version from project
$version = Get-VersionFromProject -ProjectPath $ProjectPath
Write-ColorOutput "🏷️  Version: $version" "Cyan"

# Validate version format
Test-SemanticVersion -Version $version | Out-Null

# Determine if prerelease
$isPrerelease = Test-IsPrerelease -Version $version

if ($isPrerelease) {
    Write-ColorOutput "⚠️  Build Type: PRERELEASE" "Yellow"
} else {
    Write-ColorOutput "✅ Build Type: STABLE RELEASE" "Green"
}

Write-ColorOutput "📍 Registry: $Registry" "Cyan"
Write-Host ""

# Get component name for git tag
$componentName = Get-ComponentNameForGitTag -PackageName $packageName
$gitTag = "$componentName-v$version"

# ============================================================================
# GIT CHECKS
# ============================================================================

Write-ColorOutput "Performing git validations..." "Yellow"
Write-Host ""

# 1. Check if on main branch
Write-ColorOutput "1️⃣  Checking git branch..." "Yellow"
$currentBranch = git rev-parse --abbrev-ref HEAD

if ($currentBranch -ne "main") {
    Write-ColorOutput "❌ ERROR: Not on main branch!" "Red"
    Write-ColorOutput "   Current branch: $currentBranch" "Red"
    Write-ColorOutput "   All releases must be from main branch" "Yellow"
    Write-ColorOutput "" "White"
    Write-ColorOutput "   To switch to main:" "Yellow"
    Write-ColorOutput "   git checkout main" "Gray"
    Write-ColorOutput "   git pull origin main" "Gray"
    exit 1
}

Write-ColorOutput "   ✅ On main branch" "Green"
Write-Host ""

# 2. Check for uncommitted changes
if (-not $SkipGitCheck) {
    Write-ColorOutput "2️⃣  Checking for uncommitted changes..." "Yellow"
    $gitStatus = git status --porcelain
    
    if ($gitStatus) {
        Write-ColorOutput "❌ ERROR: Uncommitted changes detected!" "Red"
        Write-ColorOutput "" "White"
        git status
        Write-ColorOutput "" "White"
        Write-ColorOutput "   Commit all changes before building release" "Yellow"
        Write-ColorOutput "   Make sure your version bump is committed!" "Yellow"
        exit 1
    }
    
    Write-ColorOutput "   ✅ No uncommitted changes" "Green"
    Write-Host ""
} else {
    Write-ColorOutput "2️⃣  ⚠️  Skipping uncommitted changes check" "Yellow"
    Write-Host ""
}

# 3. Pull latest changes
Write-ColorOutput "3️⃣  Pulling latest changes from remote..." "Yellow"
git pull origin main

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "❌ ERROR: Failed to pull from remote" "Red"
    exit 1
}

Write-ColorOutput "   ✅ Up to date with remote" "Green"
Write-Host ""

# 4. Check if package version already exists in NuGet registry
Write-ColorOutput "4️⃣  Checking if package already exists in NuGet registry..." "Yellow"
$packageExists = Test-PackageExistsInRegistry -PackageName $packageName -Version $version -Registry $Registry

if ($packageExists) {
    Write-ColorOutput "❌ ERROR: Package already exists in registry!" "Red"
    Write-ColorOutput "   Package: $packageName" "Yellow"
    Write-ColorOutput "   Version: $version" "Yellow"
    Write-ColorOutput "   Registry: $Registry" "Yellow"
    Write-ColorOutput "" "White"
    Write-ColorOutput "   This version has already been published" "Yellow"
    Write-ColorOutput "   If this is an older repo without git tags, find the release commit manually and tag it:" "Yellow"
    Write-ColorOutput "   git tag -a $gitTag <commit-hash> -m 'Release $packageName version $version'" "Gray"
    Write-ColorOutput "   git push origin $gitTag" "Gray"
    Write-ColorOutput "" "White"
    Write-ColorOutput "   Otherwise, update the version in .csproj and commit the change" "Yellow"
    exit 1
}

Write-ColorOutput "   ✅ Package version is unique in registry" "Green"
Write-Host ""

# 5. Check if git tag already exists
Write-ColorOutput "5️⃣  Checking if git tag already exists..." "Yellow"
$existingTag = git tag -l $gitTag

if ($existingTag) {
    Write-ColorOutput "❌ ERROR: Git tag already exists: $gitTag" "Red"
    Write-ColorOutput "   This version has already been tagged in git" "Yellow"
    Write-ColorOutput "   Update the version in .csproj and commit the change" "Yellow"
    exit 1
}

Write-ColorOutput "   ✅ Git tag is unique" "Green"
Write-Host ""

# ============================================================================
# BUILD
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "Building Project" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

$projectDir = Get-ProjectDirectory -ProjectPath $ProjectPath

Write-ColorOutput "🔨 Running dotnet build..." "Cyan"
Write-Host ""

# Build in Release configuration
dotnet build $ProjectPath --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "" "White"
    Write-ColorOutput "❌ ERROR: Build failed!" "Red"
    Write-ColorOutput "   Fix build errors before publishing" "Yellow"
    exit 1
}

Write-Host ""
Write-ColorOutput "✅ Build succeeded" "Green"
Write-Host ""

# ============================================================================
# TESTS
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "Running Tests" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

# Solution structure:
# SolutionRoot/
#   ├── Solution.sln
#   ├── src/
#   │   └── ProjectName/
#   └── tests/
#       ├── ProjectName.UnitTests/
#       ├── ProjectName.IntegrationTests/
#       └── ProjectName.Tests/

# Find solution root (parent of src folder)
$solutionDir = Split-Path -Parent $projectDir  # Go up from project dir (e.g., src/ProjectName -> src)
$solutionRoot = Split-Path -Parent $solutionDir  # Go up from src to solution root

Write-ColorOutput "🧪 Running dotnet test..." "Cyan"
Write-Host ""

# Get the project name to find matching test projects
$projectName = Split-Path -Leaf $projectDir

# Look for test projects in /tests folder
$testsFolder = Join-Path $solutionRoot "tests"

if (Test-Path $testsFolder) {
    Write-ColorOutput "   Searching for test projects in: $testsFolder" "Gray"
    
    # Find all test projects that start with the project name
    # Patterns: ProjectName.Tests, ProjectName.UnitTests, ProjectName.IntegrationTests, ProjectName.FunctionalTests
    $testProjects = Get-ChildItem -Path $testsFolder -Directory | 
        Where-Object { $_.Name -like "$projectName.*Tests" } |
        ForEach-Object {
            $testCsproj = Get-ChildItem -Path $_.FullName -Filter "*.csproj" | Select-Object -First 1
            if ($testCsproj) {
                $testCsproj.FullName
            }
        }
    
    if ($testProjects) {
        Write-ColorOutput "   Found $($testProjects.Count) test project(s):" "Gray"
        foreach ($testProj in $testProjects) {
            Write-ColorOutput "      - $(Split-Path -Leaf (Split-Path -Parent $testProj))" "Gray"
        }
        Write-Host ""
        
        # Run each test project
        $allTestsPassed = $true
        foreach ($testProj in $testProjects) {
            $testProjName = Split-Path -Leaf (Split-Path -Parent $testProj)
            Write-ColorOutput "   Running tests: $testProjName..." "Cyan"
            
            dotnet test $testProj --configuration Release --no-build --verbosity minimal
            
            if ($LASTEXITCODE -ne 0) {
                $allTestsPassed = $false
                Write-ColorOutput "   ❌ Tests failed in: $testProjName" "Red"
            } else {
                Write-ColorOutput "   ✅ Tests passed in: $testProjName" "Green"
            }
            Write-Host ""
        }
        
        if (-not $allTestsPassed) {
            Write-ColorOutput "❌ ERROR: One or more test projects failed!" "Red"
            Write-ColorOutput "   Fix failing tests before publishing" "Yellow"
            Write-ColorOutput "" "White"
            Write-ColorOutput "   Run tests locally:" "Yellow"
            Write-ColorOutput "   dotnet test" "Gray"
            exit 1
        }
    } else {
        Write-ColorOutput "   ⚠️  No test projects found matching: $projectName.*Tests" "Yellow"
        Write-ColorOutput "   Consider adding test projects!" "Yellow"
    }
} else {
    Write-ColorOutput "   ⚠️  No /tests folder found in solution" "Yellow"
    Write-ColorOutput "   Expected path: $testsFolder" "Gray"
    Write-ColorOutput "   Consider adding a tests folder with test projects!" "Yellow"
}

Write-Host ""
Write-ColorOutput "✅ All tests passed" "Green"
Write-Host ""

# ============================================================================
# GIT TAG
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "Creating Git Tag" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

Write-ColorOutput "🏷️  Creating git tag: $gitTag..." "Cyan"

# Create annotated tag
$tagMessage = "Release $packageName version $version"
git tag -a $gitTag -m $tagMessage

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "❌ ERROR: Failed to create git tag" "Red"
    exit 1
}

# Push tag to remote
Write-ColorOutput "   Pushing tag to remote..." "Gray"
git push origin $gitTag

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "❌ ERROR: Failed to push git tag to remote" "Red"
    Write-ColorOutput "   Rolling back local tag..." "Yellow"
    git tag -d $gitTag
    exit 1
}

Write-Host ""
Write-ColorOutput "✅ Git tag created and pushed: $gitTag" "Green"
Write-Host ""

# ============================================================================
# PACK
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "Creating NuGet Package" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

Write-ColorOutput "📦 Running dotnet pack..." "Cyan"
Write-Host ""

# Pack with Release configuration
$outputDir = Join-Path $projectDir "bin\Release"
dotnet pack $ProjectPath --configuration Release --no-build --output $outputDir

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "" "White"
    Write-ColorOutput "❌ ERROR: Pack failed!" "Red"
    
    Write-ColorOutput "" "White"
    Write-ColorOutput "⚠️  Git tag was created. You may want to delete it:" "Yellow"
    Write-ColorOutput "   git tag -d $gitTag" "Gray"
    Write-ColorOutput "   git push origin --delete $gitTag" "Gray"
    
    exit 1
}

Write-Host ""
Write-ColorOutput "✅ Package created successfully" "Green"
Write-Host ""

# ============================================================================
# PUBLISH
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "Publishing to NuGet Registry" "Cyan"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

# Find the package file
$packageFileName = "$packageName.$version.nupkg"
$packagePath = Join-Path $outputDir $packageFileName

if (-not (Test-Path $packagePath)) {
    Write-ColorOutput "❌ ERROR: Package file not found: $packagePath" "Red"
    exit 1
}

Write-ColorOutput "📤 Publishing: $packageFileName" "Cyan"
Write-ColorOutput "   To: $Registry" "Gray"
Write-Host ""

# Push to registry with API key
dotnet nuget push $packagePath --source $Registry --api-key $ApiKey

if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput "" "White"
    Write-ColorOutput "❌ ERROR: Failed to publish package!" "Red"
    Write-ColorOutput "   Possible causes:" "Yellow"
    Write-ColorOutput "   - Invalid API key" "Gray"
    Write-ColorOutput "   - Network connection issues" "Gray"
    Write-ColorOutput "   - Package with same version already exists" "Gray"
    Write-ColorOutput "" "White"
    Write-ColorOutput "⚠️  Git tag was created. You may want to delete it:" "Yellow"
    Write-ColorOutput "   git tag -d $gitTag" "Gray"
    Write-ColorOutput "   git push origin --delete $gitTag" "Gray"
    exit 1
}

Write-Host ""
Write-ColorOutput "✅ Package published successfully" "Green"
Write-Host ""

# ============================================================================
# SUCCESS SUMMARY
# ============================================================================

Write-ColorOutput "================================================" "Cyan"
Write-ColorOutput "✅ RELEASE SUCCESSFUL!" "Green"
Write-ColorOutput "================================================" "Cyan"
Write-Host ""

Write-ColorOutput "📦 Package:      $packageName" "Cyan"
Write-ColorOutput "🏷️  Version:      $version" "Cyan"
Write-ColorOutput "🏷️  Git Tag:      $gitTag" "Cyan"
Write-ColorOutput "📍 Registry:     $Registry" "Cyan"
Write-ColorOutput "🔗 Browse:       https://baget.wittag.net" "Cyan"

if ($isPrerelease) {
    Write-ColorOutput "⚠️  Type:         PRERELEASE" "Yellow"
} else {
    Write-ColorOutput "✅ Type:         STABLE RELEASE" "Green"
}

Write-Host ""
Write-ColorOutput "================================================" "Cyan"

if ($isPrerelease) {
    Write-ColorOutput "Next Steps (Prerelease):" "Yellow"
    Write-Host ""
    Write-ColorOutput "  1. Test in consuming projects:" "White"
    Write-ColorOutput "     dotnet add package $packageName --version $version --source $Registry" "Gray"
    Write-ColorOutput "" "White"
    Write-ColorOutput "  2. Verify functionality thoroughly" "White"
    Write-ColorOutput "" "White"
    Write-ColorOutput "  3. When ready, release stable version:" "White"
    Write-ColorOutput "     - Update .csproj to stable version (e.g., ${version.Split('-')[0]})" "Gray"
    Write-ColorOutput "     - Commit and run build script again" "Gray"
} else {
    Write-ColorOutput "Next Steps (Stable Release):" "Yellow"
    Write-Host ""
    Write-ColorOutput "  1. Update consuming projects:" "White"
    Write-ColorOutput "     dotnet add package $packageName --version $version --source $Registry" "Gray"
    Write-ColorOutput "" "White"
    Write-ColorOutput "  2. Test in consuming projects" "White"
    Write-ColorOutput "" "White"
    Write-ColorOutput "  3. Announce new version to team" "White"
    Write-ColorOutput "" "White"
    Write-ColorOutput "  4. Update version tracking documentation" "White"
}

Write-Host ""
Write-ColorOutput "================================================" "Cyan"
