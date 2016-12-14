using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage.Contract
{
    /// <summary>
    /// Указатель на список документов для поиска.
    /// </summary>
    public interface IDocumentFindCursor<TDocument, TProjection> : IDocumentCursor<TProjection>
    {
        /// <summary>
        /// Добавляет условие фильтрации.
        /// </summary>
        /// <param name="filter">Фильтр для поиска документов.</param>
        IDocumentFindCursor<TDocument, TProjection> Where(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Создает проекцию для выборки документов.
        /// </summary>
        IDocumentFindCursor<TDocument, TNewProjection> Project<TNewProjection>(Expression<Func<TDocument, TNewProjection>> projection);

        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> SortBy(Expression<Func<TDocument, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> SortByDescending(Expression<Func<TDocument, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию релевантности, значение которой находится в указанном свойстве.
        /// </summary>
        IDocumentFindSortedCursor<TDocument, TProjection> SortByTextScore(Expression<Func<TProjection, object>> textScoreProperty);

        /// <summary>
        /// Пропускает указанное количество документов в результирующей выборке.
        /// </summary>
        IDocumentFindCursor<TDocument, TProjection> Skip(int skip);

        /// <summary>
        /// Ограничивает результирующую выборку указанным количеством документов.
        /// </summary>
        IDocumentFindCursor<TDocument, TProjection> Limit(int limit);

        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        long Count();

        /// <summary>
        /// Возвращает количество документов, удовлетворяющих указанному фильтру.
        /// </summary>
        Task<long> CountAsync();
    }
}