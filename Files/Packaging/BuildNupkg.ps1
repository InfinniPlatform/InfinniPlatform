<#
.Synopsis
	Осуществляет сборку проекта и формирует NuGet-пакеты.
#>
param
(
	[Parameter(HelpMessage = "Каталог решения.")]
	[String] $solutionDir = '.',

	[Parameter(HelpMessage = "Каталог результатов.")]
	[String] $outputDir = 'Assemblies',

	[Parameter(HelpMessage = "Путь к файлу с версией проекта.")]
	[String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

	[Parameter(HelpMessage = "Ветка VCS версии проекта.")]
	[String] $branchName = '',

	[Parameter(HelpMessage = "Номер VCS версии проекта.")]
	[String] $commitHash = '',

	[Parameter(HelpMessage = "Режим сборки проекта.")]
	[String] $buildMode = 'Release'
)

# Зависимости скрипта
. (Join-Path $PSScriptRoot 'CreateNuspec.ps1')
. (Join-Path $PSScriptRoot 'BuildSymbols.ps1')

# Очистка результатов
Remove-Item -Path $outputDir -Recurse -ErrorAction SilentlyContinue

# Установка NuGet

$nugetDir = Join-Path $env:ProgramData 'NuGet'
$nugetPath = Join-Path $nugetDir 'nuget.exe'

if (-not (Test-Path $nugetPath))
{
	if (-not (Test-Path $nugetDir))
	{
		New-Item $nugetDir -ItemType Directory -ErrorAction SilentlyContinue
	}

	$nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
	Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
}

# Восстановление пакетов
& "$nugetPath" restore 'InfinniPlatform.sln' -NonInteractive

# Сборка проектов решения
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.sln' /t:Clean /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly
& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" 'InfinniPlatform.sln' /p:Configuration=$buildMode /verbosity:quiet /consoleloggerparameters:ErrorsOnly

# Создание nuspec-файлов
Create-Nuspec `
	-solutionDir $solutionDir `
	-outputDir $outputDir `
	-assemblyInfo $assemblyInfo `
	-branchName $branchName `
	-commitHash $commitHash

Get-ChildItem $outputDir -Filter '*.nuspec' | Foreach-Object {
	$nuspecFile = Join-Path $outputDir $_.Name

	# Создание nupkg-файлов
	& "$nugetPath" pack $nuspecFile -OutputDirectory $outputDir -NoDefaultExcludes -NonInteractive

	# Удаление nuspec-файлов
	Remove-Item -Path $nuspecFile
}