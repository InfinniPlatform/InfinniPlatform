namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Общий контекст выполнения всех точек расширения
    /// </summary>
    public interface ICommonContext
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
        /// Тип действия текущего запроса.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Признак успешности обработки.
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        /// Признак системной ошибки сервера.
        /// </summary>
        bool IsInternalServerError { get; set; }

        /// <summary>
        /// Результат фильтрации событий.
        /// </summary>
        dynamic ValidationMessage { get; set; }

        /// <summary>
        /// Результат обработки.
        /// </summary>
        dynamic Result { get; set; }
    }
}