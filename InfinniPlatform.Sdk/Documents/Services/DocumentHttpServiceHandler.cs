using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Обработчик по умолчанию для сервиса по работе с документами.
    /// </summary>
    public class DocumentHttpServiceHandler : IDocumentHttpServiceHandler
    {
        public DocumentHttpServiceHandler(string documentType)
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
        public virtual Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery query)
        {
            return Task.FromResult<DocumentGetQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterGet(DocumentGetQuery query, DocumentGetQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
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
        public virtual Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery query)
        {
            return Task.FromResult<DocumentPostQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterPost(DocumentPostQuery query, DocumentPostQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
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
        public virtual Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query)
        {
            return Task.FromResult<DocumentDeleteQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterDelete(DocumentDeleteQuery query, DocumentDeleteQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор обработчиков запросов.</param>
        public virtual void Load(IHttpServiceBuilder builder)
        {
        }
    }


    /// <summary>
    /// Обработчик по умолчанию для сервиса по работе с документами.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentHttpServiceHandler<TDocument> : IDocumentHttpServiceHandler<TDocument> where TDocument : Document
    {
        public DocumentHttpServiceHandler(string documentType = null)
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
        public virtual Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery<TDocument> query)
        {
            return Task.FromResult<DocumentGetQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterGet(DocumentGetQuery<TDocument> query, DocumentGetQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
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
        public virtual Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery<TDocument> query)
        {
            return Task.FromResult<DocumentPostQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterPost(DocumentPostQuery<TDocument> query, DocumentPostQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
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
        public virtual Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query)
        {
            return Task.FromResult<DocumentDeleteQueryResult>(null);
        }

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        public virtual Task OnAfterDelete(DocumentDeleteQuery query, DocumentDeleteQueryResult result, Exception exception)
        {
            return Task.FromResult<object>(null);
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