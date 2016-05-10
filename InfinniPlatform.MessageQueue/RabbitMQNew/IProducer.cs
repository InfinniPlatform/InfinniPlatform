using System;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public interface IProducer : IDisposable
    {
        void Publish(string message);
    }
}