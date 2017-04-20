using System;
using System.IO;
using System.Threading.Tasks;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Предоставляет методы для создания файла печатного представления на основе шаблона и данных.
    /// </summary>
    public interface IPrintViewBuilder
    {
        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="stream">Поток для записи печатного представления.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        Task Build(Stream stream, Func<Stream> template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf);

        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="stream">Поток для записи печатного представления.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        Task Build(Stream stream, PrintDocument template, object dataSource = null, PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf);
    }
}