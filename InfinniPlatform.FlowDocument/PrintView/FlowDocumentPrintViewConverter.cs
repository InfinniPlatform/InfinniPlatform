using System;
using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Api.PrintView;
using InfinniPlatform.FlowDocument.Converters;
using InfinniPlatform.FlowDocument.Properties;
using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.FlowDocument.PrintView
{
    public sealed class FlowDocumentPrintViewConverter : IFlowDocumentPrintViewConverter
    {
        private static readonly Dictionary<PrintViewFileFormat, IFlowDocumentConverter> DocumentConverters;

        static FlowDocumentPrintViewConverter()
        {
            DocumentConverters = new Dictionary<PrintViewFileFormat, IFlowDocumentConverter>();
            DocumentConverters.Add(PrintViewFileFormat.Pdf, new FlowDocumentPdfConverter());
            DocumentConverters.Add(PrintViewFileFormat.Xps, new FlowDocumentXpsConverter());
            DocumentConverters.Add(PrintViewFileFormat.Rtf, new FlowDocumentRtfConverter());
            DocumentConverters.Add(PrintViewFileFormat.Xml, new FlowDocumentXmlConverter());
        }

        public void Convert(FrameworkFlowDocument printView, Stream printViewStream,
            PrintViewFileFormat printViewFileFormat)
        {
            IFlowDocumentConverter documentConverter;

            if (DocumentConverters.TryGetValue(printViewFileFormat, out documentConverter))
            {
                documentConverter.Convert(printView, printViewStream);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported,
                    printViewFileFormat));
            }
        }

        public bool CanConvert(PrintViewFileFormat printViewFileFormat)
        {
            return DocumentConverters.ContainsKey(printViewFileFormat);
        }
    }
}