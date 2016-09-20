using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementLineBreakHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildLineBreak()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementParagraph {IndentSize = 30};
            var result = new TextWriterWrapper();

            var run1 = new PrintElementRun {Text = "Before Line Break."};
            var run2 = new PrintElementRun {Text = "After Line Break."};

            element.Inlines.Add(run1);
            element.Inlines.Add(new PrintElementLineBreak());
            element.Inlines.Add(run2);

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildLineBreak, result.GetText());
        }
    }
}