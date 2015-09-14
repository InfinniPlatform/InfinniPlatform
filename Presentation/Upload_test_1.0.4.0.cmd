xcopy TestSolution_1.0.4.0\* ..\Assemblies\TestSolution_1.0.4.0 /s /y
pushd ..\Assemblies
InfinniPlatform.Utils.exe upload TestSolution_1.0.4.0 withMetadata
popd