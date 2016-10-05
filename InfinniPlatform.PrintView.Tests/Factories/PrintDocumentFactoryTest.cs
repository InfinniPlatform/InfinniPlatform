using System.Linq;

using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Defaults;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintDocumentFactoryTest
    {
        [Test]
        public void ShouldBuildBlocks()
        {
            // Given

            var block1 = new PrintParagraph();
            var block2 = new PrintParagraph();

            var template = new PrintDocument();
            template.Blocks.Add(block1);
            template.Blocks.Add(block2);

            // When
            var element = BuildTestHelper.BuildElement<PrintDocument>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Blocks);
            Assert.AreEqual(template.Blocks.Count, element.Blocks.Count);
            Assert.IsInstanceOf<PrintParagraph>(element.Blocks.First());
            Assert.IsInstanceOf<PrintParagraph>(element.Blocks.Last());
        }

        [Test]
        public void ShouldBuildWithDefaultPageSize()
        {
            // Given
            var template = new PrintDocument();

            // When
            var element = BuildTestHelper.BuildElement<PrintDocument>(template);

            // Then

            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.PageSizes.A4.Width, PrintViewDefaults.PageSizes.A4.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PageSize.Width, element.PageSize.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.PageSizes.A4.Height, PrintViewDefaults.PageSizes.A4.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PageSize.Height, element.PageSize.SizeUnit),
                            0.1);
        }

        [Test]
        public void ShouldApplyPageSize()
        {
            // Given
            var template = new PrintDocument { PageSize = new PrintSize(100, 200, PrintSizeUnit.Px) };

            // When
            var element = BuildTestHelper.BuildElement<PrintDocument>(template);

            // Then

            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PageSize.Width, template.PageSize.SizeUnit),
                PrintSizeUnitConverter.ToUnifiedSize(element.PageSize.Width, element.PageSize.SizeUnit),
                0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PageSize.Height, template.PageSize.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PageSize.Height, element.PageSize.SizeUnit),
                            0.1);
        }

        [Test]
        public void ShouldBuildWithDefaultPagePadding()
        {
            // Given
            var template = new PrintDocument();

            // When
            var element = BuildTestHelper.BuildElement<PrintDocument>(template);

            // Then

            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PagePadding);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.Document.PagePadding.Left, PrintViewDefaults.Document.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Left, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.Document.PagePadding.Top, PrintViewDefaults.Document.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Top, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.Document.PagePadding.Right, PrintViewDefaults.Document.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Right, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(PrintViewDefaults.Document.PagePadding.Bottom, PrintViewDefaults.Document.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Bottom, element.PagePadding.SizeUnit),
                            0.1);
        }

        [Test]
        public void ShouldApplyPagePadding()
        {
            // Given
            var template = new PrintDocument { PagePadding = new PrintThickness(10, 20, 30, 40, PrintSizeUnit.Px) };

            // When
            var element = BuildTestHelper.BuildElement<PrintDocument>(template);

            // Then

            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PagePadding);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PagePadding.Left, template.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Left, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PagePadding.Top, template.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Top, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PagePadding.Right, template.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Right, element.PagePadding.SizeUnit),
                            0.1);

            Assert.AreEqual(PrintSizeUnitConverter.ToUnifiedSize(template.PagePadding.Bottom, template.PagePadding.SizeUnit),
                            PrintSizeUnitConverter.ToUnifiedSize(element.PagePadding.Bottom, element.PagePadding.SizeUnit),
                            0.1);
        }
    }
}