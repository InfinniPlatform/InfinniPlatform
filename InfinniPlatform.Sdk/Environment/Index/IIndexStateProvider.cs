namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Провайдер административных операций с индексом
    /// </summary>
    public interface IIndexStateProvider
    {
        /// <summary>
        ///     Получить состояние индекса
        /// </summary>
        /// <param name="indexName">Наименование  индекса</param>
        /// <param name="typeName"></param>
        /// <returns>Состояние индекса</returns>
        IndexStatus GetIndexStatus(string indexName, string typeName);

        /// <summary>
        ///     В ходе выполнения операции удалятся все данные и маппинга всех типов из индекса.
        /// </summary>
        /// <param name="indexName">
        ///     Наименование логической совокупности индексов, содержащих данные, относящиеся к версиям
        ///     указанного типа
        /// </param>
        /// <param name="typeName">
        ///     Наименование удаляемого типа из индекса (Удаляются все версии типа из всех индексов, относящихся
        ///     к совокупности (алиасу))
        /// </param>
        void RecreateIndex(string indexName, string typeName);

        /// <summary>
        ///     Удалить индекс с указанным наименованием
        /// </summary>
        /// <param name="indexName">Наименование логической совокупности индексов</param>
        /// <param name="typeName">Наименование удаляемого типа</param>
        void DeleteIndexType(string indexName, string typeName);

        /// <summary>
        ///     Обновление позволяет делать запросы к только что добавленным данным
        /// </summary>
        void Refresh();

        /// <summary>
        ///     Изменяет настройки маппирования для типа, хранящегося в индексе
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Тип документа в индексе</param>
        /// <param name="deleteExistingVersion">Удалить существующую версию индекса</param>
        /// <param name="mappingUpdates">Список изменений в маппинге</param>
        void CreateIndexType(
            string indexName,
            string typeName,
            bool deleteExistingVersion = false,
            IIndexTypeMapping mappingUpdates = null
            );

        /// <summary>
        ///     Удалить весь индекс для всех типов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        void DeleteIndex(string indexName);
    }
}