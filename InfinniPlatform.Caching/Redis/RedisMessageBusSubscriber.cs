using System;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Подписчик шины сообщений Redis.
    /// </summary>
    internal sealed class RedisMessageBusSubscriber : IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="handleAction">Обработчик сообщения.</param>
        /// <param name="unsubscribeAction">Функция отписки от сообщений.</param>
        public RedisMessageBusSubscriber(Action<string, string> handleAction, Action unsubscribeAction)
        {
            _handleAction = handleAction;
            _unsubscribeAction = unsubscribeAction;
        }


        private readonly Action<string, string> _handleAction;
        private readonly Action _unsubscribeAction;


        /// <summary>
        /// Обрабатывает сообщение.
        /// </summary>
        public void Handle(string key, string value)
        {
            _handleAction(key, value);
        }


        public void Dispose()
        {
            _unsubscribeAction();
        }
    }
}