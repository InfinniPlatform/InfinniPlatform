::Copy metadata
xcopy SystemConfiguration_1.0.0.0\* Assemblies\SystemConfiguration_1.0.0.0\ /s /y

::Start ServiceHost
start /d ..\InfinniPlatform\Assemblies\ InfinniPlatform.ServiceHost.exe

::Wait 5 sec till ServiceHost start
timeout 5

::Upload assemblies and metadata
pushd Assemblies
InfinniPlatform.Utils.exe upload SystemConfiguration_1.0.0.0 withMetadata
popd