using System.Windows.Documents;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementSectionFactoryTest
    {
        [Test]
        public void ShouldBuildBlocks()
        {
            // Given

            dynamic block1 = new DynamicWrapper();
            block1.Paragraph = new DynamicWrapper();

            dynamic block2 = new DynamicWrapper();
            block2.Paragraph = new DynamicWrapper();

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Blocks = new[] {block1, block2};

            // When
            Section element = BuildTestHelper.BuildSection(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Blocks);
            Assert.AreEqual(2, element.Blocks.Count);
            Assert.IsInstanceOf<Paragraph>(element.Blocks.FirstBlock);
            Assert.IsInstanceOf<Paragraph>(element.Blocks.LastBlock);
        }
    }
}