using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    public sealed class UpdateCollectionItem : IObjectToEventSerializer
    {
		private readonly string _collectionName;
        private readonly int _indexToUpdate;
        private readonly object _objectToUpdate;

        public UpdateCollectionItem(string collectionName, int indexToUpdate, object objectToUpdate)
        {
            _collectionName = collectionName;
            _indexToUpdate = indexToUpdate;
            _objectToUpdate = objectToUpdate;
        }

        public IEnumerable<EventDefinition> GetEvents()
        {
            return _objectToUpdate.ToEventListCollectionItem(_collectionName, _indexToUpdate).GetEvents(true).ToList();
        }
    }
}
