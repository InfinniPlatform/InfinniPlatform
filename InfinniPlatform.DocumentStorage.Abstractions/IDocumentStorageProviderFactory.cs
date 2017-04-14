namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Фабрика для получения экземпляров <see cref="IDocumentStorageProvider" /> и
    /// <see cref="IDocumentStorageProvider{TDocument}" />.
    /// </summary>
    public interface IDocumentStorageProviderFactory
    {
        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorageProvider" />.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        IDocumentStorageProvider GetStorageProvider(string documentType);

        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorageProvider{TDocument}" />.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentType">Имя типа документа.</param>
        IDocumentStorageProvider<TDocument> GetStorageProvider<TDocument>(string documentType = null);
    }
}