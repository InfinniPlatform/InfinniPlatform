using System;

namespace InfinniPlatform.Caching.RabbitMQ
{
    public interface IRabbitBus
    {
        /// <summary>
        /// Обработчик события получения сообщения.
        /// </summary>
        Action<SharedCacheMessage> OnMessageRecieve { get; set; }

        /// <summary>
        /// Публикация события.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        void Publish(string key, string value);
    }
}