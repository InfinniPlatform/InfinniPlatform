using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Сообщение в очереди.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Возвращает тело сообщения.
        /// </summary>
        object GetBody();

        /// <summary>
        /// Возвращает тип тела сообщения.
        /// </summary>
        Type GetBodyType();

        /// <summary>
        /// Идентификатор отправителя сообщения.
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// Заголовки сообщения.
        /// </summary>
        IDictionary<string, object> Headers { get; }
    }
}