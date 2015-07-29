using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintElementPageBreakFactoryTest
	{
		[Test]
		public void ShouldBuild()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();

			// When
			PrintElementBlock element = BuildTestHelper.BuildPageBreak(elementMetadata);

			// Then
			Assert.IsNotNull(element);
            Assert.IsInstanceOf<PrintElementPageBreak>(element);
		}
	}
}