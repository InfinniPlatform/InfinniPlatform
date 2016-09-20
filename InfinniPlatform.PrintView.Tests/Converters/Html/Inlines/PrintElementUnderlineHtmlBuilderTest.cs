using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementUnderlineHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildUnderline()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementUnderline();
            var result = new TextWriterWrapper();

            var run = new PrintElementRun {Text = "Underline Text."};

            //When
            element.Inlines.Add(run);

            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildUnderline, result.GetText());
        }
    }
}