using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Реализует <see cref="IPrintViewWriter" />.
    /// </summary>
    /// <remarks>
    /// Предполагается, что в явном виде этот класс будет создаваться только в редакторе
    /// печатных представлений. В иных случаях будет использоваться IoC контейнер.
    /// </remarks>
    public class PrintViewWriter : IPrintViewWriter
    {
        private readonly Dictionary<PrintViewFileFormat, IPrintDocumentWriter> _formatWriters
            = new Dictionary<PrintViewFileFormat, IPrintDocumentWriter>();


        /// <summary>
        /// Регистрирует реализацию <see cref="IPrintDocumentWriter" />.
        /// </summary>
        public void RegisterWriter(PrintViewFileFormat fileFormat, IPrintDocumentWriter documentWriter)
        {
            _formatWriters.Add(fileFormat, documentWriter);
        }


        /// <summary>
        /// Записывает документ в файл.
        /// </summary>
        /// <param name="stream">Поток для записи в файл.</param>
        /// <param name="document">Документ печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления для записи.</param>
        public async Task Write(Stream stream, PrintDocument document, PrintViewFileFormat fileFormat)
        {
            IPrintDocumentWriter documentConverter;

            if (_formatWriters.TryGetValue(fileFormat, out documentConverter))
            {
                await documentConverter.Write(stream, document);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported, fileFormat));
            }
        }
    }
}