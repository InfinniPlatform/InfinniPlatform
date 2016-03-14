using System;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Обработчик по умолчанию для сервиса по работе с документами.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DefaultDocumentHttpService<TDocument> : IDocumentHttpService<TDocument>
    {
        public DefaultDocumentHttpService(string documentType = null)
        {
            DocumentType = documentType;
        }


        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public string DocumentType { get; }


        /// <summary>
        /// Разрешено ли получение документов.
        /// </summary>
        public virtual bool CanGet { get; set; } = true;

        /// <summary>
        /// Вызывается перед получением документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        public virtual DocumentServiceResult<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery<TDocument> query)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual void OnAfterGet(DocumentGetQuery<TDocument> query, DocumentServiceResult<DocumentGetQueryResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Разрешено ли сохранение документов.
        /// </summary>
        public virtual bool CanPost { get; set; } = true;

        /// <summary>
        /// Вызывается перед сохранением документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        public virtual DocumentServiceResult<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery<TDocument> query)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual void OnAfterPost(DocumentPostQuery<TDocument> query, DocumentServiceResult<DocumentPostQueryResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Разрешено ли удаление документов.
        /// </summary>
        public virtual bool CanDelete { get; set; } = true;

        /// <summary>
        /// Вызывается перед удалением документов.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        public virtual DocumentServiceResult<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query)
        {
            return null;
        }

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual void OnAfterDelete(DocumentDeleteQuery query, DocumentServiceResult<DocumentDeleteQueryResult> result, Exception exception)
        {
        }


        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор обработчиков запросов.</param>
        public virtual void Load(IHttpServiceBuilder builder)
        {
        }
    }
}