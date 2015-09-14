xcopy TestSolution_2.0.0.0\* ..\Assemblies\TestSolution_2.0.0.0 /s /y
pushd ..\Assemblies
InfinniPlatform.Utils.exe upload TestSolution_2.0.0.0 withMetadata
popd