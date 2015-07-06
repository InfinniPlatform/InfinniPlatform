using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для сохранения событий из контекста
    /// </summary>
    public sealed class EventStorageComponent : IEventStorageComponent
    {
        private readonly IEventStorage _eventStorage;

        public EventStorageComponent(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public IEventStorage GetEventStorage()
        {
            return _eventStorage;
        }
    }
}