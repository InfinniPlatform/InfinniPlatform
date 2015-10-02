::Copy metadata
xcopy SystemConfiguration_1.0.0.0\* Assemblies\SystemConfiguration_1.0.0.0\ /s /y

::Upload assemblies and metadata
pushd Assemblies
InfinniPlatform.Utils.exe upload SystemConfiguration_1.0.0.0 withMetadata
popd