using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementBoldHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildBold()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementBold();
            var result = new TextWriterWrapper();

            var run = new PrintElementRun {Text = "Bold Text."};

            //When
            element.Inlines.Add(run);

            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildBold, result.GetText());
        }
    }
}