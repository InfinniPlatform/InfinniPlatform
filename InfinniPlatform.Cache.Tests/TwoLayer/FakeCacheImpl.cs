using System.Collections.Generic;

namespace InfinniPlatform.Cache.TwoLayer
{
    internal sealed class FakeCacheImpl : IInMemoryCache, ISharedCache
    {
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public bool Contains(string key)
        {
            return _data.ContainsKey(key);
        }

        public string Get(string key)
        {
            string value;
            if (_data.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }

        public bool TryGet(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        public void Set(string key, string value)
        {
            _data[key] = value;
        }

        public bool Remove(string key)
        {
            return _data.Remove(key);
        }
    }
}