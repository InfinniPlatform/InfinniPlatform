using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Запрос на удаление документа.
    /// </summary>
    public class DocumentDeleteQuery
    {
        /// <summary>
        /// Правило фильтрации документов.
        /// </summary>
        public Func<IDocumentFilterBuilder, object> Filter { get; set; }
    }


    /// <summary>
    /// Запрос на удаление документа.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public class DocumentDeleteQuery<TDocument>
    {
        /// <summary>
        /// Правило фильтрации документов.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; set; }
    }
}