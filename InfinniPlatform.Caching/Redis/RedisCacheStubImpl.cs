using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс для управления распределенным кэшем на базе Redis.
    /// </summary>
    [LoggerName("Redis")]
    internal sealed class RedisCacheStubImpl : ISharedCache
    {
        public bool Contains(string key)
        {
            return false;
        }

        public string Get(string key)
        {
            return null;
        }

        public bool TryGet(string key, out string value)
        {
            value = null;

            return false;
        }

        public void Set(string key, string value)
        {
            // empty
        }

        public bool Remove(string key)
        {
            return true;
        }
    }
}