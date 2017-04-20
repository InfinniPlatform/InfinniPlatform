using System.Collections.Generic;

using InfinniPlatform.PrintView.Block;
using InfinniPlatform.PrintView.Defaults;
using InfinniPlatform.PrintView.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintParagraphHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildParagraphWithDefaultSettings()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildParagraphWithDefaultSettings)}.txt");

            var element = new PrintParagraph();

            // When
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var result = new TextWriterWrapper();
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(expectedResult, result.GetText());
        }

        [Test]
        public void ShouldBuildParagraphWithSettings()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildParagraphWithSettings)}.txt");

            var run = new PrintRun
                      {
                          Text = "Проверка текста. ",
                          Font = new PrintFont
                                 {
                                     Family = "Courier New",
                                     Size = 30
                                 },
                          TextDecoration = PrintTextDecoration.Underline,
                          Foreground = PrintViewDefaults.Colors.Blue
                      };

            var element = new PrintParagraph
                          {
                              IndentSize = 15,
                              IndentSizeUnit = PrintSizeUnit.Px,
                              Inlines = new List<PrintInline>
                                        {
                                            run,
                                            run,
                                            run,
                                            run,
                                            run,
                                            run,
                                            run,
                                            run
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