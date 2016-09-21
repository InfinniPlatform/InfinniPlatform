using System;
using System.IO;

using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Writers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Построитель печатного представления.
    /// </summary>
    internal class PrintViewBuilder : IPrintViewBuilder
    {
        public PrintViewBuilder(IPrintViewFactory printViewFactory, IPrintViewWriter printViewWriter, IJsonObjectSerializer jsonObjectSerializer)
        {
            _printViewFactory = printViewFactory;
            _printViewWriter = printViewWriter;
            _jsonObjectSerializer = jsonObjectSerializer;
        }


        private readonly IPrintViewFactory _printViewFactory;
        private readonly IPrintViewWriter _printViewWriter;
        private readonly IJsonObjectSerializer _jsonObjectSerializer;


        public byte[] Build(Stream printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf)
        {
            if (printViewTemplate == null)
            {
                throw new ArgumentNullException(nameof(printViewTemplate));
            }

            var dynamicTemplate = _jsonObjectSerializer.Deserialize<DynamicWrapper>(printViewTemplate);

            return Build(dynamicTemplate, printViewSource, printViewFormat);
        }

        public byte[] Build(DynamicWrapper printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf)
        {
            if (printViewTemplate == null)
            {
                throw new ArgumentNullException(nameof(printViewTemplate));
            }

            // Формирование документа печатного представления
            var document = _printViewFactory.Create(printViewTemplate, printViewSource);

            if (document != null)
            {
                // Сохранение документа печатного представления в указанном формате
                using (var documentStream = new MemoryStream())
                {
                    _printViewWriter.Convert(document, documentStream, printViewFormat);

                    documentStream.Flush();

                    return documentStream.ToArray();
                }
            }

            return null;
        }
    }
}