namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Контекст обработчика бизнес-логики.
    /// </summary>
    public interface IActionContext
    {
        /// <summary>
        /// Конфигурация текущего запроса.
        /// </summary>
        string Configuration { get; }

        /// <summary>
        /// Тип документа текущего запроса.
        /// </summary>
        string DocumentType { get; }

        /// <summary>
        /// Данные для обработки.
        /// </summary>
        dynamic Item { get; }

        /// <summary>
        /// Признак успешности обработки.
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        /// Результат обработки.
        /// </summary>
        dynamic Result { get; set; }

        /// <summary>
        /// Результат фильтрации событий.
        /// </summary>
        dynamic ValidationMessage { get; set; }
    }
}