using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories
{
    /// <summary>
    /// Фабрика менеджеров управления метаданными решений
    /// </summary>
    public sealed class ManagerFactorySolution
    {
        public static IDataReader BuildSolutionReader()
        {
            return new MetadataReaderSolution();
        }

        public static MetadataManagerSolution BuildSolutionManager()
        {
            return new MetadataManagerSolution(new MetadataReaderSolution());
        }
    }
}