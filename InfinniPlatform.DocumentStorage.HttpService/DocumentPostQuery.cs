using System.Collections.Generic;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Http;

namespace InfinniPlatform.DocumentStorage.HttpService
{
    /// <summary>
    /// Запрос на сохранение документа.
    /// </summary>
    public class DocumentPostQuery
    {
        /// <summary>
        /// Экземпляр документа.
        /// </summary>
        public DynamicWrapper Document { get; set; }

        /// <summary>
        /// Список файлов документа.
        /// </summary>
        public IEnumerable<IHttpRequestFile> Files { get; set; }
    }


    /// <summary>
    /// Запрос на сохранение документа.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentPostQuery<TDocument>
    {
        /// <summary>
        /// Экземпляр документа.
        /// </summary>
        public TDocument Document { get; set; }

        /// <summary>
        /// Список файлов документа.
        /// </summary>
        public IEnumerable<IHttpRequestFile> Files { get; set; }
    }
}