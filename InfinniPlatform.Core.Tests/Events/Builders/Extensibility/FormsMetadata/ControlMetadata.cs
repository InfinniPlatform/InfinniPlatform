using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.FormsMetadata
{
	public class ControlMetadata
	{
		public string ControlIdentifier { get; set; }

		public string ControlType { get; set; }

		public object Metadata { get; set; }			

		public string LayoutName { get; set; }

		public IEnumerable<ControlMetadata> InnerControls { get; set; } 
	}
}