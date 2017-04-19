using System.Collections.Generic;

using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.Auth.UserStorage
{
    internal class AuthInternalMessageConsumerSource : IMessageConsumerSource
    {
        public AuthInternalMessageConsumerSource(AppUserStoreCacheConsumer userStoreCacheConsumer)
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