using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Queues.Consumers
{
    /// <summary>
    /// Базовый потребитель сообщений очереди задач.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public abstract class TaskConsumerBase<T> : ITaskConsumer
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