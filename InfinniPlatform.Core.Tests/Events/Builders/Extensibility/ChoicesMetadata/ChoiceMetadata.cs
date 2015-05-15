using InfinniPlatform.Core.Tests.Events.Builders.Entities;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.ChoicesMetadata
{
	public class ChoiceMetadata
	{
		public string ChoiceIdentifier { get; set; }

		public bool IsMain { get; set; }

		public VisualTemplate VisualTemplate { get; set; }
	}
}