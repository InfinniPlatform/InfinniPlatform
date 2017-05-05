using System.Collections.Generic;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.Identity.UserCache
{
    internal class AuthInternalConsumerSource : IConsumerSource
    {
        public AuthInternalConsumerSource(AppUserStoreCacheConsumer userStoreCacheConsumer)
        {
            _userStoreCacheConsumer = userStoreCacheConsumer;
        }


        private readonly AppUserStoreCacheConsumer _userStoreCacheConsumer;


        public IEnumerable<IConsumer> GetConsumers()
        {
            return new[] { _userStoreCacheConsumer };
        }
    }
}