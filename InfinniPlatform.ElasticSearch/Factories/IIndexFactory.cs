using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Versioning;

namespace InfinniPlatform.ElasticSearch.Factories
{
    /// <summary>
    /// Фабрика провайдеров для операций с данными индексов
    /// </summary>
    public interface IIndexFactory
    {
        /// <summary>
        /// Создать конструктор версий хранилища документов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа</param>
        IVersionBuilder BuildVersionBuilder(string indexName, string typeName);
        
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
        IIndexQueryExecutor BuildIndexQueryExecutor(string indexName, string typeName);

        /// <summary>
        /// Создать исполнитель агрегационных запросов к индексу
        /// </summary>
        /// <param name="indexName">Наименование индекса, для которого выполняется запрос</param>
        /// <param name="typeName">
        /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
        /// существующих в индексе типов
        /// </param>
        IAggregationProvider BuildAggregationProvider(string indexName, string typeName);

        /// <summary>
        /// Получить провайдер операций по всем индексам и типам базы
        /// </summary>
        /// <returns>Провайдер операций по всем индексам и типам базы</returns>
        IAllIndexesOperationProvider BuildAllIndexesOperationProvider();
    }
}