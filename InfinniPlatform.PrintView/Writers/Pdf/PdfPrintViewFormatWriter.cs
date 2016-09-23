﻿using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Views;
using InfinniPlatform.PrintView.Writers.Html;

namespace InfinniPlatform.PrintView.Writers.Pdf
{
    internal class PdfPrintViewFormatWriter : IPrintViewFormatWriter
    {
        public PdfPrintViewFormatWriter(PrintViewSettings settings)
        {
            _htmlToPdfUtil = new HtmlToPdfUtil(settings.HtmlToPdfUtilCommand, settings.HtmlToPdfUtilArguments, settings.HtmlToPdfTemp);
            _htmlWriter = new HtmlPrintViewFormatWriter();
        }


        private readonly HtmlPrintViewFormatWriter _htmlWriter;
        private readonly HtmlToPdfUtil _htmlToPdfUtil;


        public async Task Write(Stream stream, PrintViewDocument document)
        {
            var saveSize = document.PageSize;
            var savePadding = document.PagePadding;

            document.PagePadding = default(PrintElementThickness);
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