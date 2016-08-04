﻿using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// Интерфейс синхронизации кэша пользователей через очередь сообщений.
    /// </summary>
    public interface IUserCacheSynchronizer
    {
        /// <summary>
        /// Обрабатывает сообщение из очереди.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        Task ProcessMessage(Message<string> message);

        /// <summary>
        /// Оповестить получателей об изменении пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        void NotifyUserChanged(string userId);
    }
}