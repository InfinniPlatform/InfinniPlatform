using System;
using System.Diagnostics;
using System.IO;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Packages
{
    /// <summary>
    ///     Конструктор пакетов обновления
    /// </summary>
    public sealed class PackageBuilder
    {
        public dynamic BuildSystemPackage(string configurationName,
            string fileName)
        {
            dynamic package = new
            {
                ModuleId = fileName,
                ConfigurationName = configurationName
            }.ToDynamic();

            return package;
        }

        public dynamic BuildPackage(string configurationName, string versionIdentifier,
            string fileLocation)
        {

            var tempFolder = Path.GetDirectoryName(fileLocation) + "\\" + Guid.NewGuid();
            Directory.CreateDirectory(tempFolder);

            var pathTempLocationDll = Path.Combine(tempFolder, Path.GetFileName(fileLocation));
            var pathTempLocationPdb = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(fileLocation) + ".pdb");
            
            var pdbPath = Path.Combine(Path.GetDirectoryName(fileLocation), Path.GetFileNameWithoutExtension(fileLocation) + ".pdb");

            if (File.Exists(pdbPath))
            {
                File.Copy(pdbPath, pathTempLocationPdb);
            }
            File.Copy(fileLocation, pathTempLocationDll);

            dynamic package = new
            {
                ModuleId = Path.GetFileName(fileLocation),
                ConfigurationName = configurationName,
                Version = versionIdentifier,
                Assembly =
                    string.IsNullOrEmpty(pathTempLocationDll) ? null : Convert.ToBase64String(File.ReadAllBytes(pathTempLocationDll))
            }.ToDynamic();

            if (!string.IsNullOrEmpty(fileLocation))
            {
                if (File.Exists(pathTempLocationPdb))
                {
                    package.PdbFile = Convert.ToBase64String(File.ReadAllBytes(pathTempLocationPdb));
                }
            }
            return package;
        }
    }
}