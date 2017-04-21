using System.Collections.Generic;

using InfinniPlatform.PrintView.Block;
using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintLineBreakHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildLineBreak()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildLineBreak)}.txt");

            var element = new PrintParagraph
                          {
                              IndentSize = 30,
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Before Line Break." },
                                            new PrintLineBreak(),
                                            new PrintRun { Text = "After Line Break." }
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