using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementSectionHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildSectioWithProperties()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementSection();
            var result = new TextWriterWrapper();

            var run = new PrintElementRun();
            run.Text = "Section & Margin & Padding & Border & Background";

            var par = new PrintElementParagraph();
            par.Inlines.Add(run);
            par.TextAlignment = PrintElementTextAlignment.Center;

            var section = new PrintElementSection();
            section.Blocks.Add(par);
            section.Border = new PrintElementBorder()
            {
                Color = "blue",
                Thickness = new PrintElementThickness(5)
            };
            section.Margin = new PrintElementThickness(20);
            section.Padding = new PrintElementThickness(20);
            section.Background = "yellow";

            element.Blocks.Add(section);
            element.Border = new PrintElementBorder()
            {
                Color = PrintElementColors.Red,
                Thickness = new PrintElementThickness(5)
            };
            element.Margin = new PrintElementThickness(20);
            element.Padding = new PrintElementThickness(20);
            element.Background = PrintElementColors.Green;

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildSectioWithProperties, result.GetText());
        }
    }
}
