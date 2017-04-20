using System;
using System.Collections.Generic;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Сообщение в очереди.
    /// </summary>
    public class Message<T> : IMessage<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="appId">Идентификатор отправителя сообщения.</param>
        /// <param name="headers">Заголовки сообщения.</param>
        public Message(T body, string appId = null, IDictionary<string, object> headers = null)
        {
            Body = body;
            AppId = appId;
            Headers = headers;
        }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public T Body { get; }

        /// <summary>
        /// Возвращает тело сообщения.
        /// </summary>
        public object GetBody()
        {
            return Body;
        }

        /// <summary>
        /// Возвращает тип тела сообщения.
        /// </summary>
        public Type GetBodyType()
        {
            return Body.GetType();
        }

        /// <summary>
        /// Идентификатор приложения-отправителя сообщения.
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// Заголовки сообщения.
        /// </summary>
        public IDictionary<string, object> Headers { get; }
    }
}