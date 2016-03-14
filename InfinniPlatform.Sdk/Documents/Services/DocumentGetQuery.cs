using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Запрос на получение документов.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentGetQuery<TDocument>
    {
        /// <summary>
        /// Строка полнотекстового поиска.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Правило фильтрации документов.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; set; }

        /// <summary>
        /// Правило отображения документов.
        /// </summary>
        public Expression<Func<TDocument, object>> Select { get; set; }

        /// <summary>
        /// Правило сортировки документов.
        /// </summary>
        public IDictionary<Expression<Func<TDocument, object>>, DocumentSortOrder> Order { get; set; }

        /// <summary>
        /// Необходимость подсчета количества.
        /// </summary>
        public bool Count { get; set; }

        /// <summary>
        /// Количество документов, которое нужно пропустить.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Максимальное количество документов, которое нужно выбрать.
        /// </summary>
        public int? Take { get; set; }
    }
}