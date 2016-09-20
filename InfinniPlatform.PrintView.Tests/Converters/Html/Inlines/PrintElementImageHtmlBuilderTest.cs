using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementImageHtmlBuilderTest
    {
        [Test]
		[Ignore("Вероятно проблема с механизмом сжатия png.")]
        public void ShouldBuildImage()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementImage(Resources.BarcodeQrRotate0);
            var result = new TextWriterWrapper();

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildImage, result.GetText());
        }
    }
}