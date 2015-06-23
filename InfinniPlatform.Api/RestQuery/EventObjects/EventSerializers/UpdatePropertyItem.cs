using System.Collections.Generic;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
    public sealed class UpdatePropertyItem : IObjectToEventSerializer
    {
        private readonly EventType _eventType;
        private readonly string _path;
        private readonly string _value;

        public UpdatePropertyItem(string path, string value, EventType eventType)
        {
            _path = path;
            _value = value;
            _eventType = eventType;
        }

        public IEnumerable<EventDefinition> GetEvents()
        {
            return new[]
            {
                new EventDefinition
                {
                    Action = _eventType,
                    Property = _path,
                    Value = _value
                }
            };
        }
    }
}