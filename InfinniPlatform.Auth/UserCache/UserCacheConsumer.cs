using System;
using System.Threading.Tasks;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.UserCache
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша пользователей.
    /// </summary>
    [QueueName(nameof(UserCache))]
    internal class UserCacheConsumer : BroadcastConsumerBase<string>
    {
        public UserCacheConsumer(Lazy<IUserCacheObserver> userCacheSynchronizer)
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