using System.Collections.Generic;

using InfinniPlatform.Core.Json.EventBuilders;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.Json
{
    public class ObjectRenderingHandler
    {
        public object RenderEvents(object aggregate, IEnumerable<EventDefinition> eventList)
        {
            return new BackboneBuilderJson().ConstructJsonObject(aggregate, eventList);
        }
    }
}