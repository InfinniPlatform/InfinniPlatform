xcopy SystemConfiguration_1.0.0.0\* Assemblies\SystemConfiguration_1.0.0.0 /s /y
pushd Assemblies
InfinniPlatform.Utils.exe upload SystemConfiguration_1.0.0.0 withMetadata
popd