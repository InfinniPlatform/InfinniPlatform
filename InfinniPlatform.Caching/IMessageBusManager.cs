using System;

namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Интерфейс управления подписками шины сообщений.
    /// </summary>
    public interface IMessageBusManager
    {
        /// <summary>
        /// Создает подписку.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="handler">Обработчик.</param>
        IDisposable Subscribe(string key, Action<string, string> handler);
    }
}