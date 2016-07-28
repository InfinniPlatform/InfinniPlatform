using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
    internal sealed class FakeCacheImpl : IMemoryCache, ISharedCache, ICache
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

        public Task ProcessMessage(Message<string> message)
        {
            return Task.FromResult<object>(null);
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}