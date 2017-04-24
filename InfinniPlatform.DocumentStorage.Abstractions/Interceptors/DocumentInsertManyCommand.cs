using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.DocumentStorage.Interceptors
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
        public DocumentInsertManyCommand(IEnumerable<DynamicDocument> documents)
        {
            // Производится материализация коллекции, чтобы избежать ситуаций, когда
            // экземпляры документов создаются при каждом перечислении коллекции,
            // например: InsertMany(sources.Select(i => new DynamicDocument()))

            Documents = documents.ToList();
        }

        /// <summary>
        /// Документы для вставки.
        /// </summary>
        public IEnumerable<DynamicDocument> Documents { get; set; }
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
            // Производится материализация коллекции, чтобы избежать ситуаций, когда
            // экземпляры документов создаются при каждом перечислении коллекции,
            // например: InsertMany(sources.Select(i => new TDocument()))

            Documents = documents.ToList();
        }

        /// <summary>
        /// Документы для вставки.
        /// </summary>
        public IEnumerable<TDocument> Documents { get; set; }
    }
}