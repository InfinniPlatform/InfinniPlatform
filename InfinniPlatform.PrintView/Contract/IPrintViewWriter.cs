using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Предоставляет метод для записи <see cref="PrintDocument"/> в файл.
    /// </summary>
    public interface IPrintViewWriter
    {
        /// <summary>
        /// Осуществляет преобразование документа печатного представления в файл указанного формата.
        /// </summary>
        /// <param name="stream">Поток файла печатного представления.</param>
        /// <param name="document">Документ печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        Task Write(Stream stream, PrintDocument document, PrintViewFileFormat fileFormat);
    }
}