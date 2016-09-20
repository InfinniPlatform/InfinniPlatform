using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementListHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildList()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementList();
            var result = new TextWriterWrapper();

            var item1 = new PrintElementRun {Text = "Item1"};
            var item2 = new PrintElementRun {Text = "Item2"};
            var item3 = new PrintElementRun {Text = "Item3"};

            var par1 = new PrintElementParagraph();
            var par2 = new PrintElementParagraph();
            var par3 = new PrintElementParagraph();

            par1.Inlines.Add(item1);
            par2.Inlines.Add(item2);
            par3.Inlines.Add(item3);

            var section1 = new PrintElementSection();
            var section2 = new PrintElementSection();
            var section3 = new PrintElementSection();

            section1.Blocks.Add(par1);
            section2.Blocks.Add(par2);
            section3.Blocks.Add(par3);

            element.Items.Add(section1);
            element.Items.Add(section2);
            element.Items.Add(section3);

            element.MarkerStyle = PrintElementListMarkerStyle.LowerLatin;
            element.StartIndex = 24;

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildList, result.GetText());
        }
    }
}