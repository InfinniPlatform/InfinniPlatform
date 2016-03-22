using System.Collections.Generic;

namespace InfinniPlatform.Sdk.RestApi.Documents
{
    /// <summary>
    /// Запрос на сохранение документа.
    /// </summary>
    public sealed class DocumentPostQueryClient
    {
        /// <summary>
        /// Экземпляр документа.
        /// </summary>
        public object Document { get; set; }

        /// <summary>
        /// Список файлов документа.
        /// </summary>
        public IEnumerable<HttpRequestFileClient> Files { get; set; }
    }
}