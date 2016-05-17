using System;

namespace InfinniPlatform.Sdk.Queues
{
    public interface IConsumer
    {
        /// <summary>
        /// Тип тела сообщения.
        /// </summary>
        Type MessageType { get; }

        /// <summary>
        /// Обработчик сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void Consume(IMessage message);
    }


    public interface IDirectConsumer : IConsumer
    {
    }


    public interface IFanoutConsumer : IConsumer
    {
    }
}