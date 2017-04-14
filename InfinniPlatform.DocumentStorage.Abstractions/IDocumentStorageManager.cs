using System.Threading.Tasks;

using InfinniPlatform.Sdk.Metadata;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Предоставляет методы для управления хранилищем документов.
    /// </summary>
    public interface IDocumentStorageManager
    {
        /// <summary>
        /// Создает хранилище данных для заданного типа документа.
        /// </summary>
        /// <param name="documentMetadata">Метаданные документа.</param>
        Task CreateStorageAsync(DocumentMetadata documentMetadata);

        /// <summary>
        /// Изменяет наименование хранилища данных для заданного типа документа.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="newDocumnetType">Новое имя типа документа.</param>
        /// <returns></returns>
        Task RenameStorageAsync(string documentType, string newDocumnetType);

        /// <summary>
        /// Удаляет хранилище данных для заданного типа документа.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        Task DropStorageAsync(string documentType);
    }
}