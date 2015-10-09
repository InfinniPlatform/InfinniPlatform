﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.MetadataDesigner.Views.Update;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
    public sealed class ExchangeDirectorSolution
    {
        private readonly IUpdatePrepareConfig _updatePrepareConfig;
		private readonly string _solutionId;

        private string _solutionArchiveName;

        public ExchangeDirectorSolution(IUpdatePrepareConfig updatePrepareConfig, string solutionId)
		{
			_updatePrepareConfig = updatePrepareConfig;
            _solutionId = solutionId;

            _solutionArchiveName = string.Format("exportsolution_{0}.zip", _solutionId);
		}

        public void ExportJsonSolutionToDirectory(string exportDir, string version, string newVersion)
        {
            if (_updatePrepareConfig.PrepareRoutingOperation())
            {
                new SolutionExporter(new DirectoryStructure(exportDir), 
                    config => new DirectoryStructure(exportDir + string.Format(@"\{0}_{1}", config.Name, newVersion))).ExportSolutionToStructure(_solutionId, version, newVersion);
            }
        }

        public dynamic UpdateSolutionMetadataFromDirectory(string importDir)
        {
            if (_updatePrepareConfig.PrepareRoutingOperation())
            {
                var configUpdater = new ConfigUpdater(GetVersionFromPath(importDir));
                ZipLoader.CreateZipArchiveFromDirectory(importDir, _solutionArchiveName);
                return configUpdater.UpdateSolutionMetadataFromZip(_solutionArchiveName);
            }
            return null;
        }

        public void UpdateConfigurationAppliedAssemblies(dynamic solution)
        {
            foreach (var configuration in solution.ReferencedConfigurations)
            {
                if (_updatePrepareConfig.PrepareRoutingOperation())
                {
                    var configUpdater = new ConfigUpdater(configuration.Version.ToString());
                    configUpdater.UpdateConfigurationAppliedAssemblies(configuration.Name.ToString());
                }                
            }
        }

        private string GetVersionFromPath(string path)
        {
            var dir = Path.GetFileName(path);
            var pathConfig = dir.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (pathConfig.Count() == 2)
            {
                return pathConfig[1];
            }
            return null;
        }
    }
}