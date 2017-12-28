using InfinniPlatform.Dynamic;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Запрос на сохранение документа.
    /// </summary>
    public class DocumentPostQuery
    {
        /// <summary>
        /// Экземпляр документа.
        /// </summary>
        public DynamicDocument Document { get; set; }

        /// <summary>
        /// Список файлов документа.
        /// </summary>
        public IFormFileCollection Files { get; set; }
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
        public IFormFileCollection Files { get; set; }
    }
}