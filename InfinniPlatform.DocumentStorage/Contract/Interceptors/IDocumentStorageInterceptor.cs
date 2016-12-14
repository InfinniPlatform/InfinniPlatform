using System;

namespace InfinniPlatform.DocumentStorage.Contract.Interceptors
{
    /// <summary>
    /// Обработчик событий изменения документов в хранилище.
    /// </summary>
    public interface IDocumentStorageInterceptor
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        string DocumentType { get; }


        /// <summary>
        /// Вызывается перед вставкой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <returns>Результат вставки одного документа.</returns>
        DocumentStorageWriteResult<object> OnBeforeInsertOne(DocumentInsertOneCommand command);

        /// <summary>
        /// Вызывается после вставки одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <param name="result">Результат вставки одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterInsertOne(DocumentInsertOneCommand command, DocumentStorageWriteResult<object> result, Exception exception);


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <returns>Результат вставки набора документов.</returns>
        DocumentStorageWriteResult<object> OnBeforeInsertMany(DocumentInsertManyCommand command);

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <param name="result">Результат вставки набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterInsertMany(DocumentInsertManyCommand command, DocumentStorageWriteResult<object> result, Exception exception);


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <returns>Результат выполнения команды обновления документа.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateOne(DocumentUpdateOneCommand command);

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <param name="result">Результат выполнения команды обновления документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterUpdateOne(DocumentUpdateOneCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед обновлением набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <returns>Результат выполнения команды обновления набора документов.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateMany(DocumentUpdateManyCommand command);

        /// <summary>
        /// Вызывается после обновления набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <param name="result">Результат выполнения команды обновления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterUpdateMany(DocumentUpdateManyCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед заменой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <returns>Результат выполнения команды замены одного документа.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeReplaceOne(DocumentReplaceOneCommand command);

        /// <summary>
        /// Вызывается после замены одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <param name="result">Результат выполнения команды замены одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterReplaceOne(DocumentReplaceOneCommand command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <returns>Результат выполнения команды удаления одного документа.</returns>
        DocumentStorageWriteResult<long> OnBeforeDeleteOne(DocumentDeleteOneCommand command);

        /// <summary>
        /// Вызывается после удаления одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <param name="result">Результат выполнения команды удаления одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterDeleteOne(DocumentDeleteOneCommand command, DocumentStorageWriteResult<long> result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <returns>Результат выполнения команды удаления набора документов.</returns>
        DocumentStorageWriteResult<long> OnBeforeDeleteMany(DocumentDeleteManyCommand command);

        /// <summary>
        /// Вызывается после удаления набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <param name="result">Результат выполнения команды удаления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterDeleteMany(DocumentDeleteManyCommand command, DocumentStorageWriteResult<long> result, Exception exception);


        /// <summary>
        /// Вызывается перед выполнением набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <returns>Результат выполнения набора команд изменения документов.</returns>
        DocumentStorageWriteResult<DocumentBulkResult> OnBeforeBulk(DocumentBulkCommand command);

        /// <summary>
        /// Вызывается после выполнения набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <param name="result">Результат выполнения набора команд изменения документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterBulk(DocumentBulkCommand command, DocumentStorageWriteResult<DocumentBulkResult> result, Exception exception);
    }


    /// <summary>
    /// Обработчик событий изменения документов в хранилище.
    /// </summary>
    public interface IDocumentStorageInterceptor<TDocument> : IDocumentStorageInterceptor
    {
        /// <summary>
        /// Вызывается перед вставкой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <returns>Результат вставки одного документа.</returns>
        DocumentStorageWriteResult<object> OnBeforeInsertOne(DocumentInsertOneCommand<TDocument> command);

        /// <summary>
        /// Вызывается после вставки одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки одного документа.</param>
        /// <param name="result">Результат вставки одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterInsertOne(DocumentInsertOneCommand<TDocument> command, DocumentStorageWriteResult<object> result, Exception exception);


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <returns>Результат вставки набора документов.</returns>
        DocumentStorageWriteResult<object> OnBeforeInsertMany(DocumentInsertManyCommand<TDocument> command);

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда вставки набора документов.</param>
        /// <param name="result">Результат вставки набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterInsertMany(DocumentInsertManyCommand<TDocument> command, DocumentStorageWriteResult<object> result, Exception exception);


        /// <summary>
        /// Вызывается перед вставкой набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <returns>Результат выполнения команды обновления документа.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateOne(DocumentUpdateOneCommand<TDocument> command);

        /// <summary>
        /// Вызывается после вставки набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления одного документа.</param>
        /// <param name="result">Результат выполнения команды обновления документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterUpdateOne(DocumentUpdateOneCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед обновлением набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <returns>Результат выполнения команды обновления набора документов.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeUpdateMany(DocumentUpdateManyCommand<TDocument> command);

        /// <summary>
        /// Вызывается после обновления набора документов в хранилище.
        /// </summary>
        /// <param name="command">Команда обновления набора документов.</param>
        /// <param name="result">Результат выполнения команды обновления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterUpdateMany(DocumentUpdateManyCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед заменой одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <returns>Результат выполнения команды замены одного документа.</returns>
        DocumentStorageWriteResult<DocumentUpdateResult> OnBeforeReplaceOne(DocumentReplaceOneCommand<TDocument> command);

        /// <summary>
        /// Вызывается после замены одного документа в хранилище.
        /// </summary>
        /// <param name="command">Команда замены одного документа.</param>
        /// <param name="result">Результат выполнения команды замены одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterReplaceOne(DocumentReplaceOneCommand<TDocument> command, DocumentStorageWriteResult<DocumentUpdateResult> result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <returns>Результат выполнения команды удаления одного документа.</returns>
        DocumentStorageWriteResult<long> OnBeforeDeleteOne(DocumentDeleteOneCommand<TDocument> command);

        /// <summary>
        /// Вызывается после удаления одного документа из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления одного документа.</param>
        /// <param name="result">Результат выполнения команды удаления одного документа.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterDeleteOne(DocumentDeleteOneCommand<TDocument> command, DocumentStorageWriteResult<long> result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <returns>Результат выполнения команды удаления набора документов.</returns>
        DocumentStorageWriteResult<long> OnBeforeDeleteMany(DocumentDeleteManyCommand<TDocument> command);

        /// <summary>
        /// Вызывается после удаления набора документов из хранилища.
        /// </summary>
        /// <param name="command">Команда удаления набора документов.</param>
        /// <param name="result">Результат выполнения команды удаления набора документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterDeleteMany(DocumentDeleteManyCommand<TDocument> command, DocumentStorageWriteResult<long> result, Exception exception);


        /// <summary>
        /// Вызывается перед выполнением набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <returns>Результат выполнения набора команд изменения документов.</returns>
        DocumentStorageWriteResult<DocumentBulkResult> OnBeforeBulk(DocumentBulkCommand<TDocument> command);

        /// <summary>
        /// Вызывается после выполнения набора команд изменения документов в рамках одного запроса к хранилищу.
        /// </summary>
        /// <param name="command">Набор команд изменения документов.</param>
        /// <param name="result">Результат выполнения набора команд изменения документов.</param>
        /// <param name="exception">Исключение, возникшее при выполнении команды.</param>
        void OnAfterBulk(DocumentBulkCommand<TDocument> command, DocumentStorageWriteResult<DocumentBulkResult> result, Exception exception);
    }
}