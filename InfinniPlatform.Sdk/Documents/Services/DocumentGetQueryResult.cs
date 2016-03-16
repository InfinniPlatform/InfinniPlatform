using System.Collections;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на получение документов.
    /// </summary>
    public class DocumentGetQueryResult : DocumentQeuryResult
    {
        /// <summary>
        /// Список документов.
        /// </summary>
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Количество документов.
        /// </summary>
        public long? Count { get; set; }
    }
}