using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Json.EventBuilders;

namespace InfinniPlatform.Factories
{
	public class AggregateProvider : IAggregateProvider
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
