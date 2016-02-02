﻿using InfinniPlatform.Core.Index;

namespace InfinniPlatform.ElasticSearch.Factories
{
    /// <summary>
    /// Фабрика провайдеров для операций с данными индексов
    /// </summary>
    public interface IIndexFactory
    {
        /// <summary>
        /// Создать исполнитель запросов к индексу
        /// </summary>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка из
        /// всех существующих в индексе типов
        /// </param>
        /// <returns></returns>
        IIndexQueryExecutor BuildIndexQueryExecutor(string typeName);

        /// <summary>
        /// Создать исполнитель агрегационных запросов к индексу
        /// </summary>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
        /// существующих в индексе типов
        /// </param>
        IAggregationProvider BuildAggregationProvider(string typeName);

        /// <summary>
        /// Получить провайдер операций по всем индексам и типам базы
        /// </summary>
        /// <returns>Провайдер операций по всем индексам и типам базы</returns>
        IAllIndexesOperationProvider BuildAllIndexesOperationProvider();
    }
}