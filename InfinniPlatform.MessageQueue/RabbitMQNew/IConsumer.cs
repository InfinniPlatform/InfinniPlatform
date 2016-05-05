using System;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public interface IQueningConsumer : IDisposable
    {
        string Consume();
    }
}