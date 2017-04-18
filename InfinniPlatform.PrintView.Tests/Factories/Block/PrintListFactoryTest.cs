using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintListFactoryTest
    {
        [Test]
        public void ShouldBuildItems()
        {
            // Given

            var run1 = new PrintRun { Text = "Item1" };
            var item1 = new PrintParagraph();
            item1.Inlines.Add(run1);

            var run2 = new PrintRun { Text = "Item2" };
            var item2 = new PrintParagraph { Inlines = new List<PrintInline> { run2 } };
            item2.Inlines.Add(run2);

            var template = new PrintList();
            template.Items.Add(item1);
            template.Items.Add(item2);

            // When
            var element = BuildTestHelper.BuildElement<PrintList>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Items);
            Assert.AreEqual(2, element.Items.Count);
            Assert.AreEqual(run1.Text, ((PrintRun)((PrintParagraph)((PrintSection)element.Items[0]).Blocks[0]).Inlines[0]).Text);
            Assert.AreEqual(run2.Text, ((PrintRun)((PrintParagraph)((PrintSection)element.Items[1]).Blocks[0]).Inlines[0]).Text);
        }

        [Test]
        public void ShouldBuildItemsFromSource()
        {
            // Given

            var dataSource = new[] { "Item1", "Item2" };

            var run = new PrintRun();

            var itemTemplate = new PrintParagraph();
            itemTemplate.Inlines.Add(run);

            var template = new PrintList { Source = "$", ItemTemplate = itemTemplate };

            // When
            var element = BuildTestHelper.BuildElement<PrintList>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Items);
            Assert.AreEqual(2, element.Items.Count);
            Assert.AreEqual(dataSource[0], ((PrintRun)((PrintParagraph)((PrintSection)element.Items[0]).Blocks[0]).Inlines[0]).Text);
            Assert.AreEqual(dataSource[1], ((PrintRun)((PrintParagraph)((PrintSection)element.Items[1]).Blocks[0]).Inlines[0]).Text);
        }

        [Test]
        public void ShouldApplyStartIndex()
        {
            // Given
            var template = new PrintList { StartIndex = 5 };

            // When
            var element = BuildTestHelper.BuildElement<PrintList>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.StartIndex, element.StartIndex);
        }

        [Test]
        public void ShouldApplyMarkerStyle()
        {
            // Given
            var template = new PrintList { MarkerStyle = PrintListMarkerStyle.Decimal };

            // When
            var element = BuildTestHelper.BuildElement<PrintList>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.MarkerStyle, element.MarkerStyle);
        }

        [Test]
        public void ShouldApplyMarkerOffsetSize()
        {
            // Given
            var template = new PrintList { MarkerOffsetSize = 10, MarkerOffsetSizeUnit = PrintSizeUnit.Mm };

            // When
            var element = BuildTestHelper.BuildElement<PrintList>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.MarkerOffsetSize, element.MarkerOffsetSize);
            Assert.AreEqual(template.MarkerOffsetSizeUnit, element.MarkerOffsetSizeUnit);
        }
    }
}