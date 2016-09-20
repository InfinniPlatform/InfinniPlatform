using System.Linq;

using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Model.Views;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Views
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintViewFactoryTest
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
            PrintViewDocument element = BuildTestHelper.BuildPrintView(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Blocks);
            Assert.AreEqual(2, element.Blocks.Count);
            Assert.IsInstanceOf<PrintElementParagraph>(element.Blocks.First());
            Assert.IsInstanceOf<PrintElementParagraph>(element.Blocks.Last());
        }

        [Test]
        public void ShouldBuildWithDefaultPageSize()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            PrintViewDocument element = BuildTestHelper.BuildPrintView(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize.Width);
            Assert.AreEqual(21.0*SizeUnits.Cm, element.PageSize.Width);
            Assert.AreEqual(29.7*SizeUnits.Cm, element.PageSize.Height);
        }

        [Test]
        public void ShouldApplyPageSize()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.PageSize = new DynamicWrapper();
            elementMetadata.PageSize.Width = 100;
            elementMetadata.PageSize.Height = 200;
            elementMetadata.PageSize.SizeUnit = "Px";

            // When
            PrintViewDocument element = BuildTestHelper.BuildPrintView(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize.Width);
            Assert.AreEqual(100, element.PageSize.Width);
            Assert.AreEqual(200, element.PageSize.Height);
        }

        [Test]
        public void ShouldBuildWithDefaultPagePadding()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            PrintViewDocument element = BuildTestHelper.BuildPrintView(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize.Width);
            Assert.AreEqual(1*SizeUnits.Cm, element.PagePadding.Left);
            Assert.AreEqual(1*SizeUnits.Cm, element.PagePadding.Top);
            Assert.AreEqual(1*SizeUnits.Cm, element.PagePadding.Right);
            Assert.AreEqual(1*SizeUnits.Cm, element.PagePadding.Bottom);
        }

        [Test]
        public void ShouldApplyPagePadding()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.PagePadding = new DynamicWrapper();
            elementMetadata.PagePadding.Left = 10;
            elementMetadata.PagePadding.Top = 20;
            elementMetadata.PagePadding.Right = 30;
            elementMetadata.PagePadding.Bottom = 40;
            elementMetadata.PagePadding.SizeUnit = "Px";

            // When
            PrintViewDocument element = BuildTestHelper.BuildPrintView(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.PageSize.Width);
            Assert.AreEqual(10, element.PagePadding.Left);
            Assert.AreEqual(20, element.PagePadding.Top);
            Assert.AreEqual(30, element.PagePadding.Right);
            Assert.AreEqual(40, element.PagePadding.Bottom);
        }
    }
}