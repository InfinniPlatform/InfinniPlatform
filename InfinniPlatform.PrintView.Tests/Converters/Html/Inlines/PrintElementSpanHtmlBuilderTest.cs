using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementSpanHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildSpan()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementSpan();
            var result = new TextWriterWrapper();

            var inline1 = new PrintElementRun {Text = "Inline1. "};
            var inline2 = new PrintElementRun {Text = "Inline2. "};

            //When
            element.Inlines.Add(inline1);
            element.Inlines.Add(inline2);

            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildSpan, result.GetText());
        }
    }
}