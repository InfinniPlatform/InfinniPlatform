namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Предоставляет методы для создания документа печатного представления на основе шаблона и данных.
    /// </summary>
    public interface IPrintDocumentBuilder
    {
        /// <summary>
        /// Создает документ печатного представления.
        /// </summary>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="documentMap">Соответствие между элементами документа и их шаблонами.</param>
        /// <returns>Документ печатного представления.</returns>
        PrintDocument Build(PrintDocument template, object dataSource, PrintDocumentMap documentMap = null);
    }
}