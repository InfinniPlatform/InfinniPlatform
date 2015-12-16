Param ([string]$Version)
$Hash=git rev-parse HEAD

$VersionStub = "{VERSION}"
$HashStub = "{COMMIT}"

(Get-Content ".\Templates\InfinniPlatform.Template.nuspec") | 
Foreach-Object {$_ -replace $VersionStub, $Version} |
Foreach-Object {$_ -replace $HashStub, $Hash} | 
Set-Content ".\InfinniPlatform.nuspec"

Write-Host "InfinniPlatform version ($Version) is set."
Write-Host "InfinniPlatform commit hash ($Hash) is set."