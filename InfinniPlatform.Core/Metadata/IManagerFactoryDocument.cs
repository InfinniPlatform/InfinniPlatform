using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    ///     Контракт менеджера фабрик элементов метаданных документа
    /// </summary>
    public interface IManagerFactoryDocument
    {
        IDataReader BuildViewMetadataReader();
        IDataReader BuildGeneratorMetadataReader();
        IDataReader BuildScenarioMetadataReader();
        IDataReader BuildProcessMetadataReader();
        IDataReader BuildServiceMetadataReader();
        IDataReader BuildValidationWarningsMetadataReader();
        IDataReader BuildValidationErrorsMetadataReader();
        MetadataManagerElement BuildViewManager();
        MetadataManagerElement BuildGeneratorManager();
        MetadataManagerElement BuildScenarioManager();
        MetadataManagerElement BuildProcessManager();
        MetadataManagerElement BuildServiceManager();
        MetadataManagerElement BuildValidationErrorsManager();
        MetadataManagerElement BuildValidationWarningsManager();
        MetadataManagerElement BuildManagerByType(string metadataType);
    }
}