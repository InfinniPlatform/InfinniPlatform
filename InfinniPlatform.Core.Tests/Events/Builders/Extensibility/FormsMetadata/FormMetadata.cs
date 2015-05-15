using System.Collections.Generic;
using InfinniPlatform.Core.Tests.Events.Builders.Entities;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.ChoicesMetadata;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.LayoutMetadata;
using InfinniPlatform.Core.Tests.Events.Builders.Extensibility.RestMetadata;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.FormsMetadata
{
	public class FormMetadata
	{
		public string MetadataId { get; set; }

		public string MetadataName { get; set; }

		public ObjectMetadataRecord ObjectMetadata { get; set; }

		public RestContainer MainDataProvider { get; set; }		

		public IEnumerable<object> ControlsMetadata { get; set; }

		public LayoutScheme LayoutScheme { get; set; }

		public IEnumerable<RestContainer> DataProviders { get; set; }

		public IEnumerable<ChoiceMetadata> ChoiceMetadata { get; set; }
	}
}