using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
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
            Block element = BuildTestHelper.BuildLine(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.BorderBrush);
            Assert.IsNotNull(element.BorderThickness);
            Assert.IsTrue(element.BorderThickness.Top > 0 || element.BorderThickness.Bottom > 0);
        }
    }
}