@echo off
::Check version
IF [%1] == [] (
	echo "Set InfinniPlatform version as parameter!"
	EXIT /B
)

::Set package version in files
powershell -NoProfile -ExecutionPolicy Bypass -Command ".\SetVersion.ps1 %1"

::Clear build artifacts
pushd ..
rd /S /Q Assemblies
popd

::Build project
msbuild ..\InfinniPlatform.sln /p:Configuration=Debug /m /v:q

::Copy modified config to Assemblies
xcopy PackageContent\InfinniPlatform.config ..\Assemblies\ /y
xcopy PackageContent\InfinniPlatform.Utils.exe.config ..\Assemblies\ /y

::Create package
nuget Pack InfinniPlatform.nuspec -NoDefaultExcludes