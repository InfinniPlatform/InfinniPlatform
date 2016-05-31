function Create-Nuspec
{
	<#
	.Synopsis
		Создает шаблоны nuspec-файлов на основе файлов проекта.
	#>
	param
	(
		[Parameter(HelpMessage = "Каталог решения.")]
		[String] $solutionDir = '',

		[Parameter(HelpMessage = "Каталог результатов.")]
		[String] $outputDir = 'Assemblies',

		[Parameter(HelpMessage = "Путь к файлу с версией проекта.")]
		[String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

		[Parameter(HelpMessage = "Ветка VCS версии проекта.")]
		[String] $branchName = '',

		[Parameter(HelpMessage = "Номер VCS версии проекта.")]
		[String] $commitHash = '',

		[Parameter(HelpMessage = "Версия .NET.")]
		[String] $framework = 'net45'
	)

	process
	{
		### Retrieve version for packages

		$version = Get-Content $assemblyInfo `
			| Select-String -Pattern 'AssemblyVersion\s*\(\s*\"(?<version>.*?)\"\s*\)' `
			| ForEach-Object { $_.Matches[0].Groups['version'].Value }

		if ($branchName -and $branchName -notlike 'release-*')
		{
			$version = $version + '-' + ($branchName -replace '^(refs/heads/){0,1}(f\-){0,1}', '')
		}

		### Create nuspec for all projects

		$projects = Get-ChildItem -Path $solutionDir -Filter '*.csproj' -Exclude '*.Tests.csproj' -Recurse
		$references = @()

		foreach ($project in $projects)
		{
			[xml] $projectXml = Get-Content $project

			$projectName = (Get-ChildItem $project).BaseName
			$projectAssemblyName = ($projectXml.Project.PropertyGroup.AssemblyName[0])

			Write-Host "Create $projectName.nuspec"

			$projectNuspec = 
				"<?xml version=""1.0"" encoding=""utf-8""?>`r`n" + `
				"<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">`r`n" + `
				"    <metadata>`r`n" + `
				"        <id>$projectName</id>`r`n" + `
				"        <version>$version</version>`r`n" + `
				"        <authors>Infinnity Solutions</authors>`r`n" + `
				"        <owners>Infinnity Solutions</owners>`r`n" + `
				"        <requireLicenseAcceptance>false</requireLicenseAcceptance>`r`n" + `
				"        <description>Commit $commitHash</description>`r`n" + `
				"        <copyright>Infinnity Solutions $(Get-Date -Format yyyy)</copyright>`r`n" + `
				"        <dependencies>`r`n"

			# NuGet packages

			$projectPackages = $projectXml.Project.ItemGroup.Reference.HintPath | Where { $_ -like '..\packages\*.dll' }
			$dependencies = $projectPackages | % { $_ -replace '\\[^\\]+\.dll', '' } | Sort-Object | Get-Unique -AsString
			$references += $projectPackages | % { $_ -replace '^\.\.\\packages\\', '' }

			if ($dependencies)
			{
				foreach ($item in $dependencies)
				{
					$result = $item -match '^\.\.\\packages\\(?<name>.*?)\.(?<version>([0-9]+\.[0-9\.]+)|([0-9]+\.[0-9\.]+\-.*?))\\lib($|(\\.*?))'

					$projectNuspec = $projectNuspec + `
						"            <dependency id=""$($matches.name)"" version=""$($matches.version)"" />`r`n"
				}
			}

			# Project references

			$dependencies = $projectXml.Project.ItemGroup.ProjectReference.Name | Sort-Object | Get-Unique -AsString

			if ($dependencies)
			{
				foreach ($item in $dependencies)
				{
					$projectNuspec = $projectNuspec + "            <dependency id=""$item"" version=""$version"" />`r`n"
				}
			}

			$projectNuspec = $projectNuspec + `
				"        </dependencies>`r`n" + `
				"    </metadata>`r`n" + `
				"    <files>`r`n"

			# Project assembly

			$projectIsLibrary = $projectXml.Project.PropertyGroup.OutputType -like '*Library*'
			$projectAssembly = $projectAssemblyName + $(if ($projectIsLibrary) { '.dll' } else { '.exe' })
			$projectNuspec = $projectNuspec + "        <file target=""lib\$framework\$projectAssembly"" src=""$projectAssembly"" />`r`n"
			$references += "$projectName.$version\lib\$framework\$projectAssembly"

			# Project resources for ru-RU

			$projectResourcesRu = $projectXml.Project.ItemGroup.EmbeddedResource.Include | Where { $_ -like '*.ru-RU.*' }

			if ($projectResourcesRu -and $projectResourcesRu.Count -gt 0 -and $projectResourcesRu[0])
			{
				$projectNuspec = $projectNuspec + "        <file target=""lib\$framework\ru-RU"" src=""ru-RU\$projectAssemblyName.resources.dll"" />`r`n"
				$references += "$projectName.$version\lib\$framework\ru-RU\$projectAssemblyName.resources.dll"
			}

			# Project resources for en-US

			$projectResourcesEn = $projectXml.Project.ItemGroup.EmbeddedResource.Include | Where { $_ -like '*.en-US.*' }

			if ($projectResourcesEn -and $projectResourcesEn.Count -gt 0 -and $projectResourcesEn[0])
			{
				$projectNuspec = $projectNuspec + "        <file target=""lib\$framework\en-US"" src=""en-US\$projectAssemblyName.resources.dll"" />`r`n"
				$references += "$projectName.$version\lib\$framework\en-US\$projectAssemblyName.resources.dll"
			}

			# Project symbols

			$projectNuspec = $projectNuspec + "        <file target=""lib\$framework"" src=""$projectAssemblyName.pdb"" />`r`n"

			# Project docs

			$projectDocs = $projectXml.Project.PropertyGroup.DocumentationFile

			if ($projectDocs -and $projectDocs.Count -gt 0 -and $projectDocs[0])
			{
				$projectNuspec = $projectNuspec + "        <file target=""lib\$framework"" src=""$projectAssemblyName.xml"" />`r`n"
			}

			# Project config

			if (-not $projectIsLibrary)
			{
				$projectNuspec = $projectNuspec + "        <file target=""lib\$framework"" src=""$projectAssemblyName.exe.config"" />`r`n"
				$references += "$projectName.$version\lib\$framework\$projectAssemblyName.exe.config"
			}

			$projectNuspec = $projectNuspec + `
				"        <file target=""src"" src=""..\$projectName\**\*.cs"" exclude=""..\$projectName\obj\**\*.cs"" />`r`n" + `
				"    </files>`r`n" + `
				"</package>"

			Set-Content (Join-Path $outputDir ($projectName + '.nuspec')) -Value $projectNuspec
		}

		### Create nuspec for solution

		Write-Host "Create InfinniPlatform.nuspec"

		$solutionNuspec = 
			"<?xml version=""1.0"" encoding=""utf-8""?>`r`n" + `
			"<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">`r`n" + `
			"    <metadata>`r`n" + `
			"        <id>InfinniPlatform</id>`r`n" + `
			"        <version>$version</version>`r`n" + `
			"        <authors>Infinnity Solutions</authors>`r`n" + `
			"        <owners>Infinnity Solutions</owners>`r`n" + `
			"        <requireLicenseAcceptance>false</requireLicenseAcceptance>`r`n" + `
			"        <description>Commit hash: $commitHash</description>`r`n" + `
			"        <copyright>Infinnity Solutions $(Get-Date -Format yyyy)</copyright>`r`n" + `
			"        <dependencies>`r`n"

		foreach ($project in $projects)
		{
			$projectName = (Get-ChildItem $project).BaseName

			$solutionNuspec = $solutionNuspec + "            <dependency id=""$projectName"" version=""$version"" />`r`n"
		}

		$solutionNuspec = $solutionNuspec + `
			"        </dependencies>`r`n" + `
			"    </metadata>`r`n" + `
			"    <files>`r`n" + `
			"        <file target=""content\metadata\Authorization"" src=""content\InfinniPlatform\metadata\Authorization\**\*.*"" />`r`n" + `
			"        <file target=""lib\$framework\App.config"" src=""App.config"" />`r`n" + `
			"        <file target=""lib\$framework\AppLog.config"" src=""AppLog.config"" />`r`n" + `
			"        <file target=""lib\$framework\AppCommon.json"" src=""AppCommon.json"" />`r`n" + `
			"        <file target=""lib\$framework\AppExtension.json"" src=""AppExtension.json"" />`r`n" + `
			"        <file target=""lib\$framework\references.lock"" src=""references.lock"" />`r`n" + `
			"    </files>`r`n" + `
			"</package>"

		Set-Content (Join-Path $outputDir 'InfinniPlatform.nuspec') -Value $solutionNuspec

		### Create file with solution references

		Set-Content (Join-Path $outputDir 'references.lock') -Value ($references | Sort-Object | Get-Unique -AsString)
	}
}