<#
.Synopsis
	Осуществляет сборку проекта и формирует NuGet-пакеты.
#>
param
(
	[Parameter(HelpMessage = "Путь к шаблонам nuspec-фалов.")]
	[String] $templateDir = 'Files\Packaging\Templates',

	[Parameter(HelpMessage = "Путь к результирующему nuspec-файлу.")]
	[String] $outputDir = 'Assemblies',

	[Parameter(HelpMessage = "Путь к файлу с версией проекта.")]
	[String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

	[Parameter(HelpMessage = "Номер VCS версии проекта.")]
	[String] $commitHash = '',

	[Parameter(HelpMessage = "Режим сборки проекта.")]
	[String] $buildMode = 'Release'
)

# Script dependencies
. (Join-Path $PSScriptRoot 'BuildNuspec.ps1')

# Clean build folder
Remove-Item -Path 'Assemblies' -Recurse -ErrorAction SilentlyContinue

# Restore packages
& "${Env:ProgramFiles(x86)}\NuGet\nuget.exe" restore 'InfinniPlatform.sln' -NonInteractive

# Rebuild solution
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.sln' /t:Clean /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.sln' /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly

Get-ChildItem $templateDir -Filter '*.nuspec' | Foreach-Object {
	$nuspecFile = Join-Path $outputDir $_.Name

	# Build nuspec file
	Build-Nuspec -template $_.FullName -output $nuspecFile -assemblyInfo $assemblyInfo -commitHash $commitHash

	# Build nupkg file
	& "${Env:ProgramFiles(x86)}\NuGet\nuget.exe" pack $nuspecFile -OutputDirectory $outputDir -NoDefaultExcludes -NonInteractive

	# Remove nuspec file
	Remove-Item -Path $nuspecFile
}
