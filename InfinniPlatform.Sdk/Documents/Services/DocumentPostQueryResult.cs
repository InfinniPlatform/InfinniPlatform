using System.Collections.Generic;

using InfinniPlatform.Sdk.BlobStorage;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на сохранение документа.
    /// </summary>
    public class DocumentPostQueryResult : DocumentQueryResult
    {
        /// <summary>
        /// Идентификатор сохраненного документа.
        /// </summary>
        public object DocumentId { get; set; }

        /// <summary>
        /// Информация о сохраненных файлах документа.
        /// </summary>
        public IDictionary<string, BlobInfo> FileInfos { get; set; }

        /// <summary>
        /// Результат выполнения обновления документа.
        /// </summary>
        public DocumentUpdateResult Status { get; set; }
    }
}