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
    public sealed class ActionUnitUpdateSolutionFromJson
    {
        public void Action(IUploadContext target)
        {
            var zipArchive = target.FileContent.ReadArchive(Encoding.UTF8);

            target.Result = new DynamicWrapper();
            target.Result.InstallLog = new List<string>();
            string folderName = (string) target.LinkedData.Version + "_" + Guid.NewGuid().ToString();
            var solutionExporter = new SolutionExporter(new ZipStructure(zipArchive, folderName, null),
                config => new ZipStructure(zipArchive, folderName, string.Format(@"{0}_{1}", config.Name, config.Version)));
            dynamic solution = solutionExporter.ImportHeaderFromStructure((string) target.LinkedData.Version);
            target.Result.Solution = solution;
            target.Result.InstallLog.Add(string.Format("Solution \"{0}\"(version: \"{1}\") sucessfully installed",
                                                       solution.Name,
                                                       string.IsNullOrEmpty(target.LinkedData.Version) ? "0" : target.LinkedData.Version));
            target.Result.SolutionId = solution.Name;
        }
    }
}