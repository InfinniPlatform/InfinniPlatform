function Http-BindCertificate
{
	<#
	.Synopsis
		Связывает адрес и порт с указанным сертификатом.
	#>
	param
	(
		[Parameter(HelpMessage = "IP-адрес для связки с сертификатом.")]
		[String] $ip = "0.0.0.0",

		[Parameter(HelpMessage = "Порт для связки с сертификатом.")]
		[Int] $port = 9900,

		[Parameter(HelpMessage = "Отпечаток сертификата.")]
		[String] $thumbprint
	)

	process 
	{
		Start-Process -Wait -WindowStyle Hidden -FilePath netsh "http add sslcert ipport=$ip`:$port certhash=$thumbprint appid={a6c54cea-f380-42d3-b1d5-d71d2de1a02f}"
	}
}

function Http-SetHostAddress
{
	<#
	.Synopsis
		Устанавливает IP-адрес для заданного имени хоста.
	#>
	param
	(
		[Parameter(HelpMessage = "IP-адрес.")]
		[String] $ip = "127.0.0.1",

		[Parameter(HelpMessage = "Имя хоста.")]
		[String] $hostName = "InfinniPlatform",

		[Parameter(HelpMessage = "Заменить IP-адрес для существующего хоста.")]
		[Bool] $replaceIp = $FALSE
	)

	process 
	{
		# 1. Формирование нового содержимого для файла hosts

		$found = $FALSE
		$newHosts = @()
		$hostsFile = "$env:windir\System32\drivers\etc\hosts"
		$currentHosts = Get-Content $hostsFile

		foreach ($line in $currentHosts)
		{
			$match = [System.Text.RegularExpressions.Regex]::Match($line, "^\s*(?<ip>[0-9.]+)\s+(?<hostName>[a-zA-Zа-яА-Я0-9.-_]+)(\s+|$)")

			if ($match.Success)
			{
				$currentIp = $match.Groups["ip"].Value
				$currentHostName = $match.Groups["hostName"].Value

				if ($currentHostName.Equals($hostName, [System.StringComparison]::InvariantCultureIgnoreCase))
				{
					if ($replaceIp)
					{
						$line = [System.String]::Format("{0}	{1}", $ip, $hostName)
					}

					$found = $TRUE
				}
			}

			$newHosts += $line
		}

		if (-not $found)
		{
			$newHosts += [System.String]::Format("{0}	{1}", $ip, $hostName)
		}

		# 2. Перезапись файла hosts новым содержимым

		$writer = New-Object System.IO.StreamWriter($hostsFile)

		foreach ($line in $newHosts)
		{
			$writer.WriteLine($line)
		}

		$writer.Close()
	}
}