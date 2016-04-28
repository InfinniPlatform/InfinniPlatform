namespace InfinniPlatform.Sdk.Queues
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