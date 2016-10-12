using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Стиль печатного представления.
    /// </summary>
    public class PrintStyle : PrintNamedItem
    {
        /// <summary>
        /// Настройки шрифта элемента.
        /// </summary>
        public PrintFont Font { get; set; }

        /// <summary>
        /// Цвет содержимого элемента.
        /// </summary>
        public string Foreground { get; set; }

        /// <summary>
        /// Цвет фона содержимого элемента.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Регистр символов текста элемента.
        /// </summary>
        public PrintTextCase? TextCase { get; set; }

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

        /// <summary>
        /// Оформление текста элемента.
        /// </summary>
        public PrintTextDecoration? TextDecoration { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintStyle;
        }
    }
}