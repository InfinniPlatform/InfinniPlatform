namespace InfinniPlatform.Factories
{
    /// <summary>
    ///     Фабрика роутинга запросов к индексам
    /// </summary>
    public interface IIndexRoutingFactory
    {
        /// <summary>
        ///     Получить роутинг запросов к индексу для указанных типов
        /// </summary>
        /// <param name="userRouting">Роутинг, предоставленный пользователем</param>
        /// <param name="indexName">Индекс для формирования роутинга</param>
        /// <param name="indexType">Тип в индексе для формирования роутинга</param>
        /// <returns>Строка роутинга для запросов к индексу</returns>
        string GetRouting(string userRouting, string indexName, string indexType);

        /// <summary>
        ///     Получить роутинг для формирования запроса к данным без указания конкретного индекса и типа (запроса по всем
        ///     индексам и типам)
        /// </summary>
        /// <param name="userRouting">Роутинг, сопоставленный пользователю</param>
        /// <returns>Результирующий роутинг пользователя</returns>
        string GetRoutingUnspecifiedType(string userRouting);
    }
}