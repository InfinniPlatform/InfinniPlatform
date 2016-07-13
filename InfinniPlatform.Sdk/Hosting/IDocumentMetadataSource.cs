using System.Collections.Generic;

using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.DocumentStorage.Hosting
{
    /// <summary>
    /// Инициализирует коллекции.
    /// </summary>
    public interface IDocumentMetadataSource
    {
        /// <summary>
        /// Возвращает метаданные документов.
        /// </summary>
        IEnumerable<DocumentMetadata> GetDocumentsMetadata();
    }
}