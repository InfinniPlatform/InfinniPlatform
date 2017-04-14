using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Abstractions.Interceptors
{
    /// <summary>
    /// Команда вставки одного документа в хранилище.
    /// </summary>
    public sealed class DocumentInsertOneCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        public DocumentInsertOneCommand(DynamicWrapper document)
        {
            Document = document;
        }

        /// <summary>
        /// Документ для вставки.
        /// </summary>
        public DynamicWrapper Document { get; set; }
    }


    /// <summary>
    /// Команда вставки одного документа в хранилище.
    /// </summary>
    public sealed class DocumentInsertOneCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="document">Документ для вставки.</param>
        public DocumentInsertOneCommand(TDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Документ для вставки.
        /// </summary>
        public TDocument Document { get; set; }
    }
}