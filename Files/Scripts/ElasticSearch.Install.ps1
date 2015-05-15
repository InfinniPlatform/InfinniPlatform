$clusterName = 'InfinniPlatform_'
$elasticPath = 'C:\Program Files\elasticsearch'

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

try {

    $clusterName = $clusterName + ([system.guid]::newguid())
                        
    $elasticBinPath = join-path $elasticPath '\bin'
    $configPath = join-path $elasticPath  '\config\elasticsearch.yml'
    $hostName = [System.Net.Dns]::GetHostName()		
    $esAreadyInstalled = $FALSE	
        
    if (-not (test-path $elasticBinPath))
    {			
        write-host "	ElasticSearch bin path doesn't exist: $elasticBinPath. Download and extract elasticsearch from http://www.elasticsearch.org/downloads/1-1-1/"  -foregroundcolor red -background black	
        return		
    }
                
    if ([environment]::GetEnvironmentVariable("JAVA_HOME","machine") -eq $null)
    {
        write-host "JAVA_HOME variable not set"
    }
                
    #------------------------------------
    #Setting cluster name
    #------------------------------------
    Write-Host "1. Setting cluster name" -foregroundcolor green 
        
    if($clusterName -eq $null)
    {
        $clusterName = "elasticsearch_$hostName"
    }
    (Get-Content $configPath) | ForEach-Object { $_ -replace "#?\s?cluster.name: .+" , "cluster.name: $clusterName" } | Set-Content $configPath
        Write-Host "	Cluster name is: $clusterName"   -foregroundcolor gray	
         
    #-----------------------------------
    #Add custom analyzers
    #------------------------------------
    Add-Content -Path $configPath -Value "index:`r`n    analysis:`r`n        analyzer:"
    Add-Content -Path $configPath -Value "#стандартный анализатор, использующийся для поиска по умолчанию"
    Add-Content -Path $configPath -Value "            string_lowercase:`r`n                filter: lowercase`r`n                tokenizer: keyword"
    Add-Content -Path $configPath -Value "#стандартный анализатор, использующийся для индексирования по умолчанию"
    Add-Content -Path $configPath -Value "            keywordbasedsearch:`r`n                filter: lowercase`r`n                tokenizer: keyword"
    Add-Content -Path $configPath -Value "#полнотекстовый анализатор, использующийся для индексирования"
    Add-Content -Path $configPath -Value "            fulltextsearch:`r`n                filter: lowercase`r`n                tokenizer: standard"
    Add-Content -Path $configPath -Value "#Анализатор с разбиением на слова, использующийся для полнотекстового поиска"
    Add-Content -Path $configPath -Value "            fulltextquery:`r`n                filter: lowercase`r`n                tokenizer: whitespace"
        
    #------------------------------------
    #Install ElasticSearch as a service
    #------------------------------------		
    Write-Host "2. Install ElasticSearch as a service" -foregroundcolor green 
    cd $elasticBinPath 
    if (-not (Get-Service "elasticsearch-service-x64" -ErrorAction SilentlyContinue) )
    {
        .\service.bat install
        Write-Host "	ElasticSearch installed." -foregroundcolor gray
    }
    else
    {
        Write-Host "	ElasticSearch have been already installed." -foregroundcolor gray
        $esAreadyInstalled = $TRUE
    }
         
    #------------------------------------
    #Start ElasticSearch service
    #------------------------------------			
    Write-Host "3. Start ElasticSearch service" -foregroundcolor green 
    Start-Service 'elasticsearch-service-x64'
    Write-Host "	ElasticSearch service status: " (service "elasticsearch-service-x64").Status   -foregroundcolor gray
                 
    set-service 'elasticsearch-service-x64' -startuptype automatic
    Write-Host "	ElasticSearch service startup type: " (Get-WmiObject -Class Win32_Service -Property StartMode -Filter "Name='elasticsearch-service-x64'").StartMode  -foregroundcolor gray
    if(-not $esAreadyInstalled ){
            Write-Host "	Now 20s for ElasticSearch to start"   -foregroundcolor yellow
            Start-Sleep -s 20
    }
    Write-Host
        
         
    #------------------------------------
    #Sanity  check.
    #------------------------------------			 
        Write-Host "4. Sanity  check. " -foregroundcolor green 
        
        $esRequest = [System.Net.WebRequest]::Create("http://localhost:9200")
        $esRequest.Method = "GET"
        $esResponse = $esRequest.GetResponse()
        $reader = new-object System.IO.StreamReader($esResponse.GetResponseStream())
        Write-Host "	ElasticSearch service response status: " $esResponse.StatusCode   -foregroundcolor gray
        Write-Host "	ElasticSearch service response full text: " $reader.ReadToEnd()   -foregroundcolor gray
        Write-Host

	#------------------------------------
    #Install ElasticSearch viewer
    #------------------------------------		
    Write-Host "5. Install ElasticSearch viewer" -foregroundcolor green 
    cd $elasticBinPath 
    
    .\plugin.bat install mobz/elasticsearch-head
    Write-Host "	ElasticSearch viewer installed." -foregroundcolor gray
    
         
    #------------------------------------
    #Ending notes.
    #------------------------------------		 
        Write-Host "ElasticSerach service endopoint: http://localhost:9200" -foregroundcolor green 
        write-host
        write-host
        Write-Host "Done!" -foregroundcolor green 
}
finally{
		cd $scriptPath
}