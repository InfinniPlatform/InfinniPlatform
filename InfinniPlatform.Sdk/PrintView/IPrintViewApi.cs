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
        /// <param name="documentType">Имя типа документа.</param>
        /// <param name="printViewName">Имя печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="printViewFormat">Формат файла печатного представления.</param>
        /// <returns></returns>
        byte[] Build(string documentType, string printViewName, object printViewSource, PrintViewFileFormat printViewFormat = PrintViewFileFormat.Pdf);
    }
}