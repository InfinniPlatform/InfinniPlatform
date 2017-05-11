namespace InfinniPlatform.Cache
{
    internal class InMemoryCacheFactory : IInMemoryCacheFactory
    {
        public InMemoryCacheFactory(IInMemoryCache inMemoryCache = null)
        {
            _inMemoryCache = inMemoryCache ?? new InMemoryCacheStub();
        }


        private readonly IInMemoryCache _inMemoryCache;


        public IInMemoryCache Create()
        {
            return _inMemoryCache;
        }


        private class InMemoryCacheStub : IInMemoryCache
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