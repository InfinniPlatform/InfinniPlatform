namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Фабрика для создания элементов на основе шаблона.
    /// </summary>
    internal interface IPrintElementFactory
    {
        /// <summary>
        /// Создает элемент.
        /// </summary>
        /// <param name="context">Контекст элемента.</param>
        /// <param name="template">Шаблон элемента.</param>
        object Create(PrintElementFactoryContext context, object template);
    }
}