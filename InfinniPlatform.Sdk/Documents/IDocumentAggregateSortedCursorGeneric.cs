using System;
using System.Linq.Expressions;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Указатель на сортированный список документов для агрегации.
    /// </summary>
    public interface IDocumentAggregateSortedCursor<TResult> : IDocumentAggregateCursor<TResult>
    {
        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor<TResult> ThenBy(Expression<Func<TResult, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor<TResult> ThenByDescending(Expression<Func<TResult, object>> property);
    }
}