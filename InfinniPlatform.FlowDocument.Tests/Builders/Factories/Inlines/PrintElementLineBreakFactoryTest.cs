using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
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
            LineBreak element = BuildTestHelper.BuildLineBreak(elementMetadata);

            // Then
            Assert.IsNotNull(element);
        }
    }
}