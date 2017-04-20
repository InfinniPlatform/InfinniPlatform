using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Interceptors
{
    /// <summary>
    /// Команда удаления одного документа из хранилища.
    /// </summary>
    public sealed class DocumentDeleteOneCommand : IDocumentWriteCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        public DocumentDeleteOneCommand(Func<IDocumentFilterBuilder, object> filter = null)
        {
            Filter = filter;
        }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Func<IDocumentFilterBuilder, object> Filter { get; set; }
    }


    /// <summary>
    /// Команда удаления одного документа из хранилища.
    /// </summary>
    public sealed class DocumentDeleteOneCommand<TDocument> : IDocumentWriteCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        public DocumentDeleteOneCommand(Expression<Func<TDocument, bool>> filter = null)
        {
            Filter = filter;
        }

        /// <summary>
        /// Фильтр для поиска документов.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; set; }
    }
}