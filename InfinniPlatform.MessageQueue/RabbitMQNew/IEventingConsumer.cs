using System;

using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public interface IEventingConsumer : IDisposable
    {
        void AddRecievedEvent(EventHandler<BasicDeliverEventArgs> eventingConsumerOnReceived);
    }
}