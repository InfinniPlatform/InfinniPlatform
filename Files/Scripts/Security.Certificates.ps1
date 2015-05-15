function Certificate-Create
{
	<#
	.Synopsis
		Создает self-signed сертификат и сохраняет его в указанный файл.
	#>
	param
	(
		[Parameter(HelpMessage = "Путь к файлу сертификата.")]
		[String] $file = "InfinniPlatform.pfx",

		[Parameter(HelpMessage = "Пароль сертификата.")]
		[String] $password = "InfinniPlatform",

		[Parameter(HelpMessage = "Общее имя сертификата (CN).")]
		[String] $commonName = "InfinniPlatform",

		[Parameter(HelpMessage = "Альтернативные имена сертификата (SAN).")]
		[String[]] $alternativeNames = @( "InfinniPlatform", "localhost" ),

		[Parameter(HelpMessage = "Страна.")]
		[String] $countryName = "RU",

		[Parameter(HelpMessage = "Организация.")]
		[String] $organizationName = "Infinnity Solutions",

		[Parameter(HelpMessage = "Подразделение.")]
		[String] $organizationalUnitName = "InfinniPlatform"
	)

	process 
	{
		# 1. Генерация OpenSsl Configuration File

		$openSslConfig = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("openssl.cfg")
		$writer = New-Object System.IO.StreamWriter($openSslConfig)
		$writer.WriteLine("[req]")
		$writer.WriteLine("distinguished_name = req_distinguished_name")
		$writer.WriteLine("x509_extensions    = v3_req")
		$writer.WriteLine("prompt             = no")
		$writer.WriteLine("")
		$writer.WriteLine("[req_distinguished_name]")
		$writer.WriteLine("C  = $countryName")
		$writer.WriteLine("O  = $organizationName")
		$writer.WriteLine("OU = $organizationalUnitName")
		$writer.WriteLine("CN = $commonName")
		$writer.WriteLine("")
		$writer.WriteLine("[v3_req]")
		$writer.WriteLine("keyUsage         = keyEncipherment, dataEncipherment")
		$writer.WriteLine("extendedKeyUsage = serverAuth")
		$writer.WriteLine("subjectAltName   = @alt_names")
		$writer.WriteLine("")
		$writer.WriteLine("[alt_names]")

		$index = 1;

		foreach ($dnsName in $alternativeNames)
		{
			$writer.WriteLine("DNS.{0} = {1}", $index, $dnsName);
			$index = $index + 1;
		}

		$writer.Close()

		# 2. Генерация Certificate Request File (CSR)

		$file = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($file)
		$csrFile = $ExecutionContext.SessionState.Path.Combine([System.IO.Path]::GetDirectoryName($file), "certificate.csr")
		$openSslExe = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("..\Tools\OpenSsl\openssl.exe")
		Start-Process -Wait -NoNewWindow -FilePath $openSslExe "req -x509 -nodes -days 3650 -newkey rsa:2048 -keyout ""$csrFile"" -out ""$csrFile"" -config ""$openSslConfig"""

		# 3. Генерация сертификата (PFX)

		Start-Process -Wait -NoNewWindow -FilePath $openSslExe "pkcs12 -export -out ""$file"" -in ""$csrFile"" -name ""$commonName"" -passout pass:$password"

		# 4. Удаление временных файлов

		Remove-Item $openSslConfig
		Remove-Item $csrFile
	}
}

function Certificate-Import
{
	<#
	.Synopsis
		Импортирует сертификат из указанного файла.
	.ReturnValue
		Отпечаток сертификата.
	#>
	param
	(
		[Parameter(HelpMessage = "Путь к файлу сертификата.")]
		[String] $file = "InfinniPlatform.pfx",

		[Parameter(HelpMessage = "Пароль сертификата.")]
		[String] $password = "InfinniPlatform",

		[Parameter(HelpMessage = "Имя хранилища.")]
		[String] $storeName = "My"
	)

	begin
	{
		[void][System.Reflection.Assembly]::LoadWithPartialName("System.Security")
	}

	process 
	{
		# Открытие сертификата

		$flags = [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable `
				-bor [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::MachineKeySet `
				-bor [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::PersistKeySet `
				-bor [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::UserKeySet;

                $file = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($file)
		$certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($file, $password, $flags)

		# Импорт сертификата в хранилище
		$store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine)
		$store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
		$store.Add($certificate)
		$store.Close()

		return $certificate.Thumbprint.ToLower();
	}
}