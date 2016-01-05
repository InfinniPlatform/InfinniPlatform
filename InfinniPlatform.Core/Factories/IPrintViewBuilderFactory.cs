using InfinniPlatform.Core.PrintView;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания построителя печатного представления.
    /// </summary>
    public interface IPrintViewBuilderFactory
    {
        /// <summary>
        ///     Создает построитель печатного представления.
        /// </summary>
        IPrintViewBuilder CreatePrintViewBuilder();
    }
}