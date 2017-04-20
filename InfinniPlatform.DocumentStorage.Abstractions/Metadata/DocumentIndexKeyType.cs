namespace InfinniPlatform.DocumentStorage.Metadata
{
    /// <summary>
    /// Тип индексации ключевого поля документа.
    /// </summary>
    public enum DocumentIndexKeyType
    {
        /// <summary>
        /// Сортировка по возрастанию.
        /// </summary>
        /// <remarks>
        /// Определяет, что поле индекса сортируется по возрастанию значения.
        /// </remarks>
        Asc,

        /// <summary>
        /// Сортировка по убыванию.
        /// </summary>
        /// <remarks>
        /// Определяет, что поле индекса сортируется по убыванию значения.
        /// </remarks>
        Desc,

        /// <summary>
        /// Текстовый индекс.
        /// </summary>
        /// <remarks>
        /// Определяет, что поле индекса используется в текстовом поиске.
        /// </remarks>
        Text,

        /// <summary>
        /// Индекс TTL.
        /// </summary>
        /// <remarks>
        /// Определяет, что поле индекса используется для определения TTL (Time To Live).
        /// </remarks>
        Ttl
    }
}