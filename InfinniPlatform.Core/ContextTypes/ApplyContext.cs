using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes
{
    /// <summary>
    /// Контекст применения событий к объекту (бизнес-логика)
    /// </summary>
    public sealed class ApplyContext : CommonContext, IApplyContext
    {
        /// <summary>
        /// Идентификатор обрабатываемого объекта
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование индекса
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Объект, к которому приме
        /// </summary>
        public dynamic Item { get; set; }

        /// <summary>
        /// Маркер транзакции используемой при обработке запроса
        /// </summary>
        public string TransactionMarker { get; set; }
    }
}