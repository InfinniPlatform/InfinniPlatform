using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Runtime.Implementation.AssemblyDispatch;

using NUnit.Framework;

namespace InfinniPlatform.Runtime.Tests.AssemblyDispatch
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class MethodInvocationCacheBehavior
	{
		private string _pathToCheckedDirectory;
		private string _assemblyLocation;

		[SetUp]
		public void SetupPaths()
		{

			//заполнение списка путей к существующим сборкам для тестирования
            _assemblyLocation = Path.Combine("InfinniPlatform.RestfulApi.exe");
		}

		[Test]
		public void ShouldNotFindInfo()
		{
			var methodInvokationCache = new MethodInvokationCache("versio1", DateTime.Now, new List<Assembly>());
			var info = methodInvokationCache.FindMethodInfo("ActionUnitCreateDocument", "Action");

			Assert.IsNull(info);
		}


		[Test]
		public void ShouldFindInfo()
		{
			var assembly = Assembly.LoadFrom(_assemblyLocation);

			var methodInvokationCache = new MethodInvokationCache("versio1", DateTime.Now, new List<Assembly> { assembly });
			var info = methodInvokationCache.FindMethodInfo("ActionUnitCreateDocument", "Action");

			Assert.IsNotNull(info);
		}
	}
}