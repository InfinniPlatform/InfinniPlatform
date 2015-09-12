namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    ///     Контракт менеджера метаданных различных типов
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        ///     Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Предзаполненный объект метаданных</returns>
        dynamic CreateItem(string name);

        /// <summary>
        ///     Добавить метаданные объекта конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта</param>
        void InsertItem(dynamic objectToCreate);

        /// <summary>
        ///     Удалить метаданные указанного объекта в указанной конфигурации
        /// </summary>
        /// <param name="metadataObject"></param>
        void DeleteItem(dynamic metadataObject);

        /// <summary>
        ///     Обновить метаданные указанного объекта  в указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта метаданных</param>
        void MergeItem(dynamic objectToCreate);
    }
}