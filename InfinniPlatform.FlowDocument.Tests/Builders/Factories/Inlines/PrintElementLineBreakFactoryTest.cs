using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.FlowDocument.Model.Inlines;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintElementLineBreakFactoryTest
	{
		[Test]
		public void ShouldBuildLineBreak()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();

			// When
			PrintElementLineBreak element = BuildTestHelper.BuildLineBreak(elementMetadata);

			// Then
			Assert.IsNotNull(element);
		}
	}
}