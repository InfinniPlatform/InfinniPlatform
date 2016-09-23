using System.IO;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Model.Views;
using InfinniPlatform.PrintView.Writers.Html.Blocks;
using InfinniPlatform.PrintView.Writers.Html.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal class HtmlPrintViewFormatWriter : IPrintViewFormatWriter
    {
        private const int DefaultBufferSize = 1024;
        private static readonly Task CompletedTask = Task.FromResult(true);
        private static readonly HtmlBuilderContext Builder = CreateHtmlBuilderContext();


        private static HtmlBuilderContext CreateHtmlBuilderContext()
        {
            return new HtmlBuilderContext()

            // Block

            .Register<PrintElementLine, PrintElementLineHtmlBuilder>()
            .Register<PrintElementList, PrintElementListHtmlBuilder>()
            .Register<PrintElementPageBreak, PrintElementPageBreakHtmlBuilder>()
            .Register<PrintElementParagraph, PrintElementParagraphHtmlBuilder>()
            .Register<PrintElementSection, PrintElementSectionHtmlBuilder>()
            .Register<PrintElementTable, PrintElementTableHtmlBuilder>()

            // Inline

            .Register<PrintElementBold, PrintElementBoldHtmlBuilder>()
            .Register<PrintElementHyperlink, PrintElementHyperlinkHtmlBuilder>()
            .Register<PrintElementImage, PrintElementImageHtmlBuilder>()
            .Register<PrintElementItalic, PrintElementItalicHtmlBuilder>()
            .Register<PrintElementLineBreak, PrintElementLineBreakHtmlBuilder>()
            .Register<PrintElementRun, PrintElementRunHtmlBuilder>()
            .Register<PrintElementSpan, PrintElementSpanHtmlBuilder>()
            .Register<PrintElementUnderline, PrintElementUnderlineHtmlBuilder>()

            // PrintView

            .Register<PrintViewDocument, PrintViewDocumentHtmlBuilder>();
        }


        public Task Write(Stream stream, PrintViewDocument document)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, DefaultBufferSize, true))
            {
                Builder.Build(document, writer);
            }

            return CompletedTask;
        }
    }
}