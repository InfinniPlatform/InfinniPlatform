using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Предоставляет интерфейс для создания запросов к сервису документов.
    /// </summary>
    public interface IDocumentQueryFactory
    {
        /// <summary>
        /// Создает запрос на получение документов.
        /// </summary>
        DocumentGetQuery CreateGetQuery(IHttpRequest request);

        /// <summary>
        /// Создает запрос на сохранение документа.
        /// </summary>
        DocumentPostQuery CreatePostQuery(IHttpRequest request, string documentFormKey);

        /// <summary>
        /// Создает запрос на удаление документа.
        /// </summary>
        DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey);
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
        DocumentGetQuery<TDocument> CreateGetQuery(IHttpRequest request);

        /// <summary>
        /// Создает запрос на сохранение документа.
        /// </summary>
        DocumentPostQuery<TDocument> CreatePostQuery(IHttpRequest request, string documentFormKey);

        /// <summary>
        /// Создает запрос на удаление документа.
        /// </summary>
        DocumentDeleteQuery CreateDeleteQuery(IHttpRequest request, string documentIdKey);
    }
}