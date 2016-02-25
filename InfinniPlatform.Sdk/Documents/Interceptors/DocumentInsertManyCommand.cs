using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Documents.Interceptors
{
    /// <summary>
    /// Команда вставки набора документов в хранилище.
    /// </summary>
    public sealed class DocumentInsertManyCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="documents">Документы для вставки.</param>
        public DocumentInsertManyCommand(IEnumerable<DynamicWrapper> documents)
        {
            Documents = documents;
        }

        /// <summary>
        /// Документы для вставки.
        /// </summary>
        public IEnumerable<DynamicWrapper> Documents { get; set; }
    }


    /// <summary>
    /// Команда вставки набора документов в хранилище.
    /// </summary>
    public sealed class DocumentInsertManyCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="documents">Документы для вставки.</param>
        public DocumentInsertManyCommand(IEnumerable<TDocument> documents)
        {
            Documents = documents;
        }

        /// <summary>
        /// Документы для вставки.
        /// </summary>
        public IEnumerable<TDocument> Documents { get; set; }
    }
}