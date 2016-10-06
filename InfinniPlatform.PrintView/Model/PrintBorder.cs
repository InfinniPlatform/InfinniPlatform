namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Границы элемента.
    /// </summary>
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