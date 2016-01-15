namespace InfinniPlatform.Sdk.PrintView
{
    /// <summary>
    /// Предоставляет методы для работы с печатными представлениями.
    /// </summary>
    public interface IPrintViewApi
    {
        /// <summary>
        /// Возвращает печатное представление.
        /// </summary>
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="printViewName">Имя печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="priiViewFormat">Формат файла печатного представления.</param>
        /// <returns></returns>
        byte[] Build(string configuration, string documentType, string printViewName, object printViewSource, PrintViewFileFormat priiViewFormat = PrintViewFileFormat.Pdf);
    }
}