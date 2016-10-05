using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintDocumentHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildDocumentWithContents()
        {
            // Given

            var expectedResult = TestHelper.GetEmbeddedResource($"Writers.Html.Resources.{nameof(ShouldBuildDocumentWithContents)}.txt");

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
                                                                 Data = TestHelper.BitmapToBytes(Resources.ImageRotate0),
                                                                 Size = new PrintSize(150, 50, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = TestHelper.BitmapToBytes(Resources.ImageRotate90),
                                                                 Size = new PrintSize(50, 150, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = TestHelper.BitmapToBytes(Resources.ImageRotate180),
                                                                 Size = new PrintSize(150, 50, PrintSizeUnit.Px)
                                                             },
                                                             new PrintImage
                                                             {
                                                                 Data = TestHelper.BitmapToBytes(Resources.ImageRotate270),
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