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
        string Configuration { get; set; }

        /// <summary>
        /// Тип документа текущего запроса.
        /// </summary>
        string Metadata { get; set; }

        /// <summary>
        /// Данные для обработки.
        /// </summary>
        dynamic Item { get; set; }

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