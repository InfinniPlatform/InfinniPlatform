echo off

SET "ASSEMBLIES=%~dp0Assemblies\"
SET "ASSEMBLIES_RU=%~dp0Assemblies\ru-RU\"
SET "OUTPUT=%~dp0%1\bin\Debug\%2\"
SET "OUTPUT_RU=%~dp0%1\bin\Debug\%2\ru-RU\"

pushd %OUTPUT%
for %%i in (*) do xcopy /Y /F %%i %ASSEMBLIES%
popd

pushd %OUTPUT_RU%
for %%i in (*) do xcopy /Y /F %%i %ASSEMBLIES_RU%
popd