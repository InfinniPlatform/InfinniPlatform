using System;

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
    }
}