namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент для клиентской нотификации
    /// </summary>
    public interface IWebClientNotificationComponent
    {
        void Notify(string routingKey, object message);
    }
}