using System;

using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementHyperlinkHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildHyperlink()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementHyperlink();
            var result = new TextWriterWrapper();

            element.Reference = new Uri("http://google.com");

            var run = new PrintElementRun {Text = "Hyperlink Google"};

            element.Inlines.Add(run);

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildHyperlink, result.GetText());
        }
    }
}