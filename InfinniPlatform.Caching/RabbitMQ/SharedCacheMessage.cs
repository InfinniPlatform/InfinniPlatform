using System;

namespace InfinniPlatform.Caching.RabbitMQ
{
    /// <summary>
    /// Сообщение кэша пользователей.
    /// </summary>
    public class SharedCacheMessage
    {
        public SharedCacheMessage(string key, string value, Guid publisherId)
        {
            PublisherId = publisherId;
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Идентификатор отправителя сообщения.
        /// </summary>
        public Guid PublisherId { get; set; }

        /// <summary>
        /// Ключ.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; set; }
    }
}