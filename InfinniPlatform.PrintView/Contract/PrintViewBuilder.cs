using System;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Contract
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
        public PrintViewBuilder(IPrintViewSerializer printViewSerializer,
                                IPrintDocumentBuilder printDocumentBuilder,
                                IPrintViewWriter printViewWriter)
        {
            _printViewSerializer = printViewSerializer;
            _printDocumentBuilder = printDocumentBuilder;
            _printViewWriter = printViewWriter;
        }


        private readonly IPrintViewSerializer _printViewSerializer;
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

            PrintDocument dynamicTemplate;

            using (var templateStream = template())
            {
                dynamicTemplate = _printViewSerializer.Deserialize(templateStream);
            }

            return Build(stream, dynamicTemplate, dataSource, fileFormat);
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