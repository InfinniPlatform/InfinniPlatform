using System.Diagnostics;

namespace InfinniPlatform.Caching.RabbitMQ
{
    /// <summary>
    /// Сообщение кэша пользователей.
    /// </summary>
    [DebuggerDisplay("{Key}")]
    public class SharedCacheMessage
    {
        public SharedCacheMessage(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Ключ.
        /// </summary>
        public string Key { get; set; }
    }
}