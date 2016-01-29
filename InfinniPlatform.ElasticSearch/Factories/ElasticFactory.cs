using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Versioning;

namespace InfinniPlatform.ElasticSearch.Factories
{
    /// <summary>
    /// Фабрика для получения инициализированных, готовых к работе реализаций контрактов работы с данными ElasticSearch.
    /// </summary>
    public sealed class ElasticFactory : IIndexFactory
    {
        public delegate IIndexQueryExecutor IndexQueryExecutorFactory(string indexName, string typeName);
        public delegate IVersionBuilder VersionBuilderFactory(string indexName, string typeName);
        public delegate IAggregationProvider AggregationProviderFactory(string indexName, string typeName);


        public ElasticFactory(IndexQueryExecutorFactory indexQueryExecutorFactory,
                              VersionBuilderFactory versionBuilderFactory,
                              AggregationProviderFactory aggregationProviderFactory,
                              IAllIndexesOperationProvider allIndexesOperationProvider)
        {
            _indexQueryExecutorFactory = indexQueryExecutorFactory;
            _versionBuilderFactory = versionBuilderFactory;
            _aggregationProviderFactory = aggregationProviderFactory;
            _allIndexesOperationProvider = allIndexesOperationProvider;
        }


        private readonly IndexQueryExecutorFactory _indexQueryExecutorFactory;
        private readonly VersionBuilderFactory _versionBuilderFactory;
        private readonly AggregationProviderFactory _aggregationProviderFactory;
        private readonly IAllIndexesOperationProvider _allIndexesOperationProvider;

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