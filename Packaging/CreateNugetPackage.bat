::Clear build artifacts
pushd ..
rd /S /Q Assemblies
popd

::Build project
msbuild ..\InfinniPlatform.sln /p:Configuration=Debug /m

::Copy modified config to Assemblies
xcopy PackageContent\InfinniPlatform.config ..\Assemblies\ /y
xcopy PackageContent\InfinniPlatform.Utils.exe.config ..\Assemblies\ /y

::Create package
nuget Pack InfinniPlatform.nuspec