using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    ///     Контракт менеджера фабрик объектов конфигурации.
    /// </summary>
    public interface IManagerFactoryConfiguration
    {
        IDataReader BuildDocumentMetadataReader();
        IDataReader BuildMenuMetadataReader();
        IDataReader BuildAssemblyMetadataReader();
        MetadataManagerDocument BuildDocumentManager();
        MetadataManagerElement BuildMenuManager();
        MetadataManagerElement BuildAssemblyManager();
        MetadataManagerElement BuildReportManager();
        IDataReader BuildReportMetadataReader();
    }
}