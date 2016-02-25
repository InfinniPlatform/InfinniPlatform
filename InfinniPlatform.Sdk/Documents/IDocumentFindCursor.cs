using System;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Указатель на список документов для поиска.
    /// </summary>
    public interface IDocumentFindCursor : IDocumentCursor<DynamicWrapper>
    {
        /// <summary>
        /// Создает проекцию для выборки документов.
        /// </summary>
        IDocumentFindCursor Project(Action<IDocumentProjectionBuilder> projection);

        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor SortBy(string property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor SortByDescending(string property);

        /// <summary>
        /// Сортирует документы по убыванию релевантности, значение которой находится в указанном свойстве.
        /// </summary>
        IDocumentFindSortedCursor SortByTextScore(string textScoreProperty = DocumentStorageExtensions.DefaultTextScoreProperty);

        /// <summary>
        /// Пропускает указанное количество документов в результирующей выборке.
        /// </summary>
        IDocumentFindCursor Skip(int skip);

        /// <summary>
        /// Ограничивает результирующую выборку указанным количеством документов.
        /// </summary>
        IDocumentFindCursor Limit(int limit);
    }
}