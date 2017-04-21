using System.Collections.Generic;

using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintHyperlinkHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildHyperlink()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildHyperlink)}.txt");

            var element = new PrintHyperlink
                          {
                              Reference = "http://google.com/",
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Hyperlink Google" }
                                        }
                          };

            // When
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var result = new TextWriterWrapper();
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(expectedResult, result.GetText());
        }
    }
}