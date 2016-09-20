using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inlines
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