namespace InfinniPlatform.DocumentStorage.Obsolete
{
    /// <summary>
    /// Команда над документом при выполнении транзакции.
    /// </summary>
    public sealed class DocumentTransactionCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="action">Тип команды над документом.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <param name="document">Экземпляр документа.</param>
        private DocumentTransactionCommand(DocumentTransactionAction action, string documentType, object documentId, object document)
        {
            Action = action;
            DocumentType = documentType;
            DocumentId = documentId;
            Document = document;
        }


        public static DocumentTransactionCommand SaveCommand(string documentType, object documentId, object document)
        {
            return new DocumentTransactionCommand(DocumentTransactionAction.Save, documentType, documentId, document);
        }

        public static DocumentTransactionCommand DeleteCommand(string documentType, object documentId)
        {
            return new DocumentTransactionCommand(DocumentTransactionAction.Delete, documentType, documentId, null);
        }


        /// <summary>
        /// Тип команды над документом.
        /// </summary>
        public readonly DocumentTransactionAction Action;

        /// <summary>
        /// Тип документа.
        /// </summary>
        public readonly string DocumentType;

        /// <summary>
        /// Документ.
        /// </summary>
        public readonly object DocumentId;

        /// <summary>
        /// Документ.
        /// </summary>
        public readonly object Document;
    }
}