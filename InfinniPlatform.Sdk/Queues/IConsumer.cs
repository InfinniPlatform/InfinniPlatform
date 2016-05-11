using System;

namespace InfinniPlatform.Sdk.Queues
{
    public interface IConsumer
    {
        string QueueName { get; }

        Type MessageType { get; }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        /// <param name="message">���������.</param>
        void Consume(IMessage message);
    }
}