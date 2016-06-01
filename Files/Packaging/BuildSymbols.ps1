function Build-Symbols
{
	<#
	.Synopsis
		Включает в pdb-файлы проекта информацию о источнике кода.
	#>
	param
	(
		[Parameter(HelpMessage = "Каталог решения.")]
		[String] $solutionDir = '.',

		[Parameter(HelpMessage = "Адрес VCS проекта.")]
		[String] $repositoryUrl = '',

		[Parameter(HelpMessage = "Номер VCS версии проекта.")]
		[String] $commitHash = ''
	)

	process
	{
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

		# Установка GitLink

		$gitLinkDir = Join-Path $env:ProgramData 'GitLink'
		$gitLinkPath = Join-Path $gitLinkDir 'lib\net45\GitLink.exe'

		if (-not (Test-Path $gitLinkPath))
		{
			& "$nugetPath" install 'GitLink' -OutputDirectory $env:ProgramData -NonInteractive -Prerelease -ExcludeVersion
		}

		# Сборка символьных файлов

		if ($repositoryUrl -like '*.git')
		{
			$repositoryUrl = $repositoryUrl -replace '\.git$', ''
		}

		& "$gitLinkPath" $solutionDir -u $repositoryUrl -s $commitHash
	}
}