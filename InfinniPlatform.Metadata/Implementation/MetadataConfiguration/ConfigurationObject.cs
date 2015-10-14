using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

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
		/// <param name="tenantId">Идентификатор организации-клиента для выполнения запросов</param>
		/// <returns>Провайдер версий документа</returns>
		public IVersionProvider GetDocumentProvider(string metadata, string version, string tenantId)
		{
			var indexName = MetadataConfiguration.ConfigurationId;
			var typeName = MetadataConfiguration.GetMetadataIndexType(metadata);

			if (_indexStateProvider.GetIndexStatus(indexName, typeName) == IndexStatus.NotExists)
			{
				// Для тестов %) т.к. теперь метаданные загружаются только с диска
				_indexStateProvider.CreateIndexType(indexName, typeName);

				Logger.Log.Warn("Create new index. IndexName: '{0}', TypeName: '{1}'.", indexName, typeName);
			}

			return _indexFactory.BuildVersionProvider(indexName, typeName, tenantId, version);
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