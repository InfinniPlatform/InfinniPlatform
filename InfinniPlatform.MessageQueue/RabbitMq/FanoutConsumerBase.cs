using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public abstract class FanoutConsumerBase<T> : IFanoutConsumer where T : class
    {
        public Type MessageType => typeof(T);

        void IConsumer.Consume(IMessage message)
        {
            Consume((Message<T>)message);
        }

        protected abstract void Consume(Message<T> message);
    }
}