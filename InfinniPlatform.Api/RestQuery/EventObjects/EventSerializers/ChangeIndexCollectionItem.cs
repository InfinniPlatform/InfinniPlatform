using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    public sealed class ChangeIndexCollectionItem : IObjectToEventSerializer
    {
		private readonly string _collectionName;
        private readonly int _indexFrom;
        private readonly int _indexTo;

        public ChangeIndexCollectionItem(string collectionName, int indexFrom, int indexTo)
        {
            _collectionName = collectionName;
            _indexFrom = indexFrom;
            _indexTo = indexTo;
        }

        public IEnumerable<EventDefinition> GetEvents()
        {
            var eventDefinition = new EventDefinition
            {
                Action = EventType.ChangeCollectionItemIndex,
                Property = _collectionName,
                Value = string.Format("{0}:{1}", _indexFrom,_indexTo)
            };
            return new List<EventDefinition>() { eventDefinition };
        }
    }
}
