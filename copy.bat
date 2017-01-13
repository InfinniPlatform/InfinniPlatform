echo off

SET ASSEMBLIES="C:\Projects\InfinniPlatform\Assemblies\"
SET ASSEMBLIES_RU="C:\Projects\InfinniPlatform\Assemblies\ru-RU\"
SET "OUTPUT=%1\bin\Debug\netcoreapp1.0\"
SET "OUTPUT_RU=%1\bin\Debug\netcoreapp1.0\ru-RU\"

pushd %OUTPUT%
for %%i in (*) do xcopy /Y /F %%i %ASSEMBLIES%
popd

pushd %OUTPUT_RU%
for %%i in (*) do xcopy /Y /F %%i %ASSEMBLIES_RU%
popd