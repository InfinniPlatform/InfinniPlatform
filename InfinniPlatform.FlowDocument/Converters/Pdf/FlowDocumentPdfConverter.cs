using System;
using System.IO;

using InfinniPlatform.Api.Settings;
using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters.Pdf
{
    sealed class FlowDocumentPdfConverter : IFlowDocumentConverter
    {
        private static readonly HtmlToPdfUtil HtmlToPdfUtil;
        private static readonly FlowDocumentHtmlConverter HtmlConverter;


        static FlowDocumentPdfConverter()
        {
            var htmlToPdfUtil = AppSettings.GetValue("HtmlToPdfUtil");
            var htmlToPdfTemp = AppSettings.GetValue("HtmlToPdfTemp");

            if (string.IsNullOrWhiteSpace(htmlToPdfUtil))
            {
                htmlToPdfUtil = HtmlToPdfUtil.GetDefaultHtmlToPdfUtil();
            }

            if (string.IsNullOrWhiteSpace(htmlToPdfTemp))
            {
                htmlToPdfTemp = Path.GetTempPath();
            }

            HtmlToPdfUtil = new HtmlToPdfUtil(htmlToPdfUtil, htmlToPdfTemp);
            HtmlConverter = new FlowDocumentHtmlConverter();
        }


        public void Convert(PrintViewDocument document, Stream documentStream)
        {
            var saveSize = document.PageSize;
            var savePadding = document.PagePadding;

            document.PagePadding = default(PrintElementThickness);
            document.PageSize = null;

            try
            {
                using (var htmlStream = new MemoryStream())
                {
                    HtmlConverter.Convert(document, htmlStream);
                    htmlStream.Position = 0;

                    HtmlToPdfUtil.Convert(saveSize, savePadding, htmlStream, documentStream);
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