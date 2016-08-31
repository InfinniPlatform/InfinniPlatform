using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Queues.Consumers
{
    public abstract class BroadcastConsumerBase<T> : IBroadcastConsumer where T : class
    {
        public Type MessageType => typeof(T);

        async Task IConsumer.Consume(IMessage message)
        {
            await Consume((Message<T>)message);
        }

        async Task<bool> IConsumer.OnError(Exception exception)
        {
            return await OnError();
        }

        protected abstract Task Consume(Message<T> message);

        protected abstract Task<bool> OnError();
    }
}