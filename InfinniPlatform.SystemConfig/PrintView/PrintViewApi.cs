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
        public PrintViewApi(IMetadataApi metadataApi, IPrintViewBuilder printViewBuilder)
        {
            _metadataApi = metadataApi;
            _printViewBuilder = printViewBuilder;
        }

        private readonly IMetadataApi _metadataApi;
        private readonly IPrintViewBuilder _printViewBuilder;

        public byte[] Build(string documentType, string printViewName, object printViewSource, PrintViewFileFormat priiViewFormat = PrintViewFileFormat.Pdf)
        {
            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (string.IsNullOrEmpty(printViewName))
            {
                throw new ArgumentNullException(nameof(printViewName));
            }

            //Build view name

            var printViewMetadata = _metadataApi.GetMetadata($"PrintViews.{documentType}.{printViewName}");

            if (printViewMetadata == null)
            {
                throw new ArgumentException($"Print view '{documentType}/{printViewName}' not found.");
            }

            var printViewData = _printViewBuilder.BuildFile(printViewMetadata, printViewSource, priiViewFormat);

            return printViewData;
        }
    }
}