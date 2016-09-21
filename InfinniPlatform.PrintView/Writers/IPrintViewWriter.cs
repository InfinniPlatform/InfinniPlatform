using System.IO;

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
        /// <param name="printView">Документ печатного представления.</param>
        /// <param name="printViewStream">Поток файла печатного представления.</param>
        /// <param name="printViewFileFormat">Формат файла печатного представления.</param>
        void Convert(PrintViewDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat);
    }
}