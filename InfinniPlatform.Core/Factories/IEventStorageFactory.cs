using InfinniPlatform.Api.Factories;
using InfinniPlatform.EventStorage;

namespace InfinniPlatform.Factories
{
    /// <summary>
    ///     Фабрика для создания хранилища событий.
    /// </summary>
    public interface IEventStorageFactory
    {
        /// <summary>
        ///     Создать хранилище событий.
        /// </summary>
        IEventStorage CreateEventStorage();

        /// <summary>
        ///     Создать менеджер для управления хранилищем событий.
        /// </summary>
        IEventStorageManager CreateEventStorageManager();
    }
}