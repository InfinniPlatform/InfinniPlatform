namespace InfinniPlatform.Metadata
{
    /// <summary>
    ///     Контракт для конструирования событий изменения объектов
    /// </summary>
    public interface IObjectMetadataHandler
    {
        /// <summary>
        ///     Сформировать событие изменения свойства объекта
        /// </summary>
        /// <param name="property">Путь к свойству</param>
        /// <param name="value">Значение свойства</param>
        /// <returns>Сериализованное событие</returns>
        string CreateProperty(string property, object value);

        /// <summary>
        ///     Сформировать событие создания контейнера
        /// </summary>
        /// <param name="property">Путь к контейнеру</param>
        /// <returns>Сериализованное событие</returns>
        string CreateContainer(string property);

        /// <summary>
        ///     Сформировать событие создания коллекции объектов
        /// </summary>
        /// <param name="collectionProperty">Путь к коллекции объектов</param>
        /// <returns>Сериализованное событие</returns>
        string CreateContainerCollection(string collectionProperty);

        /// <summary>
        ///     Сформировать событие добавления объекта в коллекцию
        /// </summary>
        /// <param name="collectionProperty">Путь к коллекции объектов</param>
        /// <param name="value">Добавляемый элемент коллекции</param>
        /// <param name="index">Индекс в коллекции для вставки элемента (-1 для добавления в конец)</param>
        /// <returns>Сериализованное событие добавления объекта в коллекцию</returns>
        string AddItemToCollection(string collectionProperty, object value = null, int index = -1);
    }
}