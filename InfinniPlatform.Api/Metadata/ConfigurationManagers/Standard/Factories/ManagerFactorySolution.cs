using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories
{
    /// <summary>
    /// Фабрика менеджеров управления метаданными решений
    /// </summary>
    public sealed class ManagerFactorySolution 
    {

        public static IDataReader BuildSolutionReader(string version)
        {
            return new MetadataReaderSolution(version);
        }

        public static MetadataManagerSolution BuildSolutionManager(string version)
        {
            return new MetadataManagerSolution(new MetadataReaderSolution(version),version);
        }

    }
}
