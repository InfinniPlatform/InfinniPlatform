using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.Contract.Consumers
{
    /// <summary>
    /// Базовый потребитель сообщений очереди задач.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public abstract class TaskConsumerBase<T> : ITaskConsumer
    {
        public Type MessageType => typeof(T);

        async Task IConsumer.Consume(IMessage message)
        {
            await Consume((Message<T>)message);
        }

        async Task<bool> IConsumer.OnError(Exception exception)
        {
            return await OnError(exception);
        }

        protected abstract Task Consume(Message<T> message);

        protected virtual Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(true);
        }
    }
}