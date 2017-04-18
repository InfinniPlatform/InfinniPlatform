using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintRunHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildRun()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildRun)}.txt");

            var element = new PrintRun
                          {
                              Text = "Здесь много текста",
                              Font = new PrintFont
                                     {
                                         Family = "Tahoma",
                                         Size = 30,
                                         SizeUnit = PrintSizeUnit.Px
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