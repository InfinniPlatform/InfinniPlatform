using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.RestMetadata
{
	public class RestMethodDefinition
	{
		public string RestMethodName { get; set; }

		public IEnumerable<string> RestMethodArguments { get; set; }

		public IEnumerable<string> RestMethodArgumentTypes { get; set; }

		public string RestMethodReturnType { get; set; }
	}
}