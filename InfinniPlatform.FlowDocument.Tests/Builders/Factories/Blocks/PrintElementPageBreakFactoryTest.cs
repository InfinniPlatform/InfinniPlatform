using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
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
            Block element = BuildTestHelper.BuildPageBreak(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsTrue(element.BreakPageBefore);
        }
    }
}