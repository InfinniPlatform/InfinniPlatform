using System;
using System.Collections.Generic;

namespace InfinniPlatform.Api.ContextTypes
{
    /// <summary>
    ///   Контекст обработчика поиска документов
    /// </summary>
    public interface ISearchContext : ICommonContext
    {
        /// <summary>
        ///   Фильтр данных
        /// </summary>
        List<dynamic> Filter { get; set; }

		/// <summary>
		///   Номер страницы
		/// </summary>
		int PageNumber { get; set; }

		/// <summary>
		///  Размер страницы
		/// </summary>
		int PageSize { get; set; }

        /// <summary>
        ///   Результат поиска
        /// </summary>
        IEnumerable<dynamic> SearchResult { get; set; }

        /// <summary>
        ///   Наименование индекса
        /// </summary>
        string Index { get; set; }

        /// <summary>
        ///  Результат обработки документа
        /// </summary>
        [Obsolete("Не следует использовать в последующих конфигурациях. Удалить в ходе рефакторинга")]
        dynamic Result { get; set; }
    }
}
