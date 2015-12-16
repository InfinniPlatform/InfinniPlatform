function Build-Nuspec
{
	<#
	.Synopsis
		������� nuspec-���� �� ������� �� ������ ������ �������.
	#>
	param
	(
		[Parameter(HelpMessage = "���� � ������� nuspec-����.")]
		[String] $template = 'Files\Packaging\Templates\InfinniPlatform.Template.nuspec',

		[Parameter(HelpMessage = "���� � ��������������� nuspec-�����.")]
		[String] $output = 'Assemblies\InfinniPlatform.nuspec',

		[Parameter(HelpMessage = "���� � ����� � ������� �������.")]
		[String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

		[Parameter(HelpMessage = "����� VCS ������ �������.")]
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
		������� nuspec-���� �� ������� �� ������ ��������� ��������.
	#>
	param
	(
		[Parameter(HelpMessage = "���� � ������� nuspec-����.")]
		[String] $template = 'Files\Packaging\Templates\InfinniPlatform.Template.nuspec',

		[Parameter(HelpMessage = "���� � ��������������� nuspec-�����.")]
		[String] $output = 'Assemblies\InfinniPlatform.nuspec',

		[Parameter(HelpMessage = "�������� ��� ����������� � ������ nuspec-�����.")]
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