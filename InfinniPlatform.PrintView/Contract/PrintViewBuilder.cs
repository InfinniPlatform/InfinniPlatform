using System;
using System.IO;
using System.Threading.Tasks;

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


        public Task Build(Stream stream, Func<Stream> template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            DynamicWrapper dynamicTemplate;

            using (var templateStream = template())
            {
                dynamicTemplate = _jsonObjectSerializer.Deserialize<DynamicWrapper>(templateStream);
            }

            return Build(stream, dynamicTemplate, dataSource, fileFormat);
        }

        public async Task Build(Stream stream, DynamicWrapper template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            // Формирование документа печатного представления
            var document = _printViewFactory.Create(template, dataSource);

            if (document != null)
            {
                // Сохранение документа печатного представления в указанном формате
                await _printViewWriter.Write(stream, document, fileFormat);
            }
        }
    }
}