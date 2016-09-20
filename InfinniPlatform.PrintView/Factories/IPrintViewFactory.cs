using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Фабрика для создания документа печатного представления на основе метаданных.
    /// </summary>
    internal interface IPrintViewFactory
    {
        /// <summary>
        /// Создает документ печатного представления на основе метаданных.
        /// </summary>
        /// <param name="printView">Шаблон печатного представления.</param>
        /// <param name="printViewSource">Данные печатного представления.</param>
        /// <param name="elementMetadataMap">Соответствие между элементами печатного представления и метаданными.</param>
        /// <returns>Документ печатного представления.</returns>
        PrintViewDocument Create(object printView, object printViewSource, PrintElementMetadataMap elementMetadataMap = null);
    }
}