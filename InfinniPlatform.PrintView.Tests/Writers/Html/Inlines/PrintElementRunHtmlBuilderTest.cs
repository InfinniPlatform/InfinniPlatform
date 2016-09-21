using InfinniPlatform.PrintView.Model.Font;
using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inlines
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