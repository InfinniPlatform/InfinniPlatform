using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Metadata
{
    /// <summary>
    /// Источник метаданных документов.
    /// </summary>
    public interface IDocumentMetadataSource
    {
        /// <summary>
        /// Возвращает метаданные документов.
        /// </summary>
        IEnumerable<DocumentMetadata> GetDocumentsMetadata();
    }
}