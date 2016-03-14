using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Предоставляет интерфейс для построения запросов к сервису документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public interface IDocumentQueryBuilder<TDocument>
    {
        /// <summary>
        /// Построить запрос на получение документов.
        /// </summary>
        DocumentGetQuery<TDocument> BuildGetQuery(IDictionary<string, object> query);
    }
}