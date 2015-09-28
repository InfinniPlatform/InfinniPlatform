pushd ..

::Start ServiceHost
start InfinniPlatform.ServiceHost.exe

::Wait till ServiceHost started
timeout 20

::Upload InfinniPlatform
InfinniPlatform.Utils.exe upload content\InfinniPlatform withMetadata /y
popd