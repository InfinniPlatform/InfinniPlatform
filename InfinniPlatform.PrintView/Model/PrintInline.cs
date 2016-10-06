namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Базовый класс строкового элемента.
    /// </summary>
    /// <remarks>
    /// Все строковые элементы выводятся на той же строке.
    /// </remarks>
    public abstract class PrintInline : PrintElement
    {
        /// <summary>
        /// Оформление текста.
        /// </summary>
        public PrintTextDecoration? TextDecoration { get; set; }
    }
}