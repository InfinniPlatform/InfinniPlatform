namespace InfinniPlatform.Cache
{
    internal class SharedCacheFactory : ISharedCacheFactory
    {
        public SharedCacheFactory(ISharedCache sharedCache = null)
        {
            _sharedCache = sharedCache ?? new SharedCacheStub();
        }


        private readonly ISharedCache _sharedCache;


        public ISharedCache Create()
        {
            return _sharedCache;
        }


        private class SharedCacheStub : ISharedCache
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
}