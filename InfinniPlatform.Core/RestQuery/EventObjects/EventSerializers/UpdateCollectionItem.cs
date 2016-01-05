using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.RestQuery.EventObjects.EventSerializers
{
    public sealed class UpdateCollectionItem : IObjectToEventSerializer
    {
        private readonly string _collectionName;
        private readonly int _indexToUpdate;
        private readonly object _objectToUpdate;
        private readonly string _version;

        public UpdateCollectionItem(string collectionName, int indexToUpdate, object objectToUpdate, string version)
        {
            _collectionName = collectionName;
            _indexToUpdate = indexToUpdate;
            _objectToUpdate = objectToUpdate;
            _version = version;
        }

        public IEnumerable<EventDefinition> GetEvents()
        {
            return
                _objectToUpdate.ToEventListCollectionItem(_collectionName, _indexToUpdate)
                    .GetEvents(true)
                    .ToList()
                    .AddVersionDefinition(_version);
        }
    }
}