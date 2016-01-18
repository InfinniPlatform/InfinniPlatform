using InfinniPlatform.Sdk.PrintView;

namespace InfinniPlatform.Core.PrintView
{
    /// <summary>
    /// Построитель печатного представления.
    /// </summary>
    public interface IPrintViewBuilder
    {
        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="printView">Шаблон печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="printViewFileFormat">Формат файла печатного представления.</param>
        /// <returns>Файл печатного представления.</returns>
        byte[] BuildFile(object printView, object printViewSource, PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf);
    }
}