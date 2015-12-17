function Build-Nuspec
{
	<#
	.Synopsis
		Создает nuspec-файл по шаблону на основе версии проекта.
	#>
	param
	(
		[Parameter(HelpMessage = "Путь к шаблону nuspec-фала.")]
		[String] $template = 'Files\Packaging\Templates\InfinniPlatform.Template.nuspec',

		[Parameter(HelpMessage = "Путь к результирующему nuspec-файлу.")]
		[String] $output = 'Assemblies\InfinniPlatform.nuspec',

		[Parameter(HelpMessage = "Путь к файлу с версией проекта.")]
		[String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

		[Parameter(HelpMessage = "Номер VCS версии проекта.")]
		[String] $commitHash = ''
	)

	process 
	{
		$version = Get-Content $assemblyInfo `
			| Select-String -Pattern 'AssemblyVersion\s*\(\s*\"(?<version>.*?)\"\s*\)' `
			| ForEach-Object { $_.Matches[0].Groups['version'].Value }

		Build-NuspecByValues -template $template -output $output -values @{ '{VERSION}' = $version; '{COMMIT}' = $commitHash }
	}
}

function Build-NuspecByValues
{
	<#
	.Synopsis
		Создает nuspec-файл по шаблону на основе указанных значений.
	#>
	param
	(
		[Parameter(HelpMessage = "Путь к шаблону nuspec-фала.")]
		[String] $template = 'Files\Packaging\Templates\InfinniPlatform.Template.nuspec',

		[Parameter(HelpMessage = "Путь к результирующему nuspec-файлу.")]
		[String] $output = 'Assemblies\InfinniPlatform.nuspec',

		[Parameter(HelpMessage = "Значения для подстановок в шаблон nuspec-файла.")]
		[Hashtable] $values = @{ '{VERSION}' = '1.0.0.0'; '{COMMIT}' = '' }
	)

	process 
	{
		$content = Get-Content -Path $template

		foreach ($item in $values.GetEnumerator())
		{
			$content = $content | Foreach-Object {$_ -replace $item.Name, $item.Value}
		}

		Set-Content -Path $output -Value $content
	}
}