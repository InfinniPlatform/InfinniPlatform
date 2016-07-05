using System.Collections.Generic;
using System.Diagnostics;

namespace InfinniPlatform.Sdk.Metadata.Documents
{
    /// <summary>
    /// Метаданные документа.
    /// </summary>
    [DebuggerDisplay("Name: {Type}, Indexes count: {Indexes.Count}")]
    public sealed class DocumentMetadata
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Индексы документа.
        /// </summary>
        public IList<DocumentIndex> Indexes { get; set; }
    }
}