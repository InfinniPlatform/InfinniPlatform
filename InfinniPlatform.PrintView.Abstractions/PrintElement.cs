using System;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Базовый класс элемента.
    /// </summary>
    [Serializable]
    public abstract class PrintElement : PrintNamedItem
    {
        /// <summary>
        /// Наименование стиля.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Настройки шрифта.
        /// </summary>
        public PrintFont Font { get; set; }

        /// <summary>
        /// Цвет содержимого.
        /// </summary>
        public string Foreground { get; set; }

        /// <summary>
        /// Цвет фона содержимого.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Регистр символов текста.
        /// </summary>
        public PrintTextCase? TextCase { get; set; }

        /// <summary>
        /// Видимость элемента.
        /// </summary>
        public PrintVisibility? Visibility { get; set; }

        /// <summary>
        /// Источник данных.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Выражение данных.
        /// </summary>
        public string Expression { get; set; }


        /// <summary>
        /// Возвращает строковое представление элемента.
        /// </summary>
        public override string GetDisplayName()
        { 
            var result = base.GetDisplayName();

            if (!string.IsNullOrEmpty(Source))
            {
                result += ", " + Source;
            }

            return result;
        }
    }
}