using System;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.PrintView;
using InfinniPlatform.Sdk.PrintView;

namespace InfinniPlatform.SystemConfig.PrintView
{
    /// <summary>
    /// Предоставляет методы для работы с печатными представлениями.
    /// </summary>
    internal sealed class PrintViewApi : IPrintViewApi
    {
        public PrintViewApi(IMetadataComponent metadataComponent, IPrintViewBuilder printViewBuilder)
        {
            _metadataComponent = metadataComponent;
            _printViewBuilder = printViewBuilder;
        }

        private readonly IMetadataComponent _metadataComponent;
        private readonly IPrintViewBuilder _printViewBuilder;

        public byte[] Build(string configuration, string documentType, string printViewName, object printViewSource, PrintViewFileFormat priiViewFormat = PrintViewFileFormat.Pdf)
        {
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (string.IsNullOrEmpty(printViewName))
            {
                throw new ArgumentNullException(nameof(printViewName));
            }

            var printViewMetadata = _metadataComponent.GetMetadataItem(configuration, documentType, MetadataType.PrintView, (dynamic i) => i.Name == printViewName);

            if (printViewMetadata == null)
            {
                throw new ArgumentException($"Print view '{configuration}/{documentType}/{printViewName}' not found.");
            }

            var printViewData = _printViewBuilder.BuildFile(printViewMetadata, printViewSource, priiViewFormat);

            return printViewData;
        }
    }
}