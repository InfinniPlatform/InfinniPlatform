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

        void IConsumer.Consume(IMessage message)
        {
            Consume((Message<T>)message);
        }

        public async Task ConsumeAsync(IMessage message)
        {
            await ConsumeAsync((Message<T>)message);
        }

        protected abstract void Consume(Message<T> message);

        protected abstract Task ConsumeAsync(Message<T> message);
    }
}