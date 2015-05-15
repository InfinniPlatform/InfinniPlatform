using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.RestMetadata
{
	public class RestTypeDefinition
	{

		public string RestDeclaringType { get; set; }

		public List<RestMethodDefinition> RestMethodsList { get; set; }

	}
}