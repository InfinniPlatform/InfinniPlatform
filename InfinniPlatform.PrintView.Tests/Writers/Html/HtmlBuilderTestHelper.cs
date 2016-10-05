using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Html.Block;
using InfinniPlatform.PrintView.Writers.Html.Inline;

namespace InfinniPlatform.PrintView.Tests.Writers.Html
{
    internal static class HtmlBuilderTestHelper
    {
        public static HtmlBuilderContext CreateHtmlBuilderContext()
        {
            var context = new HtmlBuilderContext();

            context.Register(new PrintDocumentHtmlBuilder());

            // Block
            context.Register(new PrintLineHtmlBuilder());
            context.Register(new PrintListHtmlBuilder());
            context.Register(new PrintPageBreakHtmlBuilder());
            context.Register(new PrintParagraphHtmlBuilder());
            context.Register(new PrintSectionHtmlBuilder());
            context.Register(new PrintTableHtmlBuilder());

            // Inline
            context.Register(new PrintBoldHtmlBuilder());
            context.Register(new PrintHyperlinkHtmlBuilder());
            context.Register(new PrintImageHtmlBuilder());
            context.Register(new PrintItalicHtmlBuilder());
            context.Register(new PrintLineBreakHtmlBuilder());
            context.Register(new PrintRunHtmlBuilder());
            context.Register(new PrintSpanHtmlBuilder());
            context.Register(new PrintUnderlineHtmlBuilder());

            return context;
        }
    }
}