using System.Collections.Generic;

using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.RestQuery.EventObjects.EventSerializers
{
    /// <summary>
    ///     Контейнер для удаления элементов коллекции
    /// </summary>
    public sealed class RemoveCollectionItem : IObjectToEventSerializer
    {
        private readonly string _collectionItemPath;

        public RemoveCollectionItem(string collectionItemPath)
        {
            _collectionItemPath = collectionItemPath;
        }

        /// <summary>
        ///     Получить список событий по созданию указанного в аргументах объекта
        /// </summary>
        /// <returns>Список событий изменения/создания объекта</returns>
        public IEnumerable<EventDefinition> GetEvents()
        {
            var eventDefinition = new EventDefinition
            {
                Action = EventType.RemoveItemFromCollection,
                Property = _collectionItemPath
            };
            return new List<EventDefinition> {eventDefinition}.AddVersionDefinition(null);
        }
    }
}