using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintDocumentHtmlBuilderTest
    {
        [Test]
        [Ignore("Because on Linux this code makes correct but a bit different output")]
        public void ShouldBuildDocumentWithContents()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildDocumentWithContents)}.txt");

            var element = new PrintDocument
                          {
                              PagePadding = new PrintThickness(100, PrintSizeUnit.Px),
                              Blocks = new List<PrintBlock>
                                       {
                                           new PrintParagraph
                                           {
                                               Inlines = new List<PrintInline>
                                                         {
                                                             new PrintImage
                                                             {
                                                                 Data = ResourceHelper.ImageRotate0.Data,
                                                                 Size = new PrintSize(150, 50, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = ResourceHelper.ImageRotate90.Data,
                                                                 Size = new PrintSize(50, 150, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = ResourceHelper.ImageRotate180.Data,
                                                                 Size = new PrintSize(150, 50, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = ResourceHelper.ImageRotate270.Data,
                                                                 Size = new PrintSize(50, 150, PrintSizeUnit.Px)
                                                             }
                                                         }
                                           },
                                           new PrintParagraph
                                           {
                                               Inlines = new List<PrintInline>
                                                         {
                                                             new PrintRun
                                                             {
                                                                 Text = "Normal"
                                                             },
                                                             new PrintRun
                                                             {
                                                                 Text = "Subscript",
                                                                 Font = new PrintFont { Variant = PrintFontVariant.Subscript }
                                                             },
                                                             new PrintRun
                                                             {
                                                                 Text = "Superscript",
                                                                 Font = new PrintFont { Variant = PrintFontVariant.Superscript }
                                                             }
                                                         }
                                           },
                                           new PrintPageBreak(),
                                           new PrintParagraph
                                           {
                                               Foreground = PrintViewDefaults.Colors.White,
                                               Background = PrintViewDefaults.Colors.Black,
                                               Inlines = new List<PrintInline>
                                                         {
                                                             new PrintRun
                                                             {
                                                                 Text = "White Foreground & Black Background"
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