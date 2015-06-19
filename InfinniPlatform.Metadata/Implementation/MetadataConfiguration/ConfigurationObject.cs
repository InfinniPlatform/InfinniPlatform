using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///     Прикладной объект конфигурации предметной области
    /// </summary>
    public sealed class ConfigurationObject : IConfigurationObject
    {
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly IIndexFactory _indexFactory;
        private readonly IIndexStateProvider _indexStateProvider;
        private readonly IMetadataConfiguration _metadataConfiguration;

        public ConfigurationObject(IMetadataConfiguration metadataConfiguration, IIndexFactory indexFactory,
            IBlobStorageFactory blobStorageFactory)
        {
            _metadataConfiguration = metadataConfiguration;
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _indexStateProvider = _indexFactory.BuildIndexStateProvider();
        }

        public IMetadataConfiguration MetadataConfiguration
        {
            get { return _metadataConfiguration; }
        }

        /// <summary>
        ///     Предоставить провайдер версий документа для работы в прикладных скриптах
        ///     Создает провайдер, возвращающий версии документов всех существующих в индексе типов для указанной версии
        ///     конфигурации
        /// </summary>
        /// <param name="metadata">метаданные объекта</param>
        /// <param name="version"></param>
        /// <param name="routing">Роутинг для выполнения запросов</param>
        /// <returns>Провайдер версий документа</returns>
        public IVersionProvider GetDocumentProvider(string metadata, string version, string routing)
        {
            if (
                _indexStateProvider.GetIndexStatus(MetadataConfiguration.ConfigurationId,
                    MetadataConfiguration.GetMetadataIndexType(metadata)) != IndexStatus.NotExists)
            {
                return _indexFactory.BuildVersionProvider(MetadataConfiguration.ConfigurationId,
                    MetadataConfiguration.GetMetadataIndexType(metadata), routing, version);
            }
            return null;
        }

        /// <summary>
        ///     Предоставить провайдер версий документа для работы в прикладных скриптах.
        ///     Создает провайдер, возвращающий всегда все версии всех найденных документов
        /// </summary>
        /// <param name="metadata">метаданные объекта</param>
        /// <param name="routing">Роутинг выполнения запросов</param>
        /// <returns></returns>
        public IVersionProvider GetDocumentProvider(string metadata, string routing)
        {
            if (
                _indexStateProvider.GetIndexStatus(MetadataConfiguration.ConfigurationId,
                    MetadataConfiguration.GetMetadataIndexType(metadata)) != IndexStatus.NotExists)
            {
                return _indexFactory.BuildVersionProvider(MetadataConfiguration.ConfigurationId,
                    MetadataConfiguration.GetMetadataIndexType(metadata), routing, null);
            }
            return null;
        }

        /// <summary>
        ///     Получить конструктор версий индекса
        /// </summary>
        /// <param name="metadata">Метаданные</param>
        /// <returns>Конструктор версий</returns>
        public IVersionBuilder GetVersionBuilder(string metadata)
        {
            return _indexFactory.BuildVersionBuilder(
                MetadataConfiguration.ConfigurationId,
                MetadataConfiguration.GetMetadataIndexType(metadata),
                MetadataConfiguration.GetSearchAbilityType(metadata));
        }

        /// <summary>
        ///     Получить хранилище бинарных данных
        /// </summary>
        /// <returns>Хранилище бинарных данных</returns>
        public IBlobStorage GetBlobStorage()
        {
            return _blobStorageFactory.CreateBlobStorage();
        }

        /// <summary>
        ///     Получить версию метаданных конфигурации
        /// </summary>
        /// <returns>Версия конфигурации</returns>
        public string GetConfigurationVersion()
        {
            return MetadataConfiguration.Version;
        }

        /// <summary>
        ///     Получить идентификатор конфигурации метаданных
        /// </summary>
        /// <returns>Идентификатор конфигурации метаданных</returns>
        public string GetConfigurationIdentifier()
        {
            return MetadataConfiguration.ConfigurationId;
        }
    }
}