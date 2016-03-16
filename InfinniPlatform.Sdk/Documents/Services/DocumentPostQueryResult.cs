using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на сохранение документа.
    /// </summary>
    public class DocumentPostQueryResult : DocumentQeuryResult
    {
        /// <summary>
        /// Идентификатор сохраненного документа.
        /// </summary>
        public object DocumentId { get; set; }

        /// <summary>
        /// Идентификаторы сохраненных файлов документа.
        /// </summary>
        public IDictionary<string, object> FileIds { get; set; }

        /// <summary>
        /// Результат выполнения обновления документа.
        /// </summary>
        public DocumentUpdateResult Status { get; set; }
    }
}