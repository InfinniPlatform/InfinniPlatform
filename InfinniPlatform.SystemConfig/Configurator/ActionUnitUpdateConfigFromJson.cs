using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Обновление конфигурации
    /// </summary>
    public sealed class ActionUnitUpdateConfigFromJson
    {
        public void Action(IUploadContext target)
        {
            var zipArchive = target.FileContent.ReadArchive(Encoding.UTF8);

            target.Result = new DynamicWrapper();
            target.Result.InstallLog = new List<string>();
            string folderName = (string) target.LinkedData.Version + "_" + Guid.NewGuid().ToString();
            var configExporter = new ConfigExporter(new ZipStructure(zipArchive, folderName));
            dynamic config = configExporter.ImportHeaderFromStructure((string) target.LinkedData.Version);
            target.Result.InstallLog.Add(string.Format("Configuration \"{0}\"(version: \"{1}\") sucessfully installed",
                                                       config.Name,
                                                       string.IsNullOrEmpty(target.LinkedData.Version) ? "0" : target.LinkedData.Version));
            target.Result.ConfigurationId = config.Name;
        }
    }
}