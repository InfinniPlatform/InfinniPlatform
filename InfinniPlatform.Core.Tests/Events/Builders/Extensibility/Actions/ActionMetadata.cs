using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.Actions
{
	public class ActionMetadata
	{
		public string ActionId { get; set; }

		public string MethodType { get; set; }

		public string ControllerName { get; set; }

		public string ActionName { get; set; }

		public IEnumerable<string> Arguments { get; set; }

		public string ActionType { get; set; }
	}
}