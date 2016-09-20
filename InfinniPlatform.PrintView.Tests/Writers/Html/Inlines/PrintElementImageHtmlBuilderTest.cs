using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inlines
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