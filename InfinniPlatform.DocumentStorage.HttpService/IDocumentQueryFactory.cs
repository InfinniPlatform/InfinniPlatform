﻿using InfinniPlatform.Http;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс для создания запросов к сервису документов.
    /// </summary>
    public interface IDocumentQueryFactory
    {
        /// <summary>
        /// Создает запрос на получение документов.
        /// </summary>
        DocumentGetQuery CreateGetQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey);

        /// <summary>
        /// Создает запрос на сохранение документа.
        /// </summary>
        DocumentPostQuery CreatePostQuery(IHttpRequest request, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey);

        /// <summary>
        /// Создает запрос на удаление документа.
        /// </summary>
        DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey);
    }


    /// <summary>
    /// Предоставляет интерфейс для создания запросов к сервису документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public interface IDocumentQueryFactory<TDocument>
    {
        /// <summary>
        /// Создает запрос на получение документов.
        /// </summary>
        DocumentGetQuery<TDocument> CreateGetQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey);

        /// <summary>
        /// Создает запрос на сохранение документа.
        /// </summary>
        DocumentPostQuery<TDocument> CreatePostQuery(IHttpRequest request, string documentFormKey = DocumentHttpServiceConstants.DocumentFormKey);

        /// <summary>
        /// Создает запрос на удаление документа.
        /// </summary>
        DocumentDeleteQuery<TDocument> CreateDeleteQuery(IHttpRequest request, string documentIdKey = DocumentHttpServiceConstants.DocumentIdKey);
    }
}