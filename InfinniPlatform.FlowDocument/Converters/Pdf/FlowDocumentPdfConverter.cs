using System.IO;

using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters.Pdf
{
    internal sealed class FlowDocumentPdfConverter : IFlowDocumentConverter
    {
        public FlowDocumentPdfConverter(PrintViewSettings settings)
        {
            _htmlToPdfUtil = new HtmlToPdfUtil(settings.HtmlToPdfUtilCommand, settings.HtmlToPdfUtilArguments, settings.HtmlToPdfTemp);
            _htmlConverter = new FlowDocumentHtmlConverter();
        }


        private readonly FlowDocumentHtmlConverter _htmlConverter;
        private readonly HtmlToPdfUtil _htmlToPdfUtil;


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
                    _htmlConverter.Convert(document, htmlStream);

                    htmlStream.Position = 0;

                    _htmlToPdfUtil.Convert(saveSize, savePadding, htmlStream, documentStream);
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