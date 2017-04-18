using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Defaults;
using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintSectionHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildSectioWithProperties()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildSectioWithProperties)}.txt");

            var element = new PrintSection
                          {
                              Border = new PrintBorder { Color = PrintViewDefaults.Colors.Red, Thickness = new PrintThickness(5, PrintSizeUnit.Px) },
                              Margin = new PrintThickness(20, PrintSizeUnit.Px),
                              Padding = new PrintThickness(20, PrintSizeUnit.Px),
                              Background = PrintViewDefaults.Colors.Green,
                              Blocks = new List<PrintBlock>
                                       {
                                           new PrintSection
                                           {
                                               Border = new PrintBorder { Color = PrintViewDefaults.Colors.Blue, Thickness = new PrintThickness(5, PrintSizeUnit.Px) },
                                               Margin = new PrintThickness(20, PrintSizeUnit.Px),
                                               Padding = new PrintThickness(20, PrintSizeUnit.Px),
                                               Background = PrintViewDefaults.Colors.Yellow,
                                               Blocks = new List<PrintBlock>
                                                        {
                                                            new PrintParagraph
                                                            {
                                                                TextAlignment = PrintTextAlignment.Center,
                                                                Inlines = new List<PrintInline>
                                                                          {
                                                                              new PrintRun { Text = "Section & Margin & Padding & Border & Background" }
                                                                          }
                                                            }
                                                        }
                                           }
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