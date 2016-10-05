using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintUnderlineHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildUnderline()
        {
            // Given

            var expectedResult = TestHelper.GetEmbeddedResource($"Writers.Html.Resources.{nameof(ShouldBuildUnderline)}.txt");

            var element = new PrintUnderline
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Underline Text." }
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