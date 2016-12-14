namespace InfinniPlatform.DocumentStorage.Contract
{
    /// <summary>
    /// Статус выполнения операции обновления документа.
    /// </summary>
    public enum DocumentUpdateStatus
    {
        /// <summary>
        /// Ничего не изменилось.
        /// </summary>
        None,

        /// <summary>
        /// Создан документ.
        /// </summary>
        Inserted,

        /// <summary>
        /// Обновлен документ.
        /// </summary>
        Updated
    }
}