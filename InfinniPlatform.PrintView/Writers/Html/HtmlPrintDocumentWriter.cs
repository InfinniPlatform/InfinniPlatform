using System.IO;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Writers.Html.Block;
using InfinniPlatform.PrintView.Writers.Html.Inline;

namespace InfinniPlatform.PrintView.Writers.Html
{
    /// <summary>
    /// Предоставляет метод для записи <see cref="PrintDocument"/> в HTML-файл.
    /// </summary>
    public class HtmlPrintDocumentWriter : IPrintDocumentWriter
    {
        private const int DefaultBufferSize = 1024;
        private static readonly Task CompletedTask = Task.FromResult(true);
        private static readonly HtmlBuilderContext Builder = CreateHtmlBuilderContext();


        private static HtmlBuilderContext CreateHtmlBuilderContext()
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


        /// <summary>
        /// Записывает документ в файл.
        /// </summary>
        /// <param name="stream">Поток для записи в файл.</param>
        /// <param name="document">Документ печатного представления.</param>
        public Task Write(Stream stream, PrintDocument document)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, DefaultBufferSize, true))
            {
                Builder.Build(document, writer);
            }

            return CompletedTask;
        }
    }
}