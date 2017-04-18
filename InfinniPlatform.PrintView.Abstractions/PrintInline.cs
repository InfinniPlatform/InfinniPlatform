using System;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Базовый класс строкового элемента.
    /// </summary>
    /// <remarks>
    /// Все строковые элементы выводятся на той же строке.
    /// </remarks>
    [Serializable]
    public abstract class PrintInline : PrintElement
    {
        /// <summary>
        /// Оформление текста.
        /// </summary>
        public PrintTextDecoration? TextDecoration { get; set; }
    }
}