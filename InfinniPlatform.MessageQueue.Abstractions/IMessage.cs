using System;
using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Сообщение в очереди.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Идентификатор отправителя сообщения.
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// Заголовки сообщения.
        /// </summary>
        IDictionary<string, object> Headers { get; }

        /// <summary>
        /// Возвращает тело сообщения.
        /// </summary>
        object GetBody();

        /// <summary>
        /// Возвращает тип тела сообщения.
        /// </summary>
        Type GetBodyType();
    }
}