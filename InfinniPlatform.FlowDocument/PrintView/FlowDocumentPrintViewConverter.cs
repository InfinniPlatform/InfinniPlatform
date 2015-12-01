using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.FlowDocument.Converters;
using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Converters.Pdf;
using InfinniPlatform.FlowDocument.Model.Views;
using InfinniPlatform.FlowDocument.Properties;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.FlowDocument.PrintView
{
    public sealed class FlowDocumentPrintViewConverter : IFlowDocumentPrintViewConverter
    {
        public FlowDocumentPrintViewConverter(PrintViewSettings settings)
        {
            _documentConverters = new Dictionary<PrintViewFileFormat, IFlowDocumentConverter>();
            _documentConverters.Add(PrintViewFileFormat.Pdf, new FlowDocumentPdfConverter(settings));
            _documentConverters.Add(PrintViewFileFormat.Html, new FlowDocumentHtmlConverter());
        }


        private readonly Dictionary<PrintViewFileFormat, IFlowDocumentConverter> _documentConverters;


        public bool CanConvert(PrintViewFileFormat printViewFileFormat)
        {
            return _documentConverters.ContainsKey(printViewFileFormat);
        }

        public void Convert(PrintViewDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat)
        {
            IFlowDocumentConverter documentConverter;

            if (_documentConverters.TryGetValue(printViewFileFormat, out documentConverter))
            {
                documentConverter.Convert(printView, printViewStream);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported, printViewFileFormat));
            }
        }
    }
}