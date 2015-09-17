::Copy modified config to Assemblies
xcopy PackageContent\InfinniPlatform.1.0.0.config ..\Assemblies\ /y
xcopy PackageContent\InfinniPlatform.Utils.exe.config ..\Assemblies\ /y

::Create package
nuget Pack InfinniPlatform.nuspec

::Copy package to packages directory
xcopy InfinniPlatform.1.0.0.nupkg C:\Projects\InfinniPlatform.Packages\ /y