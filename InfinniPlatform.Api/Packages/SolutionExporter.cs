using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.Packages
{
    public sealed class SolutionExporter
    {
        private readonly IExportStructure _exportStructureSolution;
        private readonly Func<dynamic, IExportStructure> _exportStructureConfig;

        public SolutionExporter(IExportStructure exportStructureSolution, Func<dynamic,IExportStructure> exportStructureConfig)
        {
            _exportStructureSolution = exportStructureSolution;
            _exportStructureConfig = exportStructureConfig;
        }

        public void ExportSolutionToStructure(string solutionId, string version)
        {
            var manager = ManagerFactorySolution.BuildSolutionReader(version);

            dynamic solution = manager.GetItem(solutionId);

            _exportStructureSolution.Start();

            try
            {
                var result = ((string)solution.ToString()).Split("\n\r".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                _exportStructureSolution.AddSolution(result);

                foreach (var referencedConfig in solution.ReferencedConfigurations)
                {
                    var configExporter = new ConfigExporter(_exportStructureConfig(referencedConfig));

                    configExporter.ExportHeaderToStructure(referencedConfig.Version, referencedConfig.Name);
                }

            }
            finally
            {
                _exportStructureSolution.End();
            }
        }

        public dynamic ImportHeaderFromStructure(string version)
        {
            _exportStructureSolution.Start();

            dynamic solution = _exportStructureSolution.GetSolution();

            new UpdateApi(version).UpdateMetadataObject(solution.Name, null, solution, MetadataType.Solution);

            foreach (var referencedConfig in solution.ReferencedConfigurations)
            {
                var configExporter = new ConfigExporter(_exportStructureConfig(referencedConfig));

                configExporter.ImportHeaderFromStructure( referencedConfig.Version);
            }

            _exportStructureSolution.End();

            return solution;
        }
    }
}
