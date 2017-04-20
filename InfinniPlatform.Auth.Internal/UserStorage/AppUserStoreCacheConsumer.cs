using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.UserStorage
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

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}