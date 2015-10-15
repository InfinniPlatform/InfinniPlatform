Param ([string]$Version)

$Stub = "{INFINNI_PLATFORM_VERSION}"

(Get-Content ".\Templates\InfinniPlatform.Template.nuspec") | 
Foreach-Object {$_ -replace $Stub, $Version} | 
Set-Content ".\InfinniPlatform.nuspec"

Write-Host "InfinniPlatform $Version is set."    