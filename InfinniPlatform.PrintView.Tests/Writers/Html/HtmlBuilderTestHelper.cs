using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Model.Views;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Html.Blocks;
using InfinniPlatform.PrintView.Writers.Html.Inlines;

namespace InfinniPlatform.PrintView.Tests.Writers.Html
{
    internal static class HtmlBuilderTestHelper
    {
        public static HtmlBuilderContext CreateHtmlBuilderContext()
        {
            return new HtmlBuilderContext()

                //Block
                .Register<PrintElementLine, PrintElementLineHtmlBuilder>()
                .Register<PrintElementList, PrintElementListHtmlBuilder>()
                .Register<PrintElementPageBreak, PrintElementPageBreakHtmlBuilder>()
                .Register<PrintElementParagraph, PrintElementParagraphHtmlBuilder>()
                .Register<PrintElementSection, PrintElementSectionHtmlBuilder>()
                .Register<PrintElementTable, PrintElementTableHtmlBuilder>()

                //Inline
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