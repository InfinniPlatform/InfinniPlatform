::Check version
IF [%1] == [] (
	echo "Set InfinniPlatform version as parameter!"
	EXIT /B
)

::Set package version in files
powershell -NoProfile -ExecutionPolicy Bypass -Command ".\SetTemplateStubs.ps1 %1"

::Clear build artifacts
pushd ..\..
rd /S /Q Assemblies
popd

::Build project
nuget restore ..\..\InfinniPlatform.sln
msbuild ..\..\InfinniPlatform.sln /p:Configuration=Debug /m /v:q

::Create package
nuget Pack InfinniPlatform.nuspec -NoDefaultExcludes

::Clean up
del InfinniPlatform.nuspec /Q /F