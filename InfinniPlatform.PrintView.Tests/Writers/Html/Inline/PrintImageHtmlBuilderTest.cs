using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintImageHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildImage()
        {
            // Given

            var expectedResult = TestHelper.GetEmbeddedResource($"Writers.Html.Resources.{nameof(ShouldBuildImage)}.txt");

            var element = new PrintImage
                          {
                              Data = TestHelper.BitmapToBytes(Resources.Image)
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