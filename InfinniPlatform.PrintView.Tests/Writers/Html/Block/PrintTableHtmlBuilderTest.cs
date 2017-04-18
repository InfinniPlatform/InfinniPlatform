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
    public sealed class PrintTableHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildTable()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildTable)}.txt");

            var element = new PrintTable
                          {
                              Columns = new List<PrintTableColumn>
                                        {
                                            new PrintTableColumn { Size = 100 },
                                            new PrintTableColumn { Size = 200 }
                                        },
                              Rows = new List<PrintTableRow>
                                     {
                                         new PrintTableRow
                                         {
                                             Cells = new List<PrintTableCell>
                                                     {
                                                         new PrintTableCell
                                                         {
                                                             ColumnSpan = 2,
                                                             Border = new PrintBorder
                                                                      {
                                                                          Thickness = new PrintThickness(1, PrintSizeUnit.Px),
                                                                          Color = PrintViewDefaults.Colors.Black
                                                                      },
                                                             Block = new PrintParagraph
                                                                     {
                                                                         Inlines = new List<PrintInline>
                                                                                   {
                                                                                       new PrintRun { Text = "Text11, colspan = 2" }
                                                                                   }
                                                                     }
                                                         }
                                                     }
                                         },
                                         new PrintTableRow
                                         {
                                             Cells = new List<PrintTableCell>
                                                     {
                                                         new PrintTableCell
                                                         {
                                                             Border = new PrintBorder
                                                                      {
                                                                          Thickness = new PrintThickness(1, PrintSizeUnit.Px),
                                                                          Color = PrintViewDefaults.Colors.Black
                                                                      },
                                                             Block = new PrintParagraph
                                                                     {
                                                                         Inlines = new List<PrintInline>
                                                                                   {
                                                                                       new PrintRun { Text = "Text21" }
                                                                                   }
                                                                     }
                                                         },
                                                         new PrintTableCell
                                                         {
                                                             Border = new PrintBorder
                                                                      {
                                                                          Thickness = new PrintThickness(1, PrintSizeUnit.Px),
                                                                          Color = PrintViewDefaults.Colors.Black
                                                                      },
                                                             Block = new PrintParagraph
                                                                     {
                                                                         Inlines = new List<PrintInline>
                                                                                   {
                                                                                       new PrintRun { Text = "Text22" }
                                                                                   }
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