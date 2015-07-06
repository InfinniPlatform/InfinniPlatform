using InfinniPlatform.EventStorage;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

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