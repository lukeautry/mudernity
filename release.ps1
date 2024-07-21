## Build the client app
cd client

npm install

npm run build

cd ..

# Define the list of runtime identifiers (RIDs) for the platforms you want to publish
$runtimes = @(
    "win-x64",
    "linux-x64",
    "osx-x64"
)

# Define the path to your .csproj file
$projectPath = "Agent/Agent.csproj"

# Define the output directory for the published binaries
$outputDir = "builds"

# Ensure the output directory exists
if (-Not (Test-Path -Path $outputDir)) {
    New-Item -Path $outputDir -ItemType Directory
}

# Loop through each runtime identifier and publish the project
foreach ($runtime in $runtimes) {
    Write-Output "Publishing for runtime: $runtime"

    $publishDir = Join-Path -Path $outputDir -ChildPath $runtime

    dotnet publish $projectPath `
        -c Release `
        -r $runtime `
        --output $publishDir `
        /p:DebugType=None `
        /p:DebugSymbols=false `
        /p:AssemblyName=mudernity

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publishing failed for runtime: $runtime"
        exit $LASTEXITCODE
    }

    # Move the file into the output directory, but with a different name
    # windows - stay the same actually
    # linux - mudernity-linux-x64
    # osx - mudernity-osx-x64
    if ($runtime -eq "linux-x64" -or $runtime -eq "osx-x64") {
        Move-Item -Path "$publishDir/mudernity" -Destination "$outputDir/mudernity-$runtime"
    } else {
        Move-Item -Path "$publishDir/mudernity.exe" -Destination "$outputDir"
    }

    # delete the publish directory
    Remove-Item -Path $publishDir -Recurse

    Write-Output "Published for runtime: $runtime to $publishDir"
}

Write-Output "Publishing completed successfully."
