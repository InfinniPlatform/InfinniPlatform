using System;
using System.Collections.Generic;
using System.IO;

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


        public void Convert(PrintViewDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat)
        {
            IPrintViewFormatWriter documentConverter;

            if (_formatWriters.TryGetValue(printViewFileFormat, out documentConverter))
            {
                documentConverter.Write(printViewStream, printView);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported, printViewFileFormat));
            }
        }
    }
}