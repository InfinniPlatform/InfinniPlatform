try{
		
	$elasticPath = 'C:\Program Files\elasticsearch'	

	$elasticBinPath = join-path $elasticPath '\bin'

	$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

	if (Get-Service "elasticsearch-service-x64" -ErrorAction SilentlyContinue)
	{
		cd $elasticBinPath
		./service.bat remove
	}
	else
	{
		Write-Host "ElasticSearch is not installed"
	}
}
finally{
	cd $scriptPath
}