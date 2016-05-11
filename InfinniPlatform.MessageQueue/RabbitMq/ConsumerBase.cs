using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    public abstract class ConsumerBase<T> : IConsumer where T : class
    {
        protected ConsumerBase(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; }

        public Type MessageType => typeof(T);

        void IConsumer.Consume(IMessage message)
        {
            Consume((Message<T>)message);
        }

        protected abstract void Consume(Message<T> message);
    }
}