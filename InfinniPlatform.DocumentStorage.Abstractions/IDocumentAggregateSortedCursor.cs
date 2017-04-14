namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Указатель на сортированный список документов для агрегации.
    /// </summary>
    public interface IDocumentAggregateSortedCursor : IDocumentAggregateCursor
    {
        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor ThenBy(string property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor ThenByDescending(string property);
    }
}