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
        public delegate IIndexQueryExecutor IndexQueryExecutorFactory(IndexToTypeAccordanceSettings settings);
        public delegate IVersionBuilder VersionBuilderFactory(string indexName, string typeName);
        public delegate ICrudOperationProvider CrudOperationProviderFactory(string indexName, string typeName);
        public delegate IAggregationProvider AggregationProviderFactory(string indexName, string typeName);


        public ElasticFactory(IndexQueryExecutorFactory indexQueryExecutorFactory,
                              VersionBuilderFactory versionBuilderFactory,
                              CrudOperationProviderFactory crudOperationProviderFactory,
                              AggregationProviderFactory aggregationProviderFactory,
                              IndexToTypeAccordanceProvider accordanceProvider,
                              IAllIndexesOperationProvider allIndexesOperationProvider)
        {
            _indexQueryExecutorFactory = indexQueryExecutorFactory;
            _versionBuilderFactory = versionBuilderFactory;
            _crudOperationProviderFactory = crudOperationProviderFactory;
            _aggregationProviderFactory = aggregationProviderFactory;
            _accordanceProvider = accordanceProvider;
            _allIndexesOperationProvider = allIndexesOperationProvider;

            _providersInfo = new List<ElasticSearchProviderInfo>();
            _settings = new ConcurrentDictionary<string, IndexToTypeAccordanceSettings>();
        }


        private readonly IndexQueryExecutorFactory _indexQueryExecutorFactory;
        private readonly VersionBuilderFactory _versionBuilderFactory;
        private readonly CrudOperationProviderFactory _crudOperationProviderFactory;
        private readonly AggregationProviderFactory _aggregationProviderFactory;
        private readonly IndexToTypeAccordanceProvider _accordanceProvider;
        private readonly IAllIndexesOperationProvider _allIndexesOperationProvider;

        private readonly List<ElasticSearchProviderInfo> _providersInfo;
        private readonly ConcurrentDictionary<string, IndexToTypeAccordanceSettings> _settings;


        /// <summary>
        /// Создать конструктор версий хранилища документов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа</param>
        public IVersionBuilder BuildVersionBuilder(string indexName, string typeName)
        {
            return _versionBuilderFactory(indexName, typeName);
        }

        /// <summary>
        /// Создать провайдер данных
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа</param>
        public IVersionProvider BuildVersionProvider(string indexName, string typeName)
        {
            var indexToTypeAccordanceSettings = GetIndexTypeAccordanceSettings(indexName, typeName);

            var elasticSearchProvider = _crudOperationProviderFactory(indexName, typeName);
            var indexQueryExecutor = _indexQueryExecutorFactory(indexToTypeAccordanceSettings);

            return new VersionProvider(elasticSearchProvider, indexQueryExecutor);
        }

        /// <summary>
        /// Создать провайдер для поиска данных
        /// </summary>
        /// <param name="indexName">Наименование индекса для поиска</param>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
        /// существующих в индексе типов
        /// </param>
        /// <returns>Провайдер для поиска данных</returns>
        public ICrudOperationProvider BuildCrudOperationProvider(string indexName, string typeName)
        {
            var providerInfo = _providersInfo.FindInfo(indexName, typeName);
            if (providerInfo == null)
            {
                var provider = _crudOperationProviderFactory(indexName, typeName);
                _providersInfo.Add(new ElasticSearchProviderInfo(indexName, typeName, provider));
                return provider;
            }
            return providerInfo.Provider;
        }

        /// <summary>
        /// Получить провайдер операций по всем индексам и типам базы
        /// </summary>
        /// <returns>Провайдер операций по всем индексам и типам базы</returns>
        public IAllIndexesOperationProvider BuildAllIndexesOperationProvider()
        {
            return _allIndexesOperationProvider;
        }

        /// <summary>
        /// Создать исполнитель запросов к индексу
        /// </summary>
        /// <param name="indexName">
        /// Наименование индекса, для которого выполняется запрос. Если не указан, осуществляется выборка
        /// из всех существующих индексов
        /// </param>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка из
        /// всех существующих в индексе типов
        /// </param>
        /// <returns></returns>
        public IIndexQueryExecutor BuildIndexQueryExecutor(string indexName, string typeName)
        {
            var indexToTypeAccordanceSettings = GetIndexTypeAccordanceSettings(indexName, typeName);

            return _indexQueryExecutorFactory(indexToTypeAccordanceSettings);
        }

        /// <summary>
        /// Создать исполнитель агрегаций
        /// </summary>
        /// <param name="indexName">Наименование индекса, для которого выполняется запрос</param>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
        /// существующих в индексе типов
        /// </param>
        public IAggregationProvider BuildAggregationProvider(string indexName, string typeName)
        {
            return _aggregationProviderFactory(indexName, typeName);
        }

        private static string CreateKey(string indexName, string typeName)
        {
            return $"Index:{indexName};TypeName:{typeName}";
        }

        private IndexToTypeAccordanceSettings GetIndexTypeAccordanceSettings(string indexName, string typeName)
        {
            var key = CreateKey(indexName, typeName);

            IndexToTypeAccordanceSettings indexSettings;

            if (!_settings.TryGetValue(key, out indexSettings))
            {
                indexSettings = _accordanceProvider.GetIndexTypeAccordances(indexName, typeName);
                _settings[key] = indexSettings;
            }

            return indexSettings;
        }
    }
}