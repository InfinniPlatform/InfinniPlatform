using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// Базовый потребитель сообщений очереди задач.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public abstract class TaskConsumerBase<T> : ITaskConsumer where T : class
    {
        /// <summary>
        /// Тип тела сообщения.
        /// </summary>
        public Type MessageType => typeof(T);

        async Task IConsumer.Consume(IMessage message)
        {
            await Consume((Message<T>)message);
        }

        protected abstract Task Consume(Message<T> message);
    }
}