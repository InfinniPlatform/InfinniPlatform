using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    /// <summary>
    /// Предоставляет абстракцию для ограничения количества одновреммено выполняемых операций.
    /// </summary>
    public interface IMessageQueueThreadPool
    {
        /// <summary>
        /// Добавляет операцию в очередь.
        /// </summary>
        /// <param name="func">Операция.</param>
        Task Enqueue(Func<Task> func);
    }
}