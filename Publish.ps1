$collection = @(
  # "InfinniPlatform.ServiceHost",
  "InfinniPlatform.Sdk"
  "InfinniPlatform.Core",
  "InfinniPlatform.Http",
  "InfinniPlatform.Auth.Adfs",
  "InfinniPlatform.Auth.Cookie",
  "InfinniPlatform.Auth.Facebook",
  "InfinniPlatform.Auth.Google",
  "InfinniPlatform.Auth.Internal",
  "InfinniPlatform.MessageQueue",
  "InfinniPlatform.DocumentStorage",
  "InfinniPlatform.Caching",
  "InfinniPlatform.BlobStorage"
)

foreach ($item in $collection) {
  Push-Location $item

  dotnet.exe publish
 
  Copy-Item "bin\Debug\netstandard1.6\publish\*" "C:\Projects\InfinniPlatform\Assemblies" -Recurse -Force

  Pop-Location
}

Write-Host "Complete!"