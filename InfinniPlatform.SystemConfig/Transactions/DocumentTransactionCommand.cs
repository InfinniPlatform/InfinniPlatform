namespace InfinniPlatform.SystemConfig.Transactions
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
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="documentId">Идентификатор документа.</param>
        /// <param name="document">Экземпляр документа.</param>
        private DocumentTransactionCommand(DocumentTransactionAction action, string configuration, string documentType, object documentId, object document)
        {
            Action = action;
            Configuration = configuration;
            DocumentType = documentType;
            DocumentId = documentId;
            Document = document;
        }


        public static DocumentTransactionCommand SaveCommand(string configuration, string documentType, object documentId, object document)
        {
            return new DocumentTransactionCommand(DocumentTransactionAction.Save, configuration, documentType, documentId, document);
        }

        public static DocumentTransactionCommand DeleteCommand(string configuration, string documentType, object documentId)
        {
            return new DocumentTransactionCommand(DocumentTransactionAction.Delete, configuration, documentType, documentId, null);
        }


        /// <summary>
        /// Тип команды над документом.
        /// </summary>
        public readonly DocumentTransactionAction Action;

        /// <summary>
        /// Имя конфигурации.
        /// </summary>
        public readonly string Configuration;

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