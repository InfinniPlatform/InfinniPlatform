using System;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    /// <summary>
    /// ѕолучаетель сообщений из очереди по запросу.
    /// </summary>
    public interface IBasicConsumer : IDisposable
    {
        /// <summary>
        /// ѕолучает сообщение из очереди.
        /// </summary>
        /// <returns>ѕервое сообщение в очереди или null, если сообщений нет.</returns>
        string Get();
    }
}