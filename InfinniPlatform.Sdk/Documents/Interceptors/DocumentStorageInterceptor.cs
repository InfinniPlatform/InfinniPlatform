using System;

namespace InfinniPlatform.Sdk.Documents.Interceptors
{
    /// <summary>
    /// Базовый класс для обработчика событий изменения документов в хранилище.
    /// </summary>
    public abstract class DocumentStorageInterceptor : IDocumentStorageInterceptor
    {
        protected DocumentStorageInterceptor(string documentType)
        {
            DocumentType = documentType;
        }


        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string DocumentType { get; }


        /// <summary>
        /// Вызывается перед вставкой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <returns>Результат вставки одного документа.</returns>
        public virtual DocumentStorageWriteResult<object> OnBeforeInsertOne(DocumentInsertOneCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <param name="result">Результат вставки одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterInsertOne(DocumentInsertOneCommand command, DocumentStorageWriteResult<object> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <returns>Результат вставки набора документов.</returns>
        public virtual DocumentStorageWriteResult<object> OnBeforeInsertMany(DocumentInsertManyCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <param name="result">Результат вставки набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterInsertMany(DocumentInsertManyCommand command, DocumentStorageWriteResult<object> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <returns>Результат выполнения команды обновления документа.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateOne(DocumentUpdateOneCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <param name="result">Результат выполнения команды обновления документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterUpdateOne(DocumentUpdateOneCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед обновлением набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <returns>Результат выполнения команды обновления набора документов.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateMany(DocumentUpdateManyCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после обновления набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <param name="result">Результат выполнения команды обновления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterUpdateMany(DocumentUpdateManyCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед заменой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <returns>Результат выполнения команды замены одного документа.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeReplaceOne(DocumentReplaceOneCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после замены одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <param name="result">Результат выполнения команды замены одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterReplaceOne(DocumentReplaceOneCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед удалением одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <returns>Результат выполнения команды удаления одного документа.</returns>
        public virtual DocumentStorageWriteResult<long> OnBeforeDeleteOne(DocumentDeleteOneCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после удаления одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <param name="result">Результат выполнения команды удаления одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterDeleteOne(DocumentDeleteOneCommand command, DocumentStorageWriteResult<long> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед удалением набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <returns>Результат выполнения команды удаления набора документов.</returns>
        public virtual DocumentStorageWriteResult<long> OnBeforeDeleteMany(DocumentDeleteManyCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после удаления набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <param name="result">Результат выполнения команды удаления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterDeleteMany(DocumentDeleteManyCommand command, DocumentStorageWriteResult<long> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед выполнением набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <returns>Результат выполнения набора команд изменения документов.</returns>
        public virtual DocumentStorageWriteResult<DocumentBulkResult> OnBeforeBulk(DocumentBulkCommand command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после выполнения набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <param name="result">Результат выполнения набора команд изменения документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterBulk(DocumentBulkCommand command, DocumentStorageWriteResult<DocumentBulkResult> result, Exception exception)
        {
        }
    }


    /// <summary>
    /// Базовый класс для обработчика событий изменения документов в хранилище.
    /// </summary>
    public abstract class DocumentStorageInterceptor<TDocument> : DocumentStorageInterceptor, IDocumentStorageInterceptor<TDocument>
    {
        protected DocumentStorageInterceptor(string documentType = null)
            : base(string.IsNullOrEmpty(documentType) ? DocumentStorageExtensions.GetDefaultDocumentTypeName<TDocument>() : documentType)
        {
        }


        /// <summary>
        /// Вызывается перед вставкой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <returns>Результат вставки одного документа.</returns>
        public virtual DocumentStorageWriteResult<object> OnBeforeInsertOne(DocumentInsertOneCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <param name="result">Результат вставки одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterInsertOne(DocumentInsertOneCommand<TDocument> command, DocumentStorageWriteResult<object> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <returns>Результат вставки набора документов.</returns>
        public virtual DocumentStorageWriteResult<object> OnBeforeInsertMany(DocumentInsertManyCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <param name="result">Результат вставки набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterInsertMany(DocumentInsertManyCommand<TDocument> command, DocumentStorageWriteResult<object> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <returns>Результат выполнения команды обновления документа.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateOne(DocumentUpdateOneCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <param name="result">Результат выполнения команды обновления документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterUpdateOne(DocumentUpdateOneCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед обновлением набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <returns>Результат выполнения команды обновления набора документов.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateMany(DocumentUpdateManyCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после обновления набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <param name="result">Результат выполнения команды обновления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterUpdateMany(DocumentUpdateManyCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед заменой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <returns>Результат выполнения команды замены одного документа.</returns>
        public virtual DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeReplaceOne(DocumentReplaceOneCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после замены одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <param name="result">Результат выполнения команды замены одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterReplaceOne(DocumentReplaceOneCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед удалением одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <returns>Результат выполнения команды удаления одного документа.</returns>
        public virtual DocumentStorageWriteResult<long> OnBeforeDeleteOne(DocumentDeleteOneCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после удаления одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <param name="result">Результат выполнения команды удаления одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterDeleteOne(DocumentDeleteOneCommand<TDocument> command, DocumentStorageWriteResult<long> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед удалением набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <returns>Результат выполнения команды удаления набора документов.</returns>
        public virtual DocumentStorageWriteResult<long> OnBeforeDeleteMany(DocumentDeleteManyCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после удаления набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <param name="result">Результат выполнения команды удаления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterDeleteMany(DocumentDeleteManyCommand<TDocument> command, DocumentStorageWriteResult<long> result, Exception exception)
        {
        }


        /// <summary>
        /// Вызывается перед выполнением набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <returns>Результат выполнения набора команд изменения документов.</returns>
        public virtual DocumentStorageWriteResult<DocumentBulkResult> OnBeforeBulk(DocumentBulkCommand<TDocument> command)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после выполнения набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <param name="result">Результат выполнения набора команд изменения документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        public virtual void OnAfterBulk(DocumentBulkCommand<TDocument> command, DocumentStorageWriteResult<DocumentBulkResult> result, Exception exception)
        {
        }
    }
}