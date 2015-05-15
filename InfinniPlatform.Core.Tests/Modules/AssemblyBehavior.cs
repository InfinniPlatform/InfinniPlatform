using System.Linq;

using InfinniPlatform.Modules;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Modules
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public class AssemblyBehavior
	{
		[Test]
		public void ShouldLoadAssemblies()
		{
			var modules = ModuleExtension.LoadModulesAssemblies("InfinniPlatform.Api, InfinniPlatform.Core, InfinniPlatform.Hosting, InfinniPlatform.Metadata, InfinniPlatform.Runtime").ToArray();

			Assert.True(modules.Select(m => m.GetName().Name).Contains("InfinniPlatform.Api"));
			Assert.True(modules.Select(m => m.GetName().Name).Contains("InfinniPlatform.Core"));
			Assert.True(modules.Select(m => m.GetName().Name).Contains("InfinniPlatform.Hosting"));
			Assert.True(modules.Select(m => m.GetName().Name).Contains("InfinniPlatform.Metadata"));
			Assert.True(modules.Select(m => m.GetName().Name).Contains("InfinniPlatform.Runtime"));
		}
	}
}