xcopy SystemConfiguration\* Assemblies\ /s /y
pushd Assemblies
InfinniPlatform.Utils.exe upload Administration,Authorization,AdministrationCustomization withMetadata
popd