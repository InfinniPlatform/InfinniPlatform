using System;
using System.IO;

using InfinniPlatform.Core.Extensions;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.PrintView;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.FlowDocument.PrintView
{
    /// <summary>
    /// Предоставляет методы для работы с печатными представлениями.
    /// </summary>
    internal sealed class PrintViewApi : IPrintViewApi
    {
        public PrintViewApi(IPrintViewBuilder printViewBuilder,
                            MetadataSettings metadataSettings)
        {
            _printViewBuilder = printViewBuilder;
            _metadataSettings = metadataSettings;
        }

        private readonly MetadataSettings _metadataSettings;
        private readonly IPrintViewBuilder _printViewBuilder;

        public byte[] Build(string documentType, string printViewName, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf)
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

            var printViewPath = Path.Combine(_metadataSettings.ContentDirectory, _metadataSettings.PrintViewsPath, documentType, printViewName, ".json").ToFileSystemPath();

            var bytes = File.ReadAllBytes(printViewPath);

            var printViewMetadata = JsonObjectSerializer.Default.Deserialize<DynamicWrapper>(bytes);

            if (printViewMetadata == null)
            {
                throw new ArgumentException($"Print view '{documentType}/{printViewName}' not found.");
            }

            var printViewData = _printViewBuilder.BuildFile(printViewMetadata, printViewSource, printViewFormat);

            return printViewData;
        }
    }
}