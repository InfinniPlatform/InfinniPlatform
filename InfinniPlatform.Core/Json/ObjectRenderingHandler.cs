using System.Collections.Generic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Json.EventBuilders;

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