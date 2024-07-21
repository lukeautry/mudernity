param(
    [string]$BumpType = "patch"
)

# Hardcoded path to your .csproj file
$FilePath = "Agent\Agent.csproj"

function Get-VersionSegments {
    param ([string]$version)
    return $version -split '\.'
}

function Increment-Version {
    param (
        [string[]]$segments,
        [string]$bumpType
    )

    switch ($bumpType.ToLower()) {
        "major" {
            $segments[0] = [int]$segments[0] + 1
            $segments[1] = 0
            $segments[2] = 0
        }
        "minor" {
            $segments[1] = [int]$segments[1] + 1
            $segments[2] = 0
        }
        "patch" {
            $segments[2] = [int]$segments[2] + 1
        }
        default {
            Write-Error "Invalid bump type. Use 'major', 'minor', or 'patch'."
            exit 1
        }
    }

    return $segments -join '.'
}

# Load the XML from the .csproj file
$xml = [xml](Get-Content $FilePath)
$propertyGroup = $xml.Project.PropertyGroup

# Get the current version
$currentVersion = $propertyGroup.Version

if (-not $currentVersion) {
    Write-Error "Version not found in the .csproj file."
    exit 1
}

# Split the version into segments and increment the appropriate one
$versionSegments = Get-VersionSegments -version $currentVersion
$newVersion = Increment-Version -segments $versionSegments -bumpType $BumpType

# Update the version in the .csproj file
$propertyGroup.Version = $newVersion
$propertyGroup.AssemblyVersion = "$newVersion.0"
$propertyGroup.FileVersion = "$newVersion.0"
$propertyGroup.InformationalVersion = $newVersion

# Save the changes back to the .csproj file
$xml.Save($FilePath)

Write-Output "Updated version to $newVersion in $FilePath"
