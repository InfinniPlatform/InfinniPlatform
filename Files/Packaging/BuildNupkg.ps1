<#
.Synopsis
    Builds solution and creates NuGet packages.
#>
param
(
  [Parameter(HelpMessage = "Path to the solution directory.")]
  [String] $solutionDir = '.',

  [Parameter(HelpMessage = "Path to the solution output directory.")]
  [String] $outputDir = 'Assemblies',

  [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
  [String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

  [Parameter(HelpMessage = "VCS branch name.")]
  [String] $branchName = 'release-1.11',

  [Parameter(HelpMessage = "VCS commit hash.")]
  [String] $commitHash = '',

  [Parameter(HelpMessage = "Build mode.")]
  [String] $buildMode = 'Release'
)

# Script dependencies
. (Join-Path $PSScriptRoot 'CreateNuspec.ps1')
. (Join-Path $PSScriptRoot 'BuildSymbols.ps1')

# Clear output directory
Remove-Item -Path $outputDir -Recurse -ErrorAction SilentlyContinue

# Install NuGet package manager

$nugetDir = Join-Path $env:ProgramData 'NuGet'
$nugetPath = Join-Path $nugetDir 'nuget.exe'

if (-not (Test-Path $nugetPath)) {
  if (-not (Test-Path $nugetDir)) {
    New-Item $nugetDir -ItemType Directory -ErrorAction SilentlyContinue
  }

  $nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
  Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
}

# Restore packages
& "$nugetPath" restore 'InfinniPlatform.sln' -NonInteractive

# Build the solution
$msbuildPath = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"

if (-not (Test-Path $msbuildPath)) {
  Write-Error "Build Tools 2017 must be installed."
}
else {
  & $msbuildPath 'InfinniPlatform.sln' /t:Clean /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly /m
  & $msbuildPath 'InfinniPlatform.sln' /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly /m

  # Create nuspec-files
  Create-Nuspec `
    -solutionDir $solutionDir `
    -outputDir $outputDir `
    -assemblyInfo $assemblyInfo `
    -branchName $branchName `
    -commitHash $commitHash

  Get-ChildItem $outputDir -Filter '*.nuspec' | Foreach-Object {
    $nuspecFile = Join-Path $outputDir $_.Name

    # Create nupkg-file
    & "$nugetPath" pack $nuspecFile -OutputDirectory $outputDir -NoDefaultExcludes -NonInteractive

    # Remove nuspec-file
    Remove-Item -Path $nuspecFile
  }
}