using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes.ContextImpl
{
    /// <summary>
    ///     Контекст поиска документов
    /// </summary>
    public sealed class SearchContext : ISearchContext
    {
        /// <summary>
        ///     Глобальный контекст обработки
        /// </summary>
        public IGlobalContext Context { get; set; }

        /// <summary>
        ///     Наименование индекса
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        ///     Результат обработки документа
        /// </summary>
        public dynamic Result { get; set; }

        /// <summary>
        ///     Фильтр данных
        /// </summary>
        public List<dynamic> Filter { get; set; }

        /// <summary>
        ///     Номер страницы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        ///     Размер страницы
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Результат поиска
        /// </summary>
        public IEnumerable<dynamic> SearchResult { get; set; }

        public bool IsInternalServerError { get; set; }

        /// <summary>
        ///     Конфигурация текущего запроса
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        ///     Метаданные текущего запроса
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        ///     Действие текущего запроса
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     Авторизованный пользователь системы
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Результат фильтрации событий
        /// </summary>
        public dynamic ValidationMessage { get; set; }

        /// <summary>
        ///     Признак успешности обработки события фильтрации событий
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        ///     Наименование типа в индексе, соответствующем конфигурации
        /// </summary>
        public string IndexType { get; set; }
    }
}