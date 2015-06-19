using System.Linq;
using System.Windows;
using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;
using List = System.Windows.Documents.List;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementListFactoryTest
    {
        [Test]
        public void ShouldBuildItems()
        {
            // Given

            dynamic run1 = new DynamicWrapper();
            run1.Run = new DynamicWrapper();
            run1.Run.Text = "Item1";

            dynamic run2 = new DynamicWrapper();
            run2.Run = new DynamicWrapper();
            run2.Run.Text = "Item2";

            dynamic item1 = new DynamicWrapper();
            item1.Paragraph = new DynamicWrapper();
            item1.Paragraph.Inlines = new[] {run1};

            dynamic item2 = new DynamicWrapper();
            item2.Paragraph = new DynamicWrapper();
            item2.Paragraph.Inlines = new[] {run2};

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Items = new[] {item1, item2};

            // When
            List element = BuildTestHelper.BuildList(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.ListItems);
            Assert.AreEqual(2, element.ListItems.Count);
            Assert.AreEqual("Item1",
                ((Run) ((Paragraph) element.ListItems.ElementAt(0).Blocks.ElementAt(0)).Inlines.ElementAt(0)).Text);
            Assert.AreEqual("Item2",
                ((Run) ((Paragraph) element.ListItems.ElementAt(1).Blocks.ElementAt(0)).Inlines.ElementAt(0)).Text);
        }

        [Test]
        public void ShouldBuildItemsFromSource()
        {
            // Given

            dynamic run = new DynamicWrapper();
            run.Run = new DynamicWrapper();

            dynamic itemTemplate = new DynamicWrapper();
            itemTemplate.Paragraph = new DynamicWrapper();
            itemTemplate.Paragraph.Inlines = new[] {run};

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.ItemTemplate = itemTemplate;
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildList((object) elementMetadata,
                c => { c.PrintViewSource = new[] {"Item1", "Item2"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.ListItems);
            Assert.AreEqual(2, element.ListItems.Count);
            Assert.AreEqual("Item1",
                ((Run) ((Paragraph) element.ListItems.ElementAt(0).Blocks.ElementAt(0)).Inlines.ElementAt(0)).Text);
            Assert.AreEqual("Item2",
                ((Run) ((Paragraph) element.ListItems.ElementAt(1).Blocks.ElementAt(0)).Inlines.ElementAt(0)).Text);
        }

        [Test]
        public void ShouldApplyStartIndex()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.StartIndex = 5;

            // When
            List element = BuildTestHelper.BuildList(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(5, element.StartIndex);
        }

        [Test]
        public void ShouldApplyMarkerStyle()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.MarkerStyle = "Decimal";

            // When
            List element = BuildTestHelper.BuildList(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(TextMarkerStyle.Decimal, element.MarkerStyle);
        }

        [Test]
        public void ShouldApplyMarkerOffsetSize()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.MarkerOffsetSize = 10;
            elementMetadata.MarkerOffsetSizeUnit = "Px";

            // When
            List element = BuildTestHelper.BuildList(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(10, element.MarkerOffset);
        }
    }
}