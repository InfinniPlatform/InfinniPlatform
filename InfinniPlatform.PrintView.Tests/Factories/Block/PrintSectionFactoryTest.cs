using System.Linq;

using InfinniPlatform.PrintView.Block;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintSectionFactoryTest
    {
        [Test]
        public void ShouldBuildBlocks()
        {
            // Given

            var block1 = new PrintParagraph();
            var block2 = new PrintParagraph();

            var template = new PrintSection();
            template.Blocks.Add(block1);
            template.Blocks.Add(block2);

            // When
            var element = BuildTestHelper.BuildElement<PrintSection>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Blocks);
            Assert.AreEqual(2, element.Blocks.Count);
            Assert.IsInstanceOf<PrintParagraph>(element.Blocks.First());
            Assert.IsInstanceOf<PrintParagraph>(element.Blocks.Last());
        }
    }
}