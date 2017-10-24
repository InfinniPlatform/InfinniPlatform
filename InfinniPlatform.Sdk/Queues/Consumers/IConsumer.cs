using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Queues.Consumers
{
    /// <summary>
    /// Потребитель сообщений.
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Тип тела сообщения.
        /// </summary>
        Type MessageType { get; }

        /// <summary>
        /// Обработчик сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        Task Consume(IMessage message);

        /// <summary>
        /// Обработчик ошибок.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <remarks>True - сообщение будет считаться обработанным и не вернется в очередь. 
        /// False - сообщение будет считаться необработанным и вернется в очередь (например, для обработки на другом узле), 
        /// однако в этом случае сообщение может "зависнуть" в очереди.</remarks>
        Task<bool> OnError(Exception exception);
    }
}