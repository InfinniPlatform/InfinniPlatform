using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Abstractions.Interceptors
{
    /// <summary>
    /// Команда удаления набора документов из хранилища.
    /// </summary>
    public sealed class DocumentDeleteManyCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        public DocumentDeleteManyCommand(Func<IDocumentFilterBuilder, object> filter = null)
        {
            Filter = filter;
        }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Func<IDocumentFilterBuilder, object> Filter { get; set; }
    }


    /// <summary>
    /// Команда удаления набора документов из хранилища.
    /// </summary>
    public sealed class DocumentDeleteManyCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        public DocumentDeleteManyCommand(Expression<Func<TDocument, bool>> filter = null)
        {
            Filter = filter;
        }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; set; }
    }
}