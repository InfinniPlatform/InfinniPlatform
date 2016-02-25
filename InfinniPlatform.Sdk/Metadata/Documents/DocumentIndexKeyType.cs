namespace InfinniPlatform.Sdk.Metadata.Documents
{
    /// <summary>
    /// Тип индексации ключевого поля документа.
    /// </summary>
    public enum DocumentIndexKeyType
    {
        /// <summary>
        /// Сортировка по возрастанию.
        /// </summary>
        Asc,

        /// <summary>
        /// Сортировка по убыванию.
        /// </summary>
        Desc,

        /// <summary>
        /// Текстовый индекс.
        /// </summary>
        Text
    }
}