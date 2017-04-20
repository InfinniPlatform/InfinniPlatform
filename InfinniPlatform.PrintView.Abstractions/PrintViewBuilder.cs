using System;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Abstractions.Defaults;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Построитель печатного представления.
    /// </summary>
    /// <remarks>
    /// Предполагается, что в явном виде этот класс будет создаваться только в редакторе
    /// печатных представлений. В иных случаях будет использоваться IoC контейнер.
    /// </remarks>
    public class PrintViewBuilder : IPrintViewBuilder
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintViewBuilder(IJsonObjectSerializer printViewSerializer,
                                IPrintDocumentBuilder printDocumentBuilder,
                                IPrintViewWriter printViewWriter)
        {
            _printViewSerializer = printViewSerializer;
            _printDocumentBuilder = printDocumentBuilder;
            _printViewWriter = printViewWriter;
        }


        private readonly IJsonObjectSerializer _printViewSerializer;
        private readonly IPrintDocumentBuilder _printDocumentBuilder;
        private readonly IPrintViewWriter _printViewWriter;


        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="stream">Поток для записи печатного представления.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        public Task Build(Stream stream, Func<Stream> template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            PrintDocument documentTemplate;

            using (var templateStream = template())
            {
                documentTemplate = _printViewSerializer.Deserialize<PrintDocument>(templateStream);
            }

            return Build(stream, documentTemplate, dataSource, fileFormat);
        }

        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="stream">Поток для записи печатного представления.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        public async Task Build(Stream stream, PrintDocument template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (string.IsNullOrWhiteSpace(template.Source) && string.IsNullOrWhiteSpace(template.Expression))
            {
                template.Source = PrintViewDefaults.RootSource;
            }

            // Формирование документа печатного представления
            var document = _printDocumentBuilder.Build(template, dataSource);

            if (document != null)
            {
                // Сохранение документа печатного представления в указанном формате
                await _printViewWriter.Write(stream, document, fileFormat);
            }
        }
    }
}