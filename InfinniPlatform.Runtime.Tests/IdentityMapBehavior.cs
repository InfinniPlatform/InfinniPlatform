using System;

using InfinniPlatform.Core.Runtime;

using NUnit.Framework;

namespace InfinniPlatform.Runtime.Tests
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class IdentityMapBehavior
	{
		[Test]
		public void ShouldAddToIdentityMap()
		{
			var identityMap = new IdentityMap();
			var identityObject = new object();
			
			identityMap.RegisterIdentity("123",identityObject);
			
			Assert.AreEqual(identityMap.GetInstance("123"),identityObject);
		}

		[Test]
		public void ShouldFailInsertIdentityMapExistingIdentity()
		{
			var identityMap = new IdentityMap();
			var identityObject = new object();

			identityMap.RegisterIdentity("123", identityObject);
			Assert.Throws<ArgumentException>(() => identityMap.RegisterIdentity("123", identityObject));

		}

		[Test]
		public void ShouldNotGetNotExistingIdentity()
		{
			var identityMap = new IdentityMap();

			Assert.IsNull(identityMap.GetInstance("123"));
			
		}

	}
}
