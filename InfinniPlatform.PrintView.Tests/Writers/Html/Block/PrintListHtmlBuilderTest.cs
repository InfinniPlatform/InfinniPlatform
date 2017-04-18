using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintListHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildList()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildList)}.txt");

            var element = new PrintList
                          {
                              StartIndex = 24,
                              MarkerStyle = PrintListMarkerStyle.LowerLatin,
                              Items = new List<PrintBlock>
                                      {
                                          new PrintSection
                                          {
                                              Blocks = new List<PrintBlock>
                                                       {
                                                           new PrintParagraph
                                                           {
                                                               Inlines = new List<PrintInline> { new PrintRun { Text = "Item1" } }
                                                           }
                                                       }
                                          },
                                          new PrintSection
                                          {
                                              Blocks = new List<PrintBlock>
                                                       {
                                                           new PrintParagraph
                                                           {
                                                               Inlines = new List<PrintInline> { new PrintRun { Text = "Item2" } }
                                                           }
                                                       }
                                          },
                                          new PrintSection
                                          {
                                              Blocks = new List<PrintBlock>
                                                       {
                                                           new PrintParagraph
                                                           {
                                                               Inlines = new List<PrintInline> { new PrintRun { Text = "Item3" } }
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