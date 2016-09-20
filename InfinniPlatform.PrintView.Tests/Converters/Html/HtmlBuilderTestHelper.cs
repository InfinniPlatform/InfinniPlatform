using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Converters.Html.Blocks;
using InfinniPlatform.FlowDocument.Converters.Html.Inlines;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html
{
    internal static class HtmlBuilderTestHelper
    {
        public static HtmlBuilderContext CreateHtmlBuilderContext()
        {
            return new HtmlBuilderContext()

                //Blocks
                .Register<PrintElementLine, PrintElementLineHtmlBuilder>()
                .Register<PrintElementList, PrintElementListHtmlBuilder>()
                .Register<PrintElementPageBreak, PrintElementPageBreakHtmlBuilder>()
                .Register<PrintElementParagraph, PrintElementParagraphHtmlBuilder>()
                .Register<PrintElementSection, PrintElementSectionHtmlBuilder>()
                .Register<PrintElementTable, PrintElementTableHtmlBuilder>()

                //Inlines
                .Register<PrintElementBold, PrintElementBoldHtmlBuilder>()
                .Register<PrintElementHyperlink, PrintElementHyperlinkHtmlBuilder>()
                .Register<PrintElementImage, PrintElementImageHtmlBuilder>()
                .Register<PrintElementItalic, PrintElementItalicHtmlBuilder>()
                .Register<PrintElementLineBreak, PrintElementLineBreakHtmlBuilder>()
                .Register<PrintElementRun, PrintElementRunHtmlBuilder>()
                .Register<PrintElementSpan, PrintElementSpanHtmlBuilder>()
                .Register<PrintElementUnderline, PrintElementUnderlineHtmlBuilder>()
                .Register<PrintViewDocument, PrintViewDocumentHtmlBuilder>()
                ;
        }
    }
}