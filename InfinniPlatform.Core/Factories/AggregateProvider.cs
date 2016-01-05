using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Json.EventBuilders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.Factories
{
    public class AggregateProvider
    {
        public object CreateAggregate()
        {
            dynamic result = new DynamicWrapper();
            result.Id = Guid.NewGuid().ToString().ToLowerInvariant();
            return result;
        }

        public void ApplyChanges(ref object item, IEnumerable<EventDefinition> events)
        {
            if (item == null)
            {
                throw new ArgumentException("Need to set object to apply changes");
            }
            if (events == null)
            {
                throw new ArgumentException("Need to set event list to apply");
            }

            item = new BackboneBuilderJson().ConstructJsonObject(item, events);
        }
    }
}