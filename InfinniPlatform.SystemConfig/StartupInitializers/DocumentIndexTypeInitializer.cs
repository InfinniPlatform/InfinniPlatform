using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Создает типы индексов ElasticSearch для документов прикладных конфигураций.
    /// </summary>
    internal sealed class DocumentIndexTypeInitializer : IStartupInitializer
    {
        public DocumentIndexTypeInitializer(IMetadataApi metadataApi,
                                            ElasticTypesMigrationHelper elasticTypesMigrationHelper)
        {
            _metadataApi = metadataApi;
            _elasticTypesMigrationHelper = elasticTypesMigrationHelper;
        }

        private readonly ElasticTypesMigrationHelper _elasticTypesMigrationHelper;
        private readonly IMetadataApi _metadataApi;

        public int Order => 1;

        public void OnStart()
        {
            var documentNames = _metadataApi.GetDocumentNames();

            _elasticTypesMigrationHelper.CreateOrUpdateStorage(documentNames);
        }
    }
}