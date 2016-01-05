using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataReaders;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories
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