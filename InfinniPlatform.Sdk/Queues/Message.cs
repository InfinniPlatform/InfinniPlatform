using System;

namespace InfinniPlatform.Sdk.Queues
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
        public Message(T body, string appId = null)
        {
            Body = body;
            AppId = appId;
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

        public string AppId { get; }
    }
}