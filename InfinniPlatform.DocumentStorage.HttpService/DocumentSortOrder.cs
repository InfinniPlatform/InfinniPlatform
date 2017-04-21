namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Направление сортировки документов.
    /// </summary>
    public enum DocumentSortOrder
    {
        /// <summary>
        /// Сортировка по возрастанию значения свойства документа.
        /// </summary>
        Asc,

        /// <summary>
        /// Сортировка по убыванию значения свойства документа.
        /// </summary>
        Desc,

        /// <summary>
        /// Сортировка по убыванию релевантности полнотекстового поиска.
        /// </summary>
        TextScore
    }
}