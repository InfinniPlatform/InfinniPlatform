Param ([string]$Version)

$NuspecFilePath = ".\InfinniPlatform.nuspec"
$Stub = "{INFINNI_PLATFORM_VERSION}"

(Get-Content $NuspecFilePath) | 
Foreach-Object {$_ -replace $Stub, $Version} | 
Set-Content $NuspecFilePath

Write-Host "InfinniPlatform $Version is set."    