using System.Text;
using InfinniPlatform.FlowDocument.Model.Blocks;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementParagraphHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildParagraphWithDefaultSettings()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementParagraph();
            var result = new StringBuilder();

            // When
            context.Build(element, result);

            // Then
            Assert.AreEqual("<p style=\"\"></p>", result.ToString());
        }
    }
}