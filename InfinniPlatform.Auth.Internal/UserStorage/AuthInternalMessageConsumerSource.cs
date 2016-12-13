using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.Auth.Internal.UserStorage
{
    internal class AuthInternalMessageConsumerSource : IMessageConsumerSource
    {
        public AuthInternalMessageConsumerSource(CacheSettings cacheSettings, AppUserStoreCacheConsumer userStoreCacheConsumer)
        {
            _cacheSettings = cacheSettings;
            _userStoreCacheConsumer = userStoreCacheConsumer;
        }


        private readonly CacheSettings _cacheSettings;
        private readonly AppUserStoreCacheConsumer _userStoreCacheConsumer;


        public IEnumerable<IConsumer> GetConsumers()
        {
            return (_cacheSettings.Type == CacheSettings.SharedCacheKey)
                       ? new[] { _userStoreCacheConsumer }
                       : Enumerable.Empty<IConsumer>();
        }
    }
}