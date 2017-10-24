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

        async Task<bool> IConsumer.OnError(Exception exception)
        {
            return await OnError(exception);
        }

        /// <remarks>True - сообщение будет считаться обработанным и не вернется в очередь. 
        /// False - сообщение будет считаться необработанным и вернется в очередь (например, для обработки на другом узле), 
        /// однако в этом случае сообщение может "зависнуть" в очереди.</remarks>
        protected virtual Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(true);
        }
    }
}