using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintBoldHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildBold()
        {
            // Given

            var expectedResult = TestHelper.GetEmbeddedResource($"Writers.Html.Resources.{nameof(ShouldBuildBold)}.txt");

            var element = new PrintBold
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Bold Text." }
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