using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Versioning;

using Nest;

namespace InfinniPlatform.ElasticSearch.Factories
{
    /// <summary>
    /// Фабрика для получения инициализированных, готовых к работе реализаций контрактов работы с данными ElasticSearch.
    /// </summary>
    public sealed class ElasticFactory : IIndexFactory
    {
        public delegate IIndexQueryExecutor IndexQueryExecutorFactory(string indexName, string typeName);
        public delegate IVersionBuilder VersionBuilderFactory(string indexName, string typeName);
        public delegate ICrudOperationProvider CrudOperationProviderFactory(string indexName, string typeName);
        public delegate IAggregationProvider AggregationProviderFactory(string indexName, string typeName);


        public ElasticFactory(IndexQueryExecutorFactory indexQueryExecutorFactory,
                              VersionBuilderFactory versionBuilderFactory,
                              CrudOperationProviderFactory crudOperationProviderFactory,
                              AggregationProviderFactory aggregationProviderFactory,
                              IAllIndexesOperationProvider allIndexesOperationProvider)
        {
            _indexQueryExecutorFactory = indexQueryExecutorFactory;
            _versionBuilderFactory = versionBuilderFactory;
            _crudOperationProviderFactory = crudOperationProviderFactory;
            _aggregationProviderFactory = aggregationProviderFactory;
            _allIndexesOperationProvider = allIndexesOperationProvider;

            _providersInfo = new List<ElasticSearchProviderInfo>();
        }


        private readonly IndexQueryExecutorFactory _indexQueryExecutorFactory;
        private readonly VersionBuilderFactory _versionBuilderFactory;
        private readonly CrudOperationProviderFactory _crudOperationProviderFactory;
        private readonly AggregationProviderFactory _aggregationProviderFactory;
        private readonly IAllIndexesOperationProvider _allIndexesOperationProvider;

        private readonly List<ElasticSearchProviderInfo> _providersInfo;
        
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
            var elasticSearchProvider = _crudOperationProviderFactory(indexName, typeName);
            var indexQueryExecutor = _indexQueryExecutorFactory(indexName, typeName);

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
            return _indexQueryExecutorFactory(indexName, typeName);
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
    }
}