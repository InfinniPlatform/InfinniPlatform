using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html
{
    internal static class HtmlBuilderTestHelper
    {
        public static HtmlBuilderContext CreateHtmlBuilderContext()
        {
            return new HtmlBuilderContext()
                .Register<PrintElementParagraph, PrintElementParagraphHtmlBuilder>()
                // Todo
                ;
        }
    }
}