using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public abstract class BroadcastConsumerBase<T> : IBroadcastConsumer where T : class
    {
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