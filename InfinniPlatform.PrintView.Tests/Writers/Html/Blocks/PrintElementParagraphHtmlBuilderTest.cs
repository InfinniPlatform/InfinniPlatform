using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Model.Font;
using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Blocks
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
            var result = new TextWriterWrapper();

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildParagraphWithDefaultSettings, result.GetText());
        }

        [Test]
        public void ShouldBuildParagraphWithSettings()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementParagraph();
            var result = new TextWriterWrapper();

            var run = new PrintElementRun();
            run.Text = "Проверка текста. ";
            run.Font = new PrintElementFont
            {
                Family = "Courier New",
                Size = 30
            };
            run.TextDecoration = PrintElementTextDecoration.Underline;
            run.Foreground = "Blue";

            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);
            element.Inlines.Add(run);

            element.IndentSize = 15;

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildParagraphWithSettings, result.GetText());
        }
    }
}