using System;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Базовый потребитель сообщений широковещательной очереди.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public abstract class BroadcastConsumerBase<T> : IBroadcastConsumer where T : class
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