using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Views;
using InfinniPlatform.PrintView.Properties;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Pdf;

namespace InfinniPlatform.PrintView.Writers
{
    internal class PrintViewWriter : IPrintViewWriter
    {
        public PrintViewWriter(PrintViewSettings settings)
        {
            _formatWriters = new Dictionary<PrintViewFileFormat, IPrintViewFormatWriter>
                             {
                                 { PrintViewFileFormat.Pdf, new PdfPrintViewFormatWriter(settings) },
                                 { PrintViewFileFormat.Html, new HtmlPrintViewFormatWriter() }
                             };
        }


        private readonly Dictionary<PrintViewFileFormat, IPrintViewFormatWriter> _formatWriters;


        public async Task Write(Stream stream, PrintViewDocument document, PrintViewFileFormat fileFormat)
        {
            IPrintViewFormatWriter documentConverter;

            if (_formatWriters.TryGetValue(fileFormat, out documentConverter))
            {
                await documentConverter.Write(stream, document);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported, fileFormat));
            }
        }
    }
}