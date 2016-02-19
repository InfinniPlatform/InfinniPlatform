namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Указатель на сортированный список документов для поиска.
    /// </summary>
    public interface IDocumentFindSortedCursor : IDocumentFindCursor
    {
        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor ThenBy(string property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentFindSortedCursor ThenByDescending(string property);

        /// <summary>
        /// Сортирует документы по убыванию релевантности, значение которой находится в указанном свойстве.
        /// </summary>
        IDocumentFindSortedCursor ThenByTextScore(string textScoreProperty = DocumentStorageExtensions.DefaultTextScoreProperty);
    }
}