using System;

using InfinniPlatform.Caching.Session;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Caching.Factory
{
    internal sealed class SessionManagerFactory : ISessionManagerFactory
    {
        public SessionManagerFactory(IAppConfiguration appConfiguration, ICacheFactory cacheFactory)
        {
            var cacheSettings = appConfiguration.GetSection<CacheSettings>(CacheSettings.SectionName);

            ICache cache;

            if (string.Equals(cacheSettings.Type, "Redis", StringComparison.OrdinalIgnoreCase))
            {
                cache = cacheFactory.GetSharedCache();
            }
            else if (string.Equals(cacheSettings.Type, "TwoLayer", StringComparison.OrdinalIgnoreCase))
            {
                cache = cacheFactory.GetTwoLayerCache();
            }
            else
            {
                cache = cacheFactory.GetMemoryCache();
            }

            _sessionManager = new SessionManager(cache);
        }


        private readonly ISessionManager _sessionManager;


        public ISessionManager CreateSessionManager()
        {
            return _sessionManager;
        }
    }
}