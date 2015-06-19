using InfinniPlatform.Api.PrintView;

namespace InfinniPlatform.Factories
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