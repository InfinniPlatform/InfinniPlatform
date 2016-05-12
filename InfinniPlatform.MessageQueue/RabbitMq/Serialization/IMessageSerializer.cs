using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq.Serialization
{
    public interface IMessageSerializer
    {
        /// <summary>
        /// Преобразует сообщение в массив байтов для передачи в шину сообщений.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        byte[] MessageToBytes(IMessage message);

        /// <summary>
        /// Преобразует массив байтов из шины сообщения в строготипизированный объект.
        /// </summary>
        /// <param name="bytes">Массив байтов из шины сообщений.</param>
        /// <param name="type">Тип тела сообщения.</param>
        /// <returns></returns>
        IMessage BytesToMessage(byte[] bytes, Type type);
    }
}