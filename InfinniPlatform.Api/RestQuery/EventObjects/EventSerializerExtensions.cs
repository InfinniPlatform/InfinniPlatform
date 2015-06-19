using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects
{
    public static class EventSerializerExtensions
    {
        public static IEnumerable<EventDefinition> AddVersionDefinition(this IList<EventDefinition> eventDefinitions,
            string version)
        {
            eventDefinitions.Add(new EventDefinition
            {
                Property = "Version",
                Action = EventType.CreateProperty,
                Value = version
            });

            return eventDefinitions;
        }
    }
}