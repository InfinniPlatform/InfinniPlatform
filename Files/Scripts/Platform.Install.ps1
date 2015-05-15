. ".\Security.Certificates.ps1"
. ".\Security.Http.ps1"

# 1. Импорт self-signed сертификата платформы
$thumbprint = Certificate-Import -file "..\Certificates\InfinniPlatform.pfx" -password "InfinniPlatform" -storeName "My"
$thumbprint = Certificate-Import -file "..\Certificates\InfinniPlatform.pfx" -password "InfinniPlatform" -storeName "AuthRoot"

# 2. Добавление алиаса в файл hosts
Http-SetHostAddress -ip "127.0.0.1" -hostName "InfinniPlatform" -replaceIp $FALSE
