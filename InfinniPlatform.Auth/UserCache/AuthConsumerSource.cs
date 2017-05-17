using System.Collections.Generic;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.UserCache
{
    internal class AuthConsumerSource : IConsumerSource
    {
        public AuthConsumerSource(UserCacheConsumer userStoreCacheConsumer)
        {
            _userStoreCacheConsumer = userStoreCacheConsumer;
        }


        private readonly UserCacheConsumer _userStoreCacheConsumer;


        public IEnumerable<IConsumer> GetConsumers()
        {
            return new[] { _userStoreCacheConsumer };
        }
    }
}