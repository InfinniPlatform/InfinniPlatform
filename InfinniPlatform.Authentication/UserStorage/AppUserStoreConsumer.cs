using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша пользователей.
    /// </summary>
    [QueueName(nameof(AppUserStoreCache))]
    internal class AppUserStoreConsumer : BroadcastConsumerBase<string>
    {
        public AppUserStoreConsumer(Lazy<AppUserStoreCache> userCache)
        {
            _userCache = userCache;
        }

        private readonly Lazy<AppUserStoreCache> _userCache;

        protected override Task Consume(Message<string> message)
        {
            _userCache.Value.ProcessMessage(message);

            return Task.FromResult<object>(null);
        }
    }
}