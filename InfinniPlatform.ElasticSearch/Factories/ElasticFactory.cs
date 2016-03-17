﻿using InfinniPlatform.Core.Index;

namespace InfinniPlatform.ElasticSearch.Factories
{
    /// <summary>
    /// Фабрика для получения инициализированных, готовых к работе реализаций контрактов работы с данными ElasticSearch.
    /// </summary>
    public sealed class ElasticFactory : IIndexFactory
    {
        public delegate IIndexQueryExecutor IndexQueryExecutorFactory(string typeName);
        public delegate IAggregationProvider AggregationProviderFactory(string typeName);


        public ElasticFactory(IndexQueryExecutorFactory indexQueryExecutorFactory,
                              AggregationProviderFactory aggregationProviderFactory,
                              IAllIndexesOperationProvider allIndexesOperationProvider)
        {
            _indexQueryExecutorFactory = indexQueryExecutorFactory;
            _aggregationProviderFactory = aggregationProviderFactory;
            _allIndexesOperationProvider = allIndexesOperationProvider;
        }


        private readonly IndexQueryExecutorFactory _indexQueryExecutorFactory;
        private readonly AggregationProviderFactory _aggregationProviderFactory;
        private readonly IAllIndexesOperationProvider _allIndexesOperationProvider;

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
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка из
        /// всех существующих в индексе типов
        /// </param>
        /// <returns></returns>
        public IIndexQueryExecutor BuildIndexQueryExecutor(string typeName)
        {
            return _indexQueryExecutorFactory(typeName);
        }

        /// <summary>
        /// Создать исполнитель агрегаций
        /// </summary>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
        /// существующих в индексе типов
        /// </param>
        public IAggregationProvider BuildAggregationProvider(string typeName)
        {
            return _aggregationProviderFactory(typeName);
        }
    }
}