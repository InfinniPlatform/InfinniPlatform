xcopy ..\Gameshop_1.0.0.0\* ..\Assemblies\Gameshop_1.0.0.0\ /s /y /r
pushd ..\Assemblies
InfinniPlatform.Utils.exe upload Gameshop_1.0.0.0 withMetadata
popd