using System.Collections.Generic;
using InfinniPlatform.Json.EventBuilders;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Json
{
    public class ObjectRenderingHandler
    {
        public object RenderEvents(object aggregate, IEnumerable<EventDefinition> eventList)
        {
            return new BackboneBuilderJson().ConstructJsonObject(aggregate, eventList);
        }
    }
}