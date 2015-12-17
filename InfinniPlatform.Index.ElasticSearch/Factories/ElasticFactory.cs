using System.Collections.Concurrent;
using System.Collections.Generic;

using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Factories
{
	/// <summary>
	/// Фабрика для получения инициализированных, готовых к работе реализаций контрактов работы с данными ElasticSearch.
	/// </summary>
	public sealed class ElasticFactory : IIndexFactory
	{
		private readonly IndexToTypeAccordanceProvider _accordanceProvider;

		private readonly ConcurrentDictionary<string, IndexToTypeAccordanceSettings> _settings = new ConcurrentDictionary<string, IndexToTypeAccordanceSettings>();

		public ElasticFactory()
		{
			_accordanceProvider = new IndexToTypeAccordanceProvider();
		}

		private static string CreateKey(string indexName, IEnumerable<string> typeNames)
		{
			return $"Indexes:{indexName};TypeNames:{string.Join("_", typeNames)}";
		}

		private IndexToTypeAccordanceSettings GetIndexTypeAccordanceSettings(string indexName, IEnumerable<string> typeNames)
		{
			var key = CreateKey(indexName, typeNames);

			IndexToTypeAccordanceSettings indexSettings;

			if (!_settings.TryGetValue(key, out indexSettings))
			{
				indexSettings = _accordanceProvider.GetIndexTypeAccordances(indexName, typeNames);
				_settings[key] = indexSettings;
			}

			return indexSettings;
		}

		/// <summary>
		///     Создать конструктор версий хранилища документов
		/// </summary>
		/// <param name="indexName">Наименование индекса</param>
		/// <param name="typeName">Наименование типа</param>
		public IVersionBuilder BuildVersionBuilder(string indexName, string typeName)
		{
			return new VersionBuilder(new ElasticConnection(), indexName, typeName);
		}

	    /// <summary>
	    ///     Создать провайдер данных
	    /// </summary>
	    /// <param name="indexName">Наименование индекса</param>
	    /// <param name="typeName">Наименование типа</param>
	    public IVersionProvider BuildVersionProvider(string indexName, string typeName)
		{
			var elasticSearchProvider = new ElasticSearchProvider(indexName, typeName);
			var indexSettings = GetIndexTypeAccordanceSettings(indexName, new[] { typeName });
			var indexQueryExecutor = new IndexQueryExecutor(indexSettings);
			var documentProvider = new DocumentProvider(indexQueryExecutor);

			return new VersionProvider(elasticSearchProvider, documentProvider);
		}

		/// <summary>
		///     Создать провайдер данных для доступа к нескольким индексам
		/// </summary>
		/// <param name="indexName">
		/// Наименование индексов. Если имена не указаны,
		/// для поиска будут использованы все имеющиеся индексы
		/// </param>
		/// <param name="typeNames">Наименования типов</param>
		public IDocumentProvider BuildMultiIndexDocumentProvider(string indexName = null, IEnumerable<string> typeNames = null)
		{
			// Создаём универсальный провайдер для выполнения поисковых запросов ко всем документам конфигурации
			return new DocumentProvider(new IndexQueryExecutor(GetIndexTypeAccordanceSettings(indexName, typeNames)));
		}

		private readonly List<ElasticSearchProviderInfo> _providersInfo = new List<ElasticSearchProviderInfo>();

		/// <summary>
		///     Создать провайдер для поиска данных
		/// </summary>
		/// <param name="indexName">Наименование индекса для поиска</param>
		/// <param name="typeName">
		/// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
		/// существующих в индексе типов
		/// </param>
		/// <param name="version">Версия конфигурации, для которой создается провайдер данных</param>
		/// <returns>Провайдер для поиска данных</returns>
		public ICrudOperationProvider BuildCrudOperationProvider(string indexName, string typeName, string version = null)
		{
			var providerInfo = _providersInfo.FindInfo(indexName, typeName);
			if (providerInfo == null)
			{
				var provider = new ElasticSearchProvider(indexName, typeName);
				_providersInfo.Add(new ElasticSearchProviderInfo(indexName, typeName, provider));
				return provider;
			}
			return providerInfo.Provider;
		}

		/// <summary>
		///     Получить провайдер операций по всем индексам и типам базы
		/// </summary>
		/// <returns>Провайдер операций по всем индексам и типам базы</returns>
		public IAllIndexesOperationProvider BuildAllIndexesOperationProvider()
		{
			return new ElasticSearchProviderAllIndexes();
		}

		/// <summary>
		///     Создать исполнитель запросов к индексу
		/// </summary>
		/// <param name="indexName">
		/// Наимемнование индекса, для которого выполняется запрос. Если не указан, осуществляется выборка
		/// из всех существующих индексов
		/// </param>
		/// <param name="typeName">
		/// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка из
		/// всех существующих в индексе типов
		/// </param>
		/// <returns></returns>
		public IIndexQueryExecutor BuildIndexQueryExecutor(string indexName, string typeName)
		{
			return new IndexQueryExecutor(GetIndexTypeAccordanceSettings(indexName, new[] { typeName }));
		}

		/// <summary>
		///     Создать исполнитель агрегационных запросов к индексу
		/// </summary>
		/// <param name="indexName">Наименование индекса, для которого выполняется запрос</param>
		/// <param name="typeName">
		/// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
		/// существующих в индексе типов
		/// </param>
		public IAggregationProvider BuildAggregationProvider(string indexName, string typeName)
		{
			return new ElasticSearchAggregationProvider(indexName, typeName);
		}
	}
}