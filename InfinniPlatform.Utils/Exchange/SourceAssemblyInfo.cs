using System.Reflection;

namespace InfinniPlatform.Utils.Exchange
{
	public sealed class SourceAssemblyInfo
	{
		public string Name { get; set; }

		public Assembly Assembly { get; set; }

		public string AssemblyFileName { get; set; }
	}
}