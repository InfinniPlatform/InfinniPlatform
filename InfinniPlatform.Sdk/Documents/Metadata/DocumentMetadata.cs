using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents.Metadata
{
    /// <summary>
    /// Метаданные документа.
    /// </summary>
    public sealed class DocumentMetadata
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Список индексов документа.
        /// </summary>
        public IList<DocumentIndex> Indexes { get; set; }
    }
}