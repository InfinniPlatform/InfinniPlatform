using System;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Фабрика для получения экземпляров <see cref="IDocumentStorage" /> и <see cref="IDocumentStorage{TDocument}" />.
    /// </summary>
    public interface IDocumentStorageFactory
    {
        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorage" />.
        /// </summary>
        /// <param name="documentTypeName">Имя типа документа.</param>
        IDocumentStorage GetStorage(string documentTypeName);

        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorage{TDocument}" />.
        /// </summary>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentTypeName">Имя типа документа.</param>
        object GetStorage(Type documentType, string documentTypeName = null);

        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorage{TDocument}" />.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="documentTypeName">Имя типа документа.</param>
        IDocumentStorage<TDocument> GetStorage<TDocument>(string documentTypeName = null) where TDocument : Document;
    }
}