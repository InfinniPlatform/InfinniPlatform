namespace InfinniPlatform.Sdk.Application.Contracts
{
    /// <summary>
    ///   Контекст применения результата 
    /// </summary>
    public interface IApplyResultContext : ICommonContext
    {
        /// <summary>
        ///   Объект, к которому применены изменения
        /// </summary>
        dynamic Item { get; set; }

        /// <summary>
        ///  Статус обработки документа 
        /// </summary>
        object Status { get; set; }

        /// <summary>
        ///  Результат бизнес-обработки документа
        /// </summary>
        dynamic Result { get; set; }

    }
}
