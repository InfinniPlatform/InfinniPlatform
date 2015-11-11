using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Фабрика провайдеров для операций с данными индексов
    /// </summary>
    public interface IIndexFactory
    {
        /// <summary>
        ///     Создать конструктор версий хранилища документов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа</param>
        IVersionBuilder BuildVersionBuilder(string indexName, string typeName);

	    /// <summary>
	    ///     Создать провайдер данных
	    /// </summary>
	    /// <param name="indexName">Наименование индекса</param>
	    /// <param name="typeName">Наименование типа</param>
	    /// <param name="version"></param>
	    IVersionProvider BuildVersionProvider(string indexName, string typeName, string version);

	    /// <summary>
	    ///     Создать провайдер данных для доступа к нескольким индексам
	    /// </summary>
	    /// <param name="indexNames">
	    /// Наименование индексов. Если имена не указаны,
	    /// для поиска будут использованы все имеющиеся индексы
	    /// </param>
	    /// <param name="typeNames">Имена типов, по которым будет производиться поиск</param>
	    IDocumentProvider BuildMultiIndexDocumentProvider(IEnumerable<string> indexNames = null, IEnumerable<string> typeNames = null);

	    /// <summary>
	    ///     Создать провайдер для поиска данных
	    /// </summary>
	    /// <param name="indexName">Наименование индекса для поиска</param>
	    /// <param name="typeName">
	    /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
	    /// существующих в индексе типов
	    /// </param>
	    /// <param name="version"></param>
	    /// <returns>Провайдер для поиска данных</returns>
	    ICrudOperationProvider BuildCrudOperationProvider(string indexName, string typeName, string version);

        /// <summary>
        ///     Создать провайдер операций для работы с индексами
        /// </summary>
        /// <returns>Провайдер операций для работы с индексом</returns>
        IIndexStateProvider BuildIndexStateProvider();

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
	    IIndexQueryExecutor BuildIndexQueryExecutor(string indexName, string typeName);

	    /// <summary>
	    ///     Создать исполнитель агрегационных запросов к индексу
	    /// </summary>
	    /// <param name="indexName">Наимемнование индекса, для которого выполняется запрос</param>
	    /// <param name="typeName">
	    /// Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех
	    /// существующих в индексе типов
	    /// </param>
	    IAggregationProvider BuildAggregationProvider(string indexName, string typeName);

	    /// <summary>
	    ///     Получить провайдер операций по всем индексам и типам базы
	    /// </summary>
	    /// <returns>Провайдер операций по всем индексам и типам базы</returns>
	    IAllIndexesOperationProvider BuildAllIndexesOperationProvider();
    }
}