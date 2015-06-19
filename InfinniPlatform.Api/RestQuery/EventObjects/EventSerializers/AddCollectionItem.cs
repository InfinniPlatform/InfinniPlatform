using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    /// <summary>
    ///     Контейнер для добавления элемента коллекции
    /// </summary>
    public sealed class AddCollectionItem : IObjectToEventSerializer
    {
        private readonly string _collectionName;
        private readonly object _objectToCreate;
        private readonly string _version;

        public AddCollectionItem(string collectionName, object objectToCreate, string version)
        {
            _collectionName = collectionName;
            _objectToCreate = objectToCreate;
            _version = version;
        }

        /// <summary>
        ///     Получить список событий по созданию указанного в аргументах объекта
        /// </summary>
        /// <returns>Список событий изменения/создания объекта</returns>
        public IEnumerable<EventDefinition> GetEvents()
        {
            return
                _objectToCreate.ToEventListCollectionItem(_collectionName)
                    .GetEvents()
                    .ToList()
                    .AddVersionDefinition(_version);
        }
    }
}