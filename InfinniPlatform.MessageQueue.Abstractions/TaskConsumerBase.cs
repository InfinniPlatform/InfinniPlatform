using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Базовый потребитель сообщений очереди задач.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public abstract class TaskConsumerBase<T> : ITaskConsumer
    {
        /// <inheritdoc />
        public Type MessageType => typeof(T);

        async Task IConsumer.Consume(IMessage message)
        {
            await Consume((Message<T>)message);
        }

        async Task<bool> IConsumer.OnError(Exception exception)
        {
            return await OnError(exception);
        }

        /// <summary>
        /// Message consume handler.
        /// </summary>
        /// <param name="message">Message from queue.</param>
        protected abstract Task Consume(Message<T> message);

        /// <summary>
        /// Handles message processing exceptions.
        /// </summary>
        /// <param name="exception">Exception.</param>
        protected virtual Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(true);
        }
    }
}