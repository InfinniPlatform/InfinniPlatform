using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementImageHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildImage()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementImage(_getStream(Resources.BarcodeQrRotate0));
            var result = new TextWriterWrapper();

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildImage, result.GetText());
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