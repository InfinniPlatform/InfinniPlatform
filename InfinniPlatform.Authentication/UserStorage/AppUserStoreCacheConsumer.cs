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
    internal class AppUserStoreCacheConsumer : BroadcastConsumerBase<string>
    {
        public AppUserStoreCacheConsumer(Lazy<IUserCacheSynchronizer> userCacheSynchronizer)
        {
            _userCacheSynchronizer = userCacheSynchronizer;
        }

        private readonly Lazy<IUserCacheSynchronizer> _userCacheSynchronizer;

        protected override async Task Consume(Message<string> message)
        {
            await _userCacheSynchronizer.Value.ProcessMessage(message);
        }
    }
}