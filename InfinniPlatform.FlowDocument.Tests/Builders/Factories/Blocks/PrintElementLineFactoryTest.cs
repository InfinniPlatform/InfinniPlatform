using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintElementLineFactoryTest
	{
		[Test]
		public void ShouldBuild()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();

			// When
			PrintElementBlock element = BuildTestHelper.BuildLine(elementMetadata);

			// Then
			Assert.IsNotNull(element);
            Assert.IsInstanceOf<PrintElementLine>(element);
		}
	}
}