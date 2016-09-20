using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementRunHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildRun()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementRun
            {
                Text = "Здесь много текста",
                Font = new PrintElementFont
                {
                    Family = "Tahoma",
                    Size = 30
                }
            };
            var result = new TextWriterWrapper();

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildRun, result.GetText());
        }
    }
}