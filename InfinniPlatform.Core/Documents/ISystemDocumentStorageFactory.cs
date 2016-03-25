using System;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Documents
{
    /// <summary>
    /// Фабрика для получения экземпляров <see cref="ISystemDocumentStorage" /> и <see cref="ISystemDocumentStorage" />.
    /// </summary>
    public interface ISystemDocumentStorageFactory
    {
        /// <summary>
        /// Возвращает экземпляр <see cref="IDocumentStorage" />.
        /// </summary>
        /// <param name="documentTypeName">Имя типа документа.</param>
        ISystemDocumentStorage GetStorage(string documentTypeName);

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
        ISystemDocumentStorage<TDocument> GetStorage<TDocument>(string documentTypeName = null) where TDocument : Document;
    }
}