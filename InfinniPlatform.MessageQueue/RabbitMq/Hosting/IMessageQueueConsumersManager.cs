using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    /// <summary>
    /// Предоставляет метод регистрации получателей сообщений из очереди.
    /// </summary>
    public interface IMessageQueueConsumersManager
    {
        /// <summary>
        /// Регистрирует обработчик.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="consumer">Экземпляр получателя.</param>
        void RegisterConsumer(string queueName, IConsumer consumer);
    }
}