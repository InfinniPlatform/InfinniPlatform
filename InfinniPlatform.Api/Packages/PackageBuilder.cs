using System;
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
            dynamic package = new
            {
                ModuleId = Path.GetFileName(fileLocation),
                ConfigurationName = configurationName,
                Version = versionIdentifier,
                Assembly =
                    string.IsNullOrEmpty(fileLocation) ? null : Convert.ToBase64String(File.ReadAllBytes(fileLocation))
            }.ToDynamic();

            if (!string.IsNullOrEmpty(fileLocation))
            {
                var pdbFileName = Path.GetFileNameWithoutExtension(fileLocation) + ".pdb";
                var pathToPdb = Path.Combine(Path.GetDirectoryName(fileLocation), pdbFileName);
                if (File.Exists(pathToPdb))
                {
                    package.PdbFile = Convert.ToBase64String(File.ReadAllBytes(pathToPdb));
                }
            }
            return package;
        }
    }
}