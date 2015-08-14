using System.IO;
using System.Text;

using InfinniPlatform.FlowDocument.Converters.Html.Blocks;
using InfinniPlatform.FlowDocument.Converters.Html.Inlines;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    internal sealed class FlowDocumentHtmlConverter : IFlowDocumentConverter
    {
        private const int DefaultBufferSize = 1024;
        private static readonly HtmlBuilderContext Builder = CreateHtmlBuilderContext();

        private static HtmlBuilderContext CreateHtmlBuilderContext()
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

        public void Convert(PrintViewDocument document, Stream documentStream)
        {
            using (var writer = new StreamWriter(documentStream, Encoding.UTF8, DefaultBufferSize, true))
            {
                Builder.Build(document, writer);
            }
        }
    }
}