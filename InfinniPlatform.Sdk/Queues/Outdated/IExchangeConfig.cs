namespace InfinniPlatform.Sdk.Queues.Outdated
{
    /// <summary>
    ///     Интерфейс для настройки свойств точки обмена сообщениями.
    /// </summary>
    public interface IExchangeConfig
    {
        /// <summary>
        ///     Сохранять настройки точки обмена на диске.
        /// </summary>
        IExchangeConfig Durable();
    }
}