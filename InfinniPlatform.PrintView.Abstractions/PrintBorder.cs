using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Границы элемента.
    /// </summary>
    [Serializable]
    public class PrintBorder
    {
        /// <summary>
        /// Цвет границ.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Толщина границ.
        /// </summary>
        public PrintThickness Thickness { get; set; }
    }
}