using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Caching;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// Источник потребителей сообщений сборки InfinniPlatform.Authentication.
    /// </summary>
    public class AuthenticationMessageConsumerSource : IMessageConsumerSource
    {
        public AuthenticationMessageConsumerSource(CacheSettings cacheSettings,
                                                   AppUserStoreCacheConsumer userStoreCacheConsumer)
        {
            _cacheSettings = cacheSettings;
            _userStoreCacheConsumer = userStoreCacheConsumer;
        }

        private readonly CacheSettings _cacheSettings;
        private readonly AppUserStoreCacheConsumer _userStoreCacheConsumer;

        public IEnumerable<IConsumer> GetConsumers()
        {
            return _cacheSettings.Type == CacheSettings.SharedCacheKey
                       ? new[] { _userStoreCacheConsumer }
                       : Enumerable.Empty<IConsumer>();
        }
    }
}