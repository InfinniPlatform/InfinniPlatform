namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Базовый класс фабрики для создания элементов на основе шаблона.
    /// </summary>
    /// <typeparam name="TElement">Тип шаблона элемента печатного представления.</typeparam>
    internal abstract class PrintElementFactoryBase<TElement> : IPrintElementFactory
    {
        /// <summary>
        /// Создает элемент.
        /// </summary>
        /// <param name="context">Контекст элемента.</param>
        /// <param name="template">Шаблон элемента.</param>
        public object Create(PrintElementFactoryContext context, object template)
        {
            return Create(context, (TElement)template);
        }

        /// <summary>
        /// Создает элемент.
        /// </summary>
        /// <param name="context">Контекст элемента.</param>
        /// <param name="template">Шаблон элемента.</param>
        public abstract object Create(PrintElementFactoryContext context, TElement template);
    }
}