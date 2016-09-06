using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client.Framing;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// Предоставляет методы для работы с базовыми свойствами сообщений в очереди.
    /// </summary>
    public interface IBasicPropertiesProvider
    {
        /// <summary>
        /// Заполняет базовые свойства сообщения служебной информацией.
        /// </summary>
        BasicProperties Create();

        /// <summary>
        /// Возвращает с десериализованные заголовки сообщения.
        /// </summary>
        Dictionary<string, Func<string>> GetHeaders(IMessage message);
    }
}