using System;
using InfinniPlatform.MessageQueue.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Serialization
{
    public interface IMessageSerializer
    {
        /// <summary>
        /// Преобразует сообщение в массив байтов для передачи в шину сообщений.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        byte[] MessageToBytes(object message);

        /// <summary>
        /// Преобразует массив байтов из шины сообщения в строготипизированный объект.
        /// </summary>
        /// <param name="args">Сообщения из шины.</param>
        /// <param name="type">Тип тела сообщения.</param>
        IMessage BytesToMessage(BasicDeliverEventArgs args, Type type);

        /// <summary>
        /// Преобразует массив байтов из шины сообщения в строготипизированный объект.
        /// </summary>
        /// <typeparam name="T">Тип тела сообщения.</typeparam>
        /// <param name="args">Сообщения из шины.</param>
        IMessage BytesToMessage<T>(BasicDeliverEventArgs args);

        /// <summary>
        /// Преобразует массив байтов из шины сообщения в строготипизированный объект.
        /// </summary>
        /// <typeparam name="T">Тип тела сообщения.</typeparam>
        /// <param name="args">Сообщения из шины.</param>
        IMessage BytesToMessage<T>(BasicGetResult args);
    }
}