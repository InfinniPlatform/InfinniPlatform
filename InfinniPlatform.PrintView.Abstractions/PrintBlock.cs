using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Базовый класс блочного элемента.
    /// </summary>
    /// <remarks>
    /// Все блочные элементы выводятся с новой строки.
    /// </remarks>
    [Serializable]
    public abstract class PrintBlock : PrintElement
    {
        /// <summary>
        /// Границы элемента.
        /// </summary>
        public PrintBorder Border { get; set; }

        /// <summary>
        /// Отступ от края элемента до родительского элемента.
        /// </summary>
        public PrintThickness Margin { get; set; }

        /// <summary>
        /// Отступ от края элемента до содержимого элемента.
        /// </summary>
        public PrintThickness Padding { get; set; }

        /// <summary>
        /// Горизонтальное выравнивание текста элемента.
        /// </summary>
        public PrintTextAlignment? TextAlignment { get; set; }
    }
}