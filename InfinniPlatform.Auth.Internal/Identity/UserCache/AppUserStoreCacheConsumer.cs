using System;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserStorage;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.Identity.UserCache
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша пользователей.
    /// </summary>
    [QueueName(nameof(UserCache))]
    internal class AppUserStoreCacheConsumer : BroadcastConsumerBase<string>
    {
        public AppUserStoreCacheConsumer(Lazy<IUserCacheObserver> userCacheSynchronizer)
        {
            _userCacheSynchronizer = userCacheSynchronizer;
        }

        private readonly Lazy<IUserCacheObserver> _userCacheSynchronizer;

        protected override async Task Consume(Message<string> message)
        {
            await _userCacheSynchronizer.Value.ProcessMessage(message);
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}