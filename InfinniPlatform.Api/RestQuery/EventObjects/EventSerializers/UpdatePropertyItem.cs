using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers
{
	public sealed class UpdatePropertyItem : IObjectToEventSerializer
	{
		private readonly string _path;
		private readonly string _value;
		private readonly EventType _eventType;

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
					       new EventDefinition()
						       {
							       Action = _eventType,
								   Property = _path,
								   Value =  _value
						       }
				       };
		}
	}
}
