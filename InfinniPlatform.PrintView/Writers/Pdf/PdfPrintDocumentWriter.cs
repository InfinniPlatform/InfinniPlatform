using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Writers.Html;

namespace InfinniPlatform.PrintView.Writers.Pdf
{
    /// <summary>
    /// Предоставляет метод для записи <see cref="PrintDocument"/> в PDF-файл.
    /// </summary>
    public class PdfPrintDocumentWriter : IPrintDocumentWriter
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PdfPrintDocumentWriter(PrintViewOptions options, HtmlPrintDocumentWriter htmlWriter)
        {
            _htmlToPdfUtil = new HtmlToPdfUtil(options.WkHtmlToPdfPath, options.WkHtmlToPdfArguments, options.TempDirectory);
            _htmlWriter = htmlWriter;
        }


        private readonly HtmlToPdfUtil _htmlToPdfUtil;
        private readonly HtmlPrintDocumentWriter _htmlWriter;


        /// <summary>
        /// Записывает документ в файл.
        /// </summary>
        /// <param name="stream">Поток для записи в файл.</param>
        /// <param name="document">Документ печатного представления.</param>
        public async Task Write(Stream stream, PrintDocument document)
        {
            var saveSize = document.PageSize;
            var savePadding = document.PagePadding;

            document.PagePadding = default(PrintThickness);
            document.PageSize = null;

            try
            {
                using (var htmlStream = new MemoryStream())
                {
                    await _htmlWriter.Write(htmlStream, document);

                    htmlStream.Position = 0;

                    await _htmlToPdfUtil.Convert(saveSize, savePadding, htmlStream, stream);
                }
            }
            finally
            {
                document.PageSize = saveSize;
                document.PagePadding = savePadding;
            }
        }
    }
}