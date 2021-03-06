﻿using System;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Обработчик для сервиса по работе с документами.
    /// </summary>
    public interface IDocumentHttpServiceHandler : IDocumentHttpServiceHandlerBase
    {
        /// <summary>
        /// Вызывается перед получением документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery query);

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterGet(DocumentGetQuery query, DocumentGetQueryResult result, Exception exception);


        /// <summary>
        /// Вызывается перед сохранением документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery query);

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterPost(DocumentPostQuery query, DocumentPostQueryResult result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением документов.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query);

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterDelete(DocumentDeleteQuery query, DocumentDeleteQueryResult result, Exception exception);
    }


    /// <summary>
    /// Обработчик для сервиса по работе с документами.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public interface IDocumentHttpServiceHandler<TDocument> : IDocumentHttpServiceHandlerBase where TDocument : Document
    {
        /// <summary>
        /// Вызывается перед получением документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery<TDocument> query);

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterGet(DocumentGetQuery<TDocument> query, DocumentGetQueryResult result, Exception exception);


        /// <summary>
        /// Вызывается перед сохранением документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery<TDocument> query);

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterPost(DocumentPostQuery<TDocument> query, DocumentPostQueryResult result, Exception exception);


        /// <summary>
        /// Вызывается перед удалением документов.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        Task<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery<TDocument> query);

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        Task OnAfterDelete(DocumentDeleteQuery<TDocument> query, DocumentDeleteQueryResult result, Exception exception);
    }
}