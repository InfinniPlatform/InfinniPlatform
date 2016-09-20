using System.IO;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Предоставляет методы для создания печатных представлений.
    /// </summary>
    public interface IPrintViewBuilder
    {
        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="printViewTemplate">Шаблон печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="printViewFormat">Формат печатного представления.</param>
        /// <returns>Файл печатного представления.</returns>
        byte[] Build(Stream printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf);

        /// <summary>
        /// Создает файл печатного представления.
        /// </summary>
        /// <param name="printViewTemplate">Шаблон печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="printViewFormat">Формат печатного представления.</param>
        /// <returns>Файл печатного представления.</returns>
        byte[] Build(DynamicWrapper printViewTemplate, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf);
    }
}