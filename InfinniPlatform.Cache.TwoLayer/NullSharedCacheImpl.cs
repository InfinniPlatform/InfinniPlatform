using InfinniPlatform.Cache.Abstractions;

namespace InfinniPlatform.Cache.TwoLayer
{
    /// <summary>
    /// Пустая реализация интерфейса для управления распределенным кэшем.
    /// </summary>
    public class NullSharedCacheImpl : ISharedCache
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
        }

        public bool Remove(string key)
        {
            return true;
        }
    }
}