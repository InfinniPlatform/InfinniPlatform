using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Writers
{
    /// <summary>
    /// Преобразовывает печатное представление в файл определенного формата.
    /// </summary>
    internal interface IPrintViewWriter
    {
        /// <summary>
        /// Осуществляет преобразование документа печатного представления в файл указанного формата.
        /// </summary>
        /// <param name="stream">Поток файла печатного представления.</param>
        /// <param name="document">Документ печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        Task Write(Stream stream, PrintViewDocument document, PrintViewFileFormat fileFormat);
    }
}