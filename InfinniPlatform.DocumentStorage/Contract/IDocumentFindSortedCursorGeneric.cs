using System;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Contract
{
    /// <summary>
    /// Указатель на сортированный список документов для поиска.
    /// </summary>
    public interface IDocumentFindSortedCursor<TDocument, TProjection> : IDocumentFindCursor<TDocument, TProjection>
    {
        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> ThenBy(Expression<Func<TDocument, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> ThenByDescending(Expression<Func<TDocument, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию релевантности, значение которой находится в указанном свойстве.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> ThenByTextScore(Expression<Func<TProjection, object>> textScoreProperty);
    }
}