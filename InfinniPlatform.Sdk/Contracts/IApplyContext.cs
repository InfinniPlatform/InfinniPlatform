namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Контекст обработчика бизнес-логики проведения документа
    /// </summary>
    public interface IApplyContext : ICommonContext
    {
        /// <summary>
        /// Идентификатор обрабатываемого объекта
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Наименование индекса
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Объект, переводимый в состояние
        /// </summary>
        dynamic Item { get; set; }

        /// <summary>
        /// Результат, возвращаемый по окончании обработки
        /// </summary>
        dynamic Result { get; set; }

        /// <summary>
        /// Маркер транзакции используемой при обработке запроса
        /// </summary>
        string TransactionMarker { get; set; }
    }
}