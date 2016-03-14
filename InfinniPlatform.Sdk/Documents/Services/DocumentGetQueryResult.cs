using System.Collections;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на получение документов.
    /// </summary>
    public class DocumentGetQueryResult
    {
        /// <summary>
        /// Список документов.
        /// </summary>
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Количество документов.
        /// </summary>
        public int? Count { get; set; }
    }
}