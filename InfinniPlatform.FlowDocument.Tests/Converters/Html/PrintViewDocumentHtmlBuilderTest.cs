using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Model.Views;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    [Ignore("TODO: Dont work in Mono")]
    public sealed class PrintViewDocumentHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildDocumentWithContents()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintViewDocument();
            var result = new TextWriterWrapper();

            var image0 = new PrintElementImage(_getStream(Resources.ImageRotate0));
            var image90 = new PrintElementImage(_getStream(Resources.ImageRotate90));
            var image180 = new PrintElementImage(_getStream(Resources.ImageRotate180));
            var image270 = new PrintElementImage(_getStream(Resources.ImageRotate270));

            image0.Size = new PrintElementSize {Height = 50, Width = 150};
            image90.Size = new PrintElementSize {Height = 150, Width = 50};
            image180.Size = new PrintElementSize {Height = 50, Width = 150};
            image270.Size = new PrintElementSize {Height = 150, Width = 50};

            var par1 = new PrintElementParagraph();

            par1.Inlines.Add(image0);
            par1.Inlines.Add(image90);
            par1.Inlines.Add(image180);
            par1.Inlines.Add(image270);

            var runNormal = new PrintElementRun {Text = "Normal"};
            var runSubscript = new PrintElementRun
            {
                Text = "Subscript",
                Font = new PrintElementFont {Variant = PrintElementFontVariant.Subscript}
            };
            var runSuperscript = new PrintElementRun
            {
                Text = "Superscript",
                Font = new PrintElementFont {Variant = PrintElementFontVariant.Superscript}
            };

            var par2 = new PrintElementParagraph();

            par2.Inlines.Add(runNormal);
            par2.Inlines.Add(runSubscript);
            par2.Inlines.Add(runSuperscript);

            var run = new PrintElementRun {Text = "White Foreground & Black Background"};

            var par3 = new PrintElementParagraph();

            par3.Foreground = "white";
            par3.Background = "black";

            par3.Inlines.Add(run);

            element.Blocks.Add(par1);
            element.Blocks.Add(par2);
            element.Blocks.Add(new PrintElementPageBreak());
            element.Blocks.Add(par3);

            element.PagePadding = new PrintElementThickness(100);

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildDocumentWithContents, result.GetText());
        }

        private Stream _getStream(Bitmap image)
        {
            var stream = new MemoryStream();

            image.Save(stream, ImageFormat.Png);

            stream.Position = 0;

            return stream;
        }
    }
}