using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Получатель сообщений из очереди по запросу.
    /// </summary>
    public interface IOnDemandConsumer
    {
        /// <summary>
        /// Получает сообщение из очереди.
        /// </summary>
        /// <typeparam name="T">Тип тела сообщения.</typeparam>
        /// <param name="queueName">Имя очереди.</param>
        /// <returns>Первое сообщение в очереди или null, если сообщений нет.</returns>
        Task<IMessage> Consume<T>(string queueName = null);
    }
}