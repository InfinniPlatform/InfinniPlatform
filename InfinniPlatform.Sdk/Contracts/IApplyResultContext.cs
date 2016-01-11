namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Контекст применения результата
    /// </summary>
    public interface IApplyResultContext : ICommonContext
    {
        /// <summary>
        /// Объект, к которому применены изменения
        /// </summary>
        dynamic Item { get; set; }

        /// <summary>
        /// Результат бизнес-обработки документа
        /// </summary>
        dynamic Result { get; set; }
    }
}