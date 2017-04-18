using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Документ печатного представления.
    /// </summary>
    [Serializable]
    public class PrintDocument : PrintElement
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintDocument()
        {
            Styles = new List<PrintStyle>();
            Blocks = new List<PrintBlock>();
        }


        /// <summary>
        /// Размеры страницы.
        /// </summary>
        public PrintSize PageSize { get; set; }

        /// <summary>
        /// Отступ от края страницы до содержимого страницы.
        /// </summary>
        public PrintThickness PagePadding { get; set; }

        /// <summary>
        /// Список стилей.
        /// </summary>
        public List<PrintStyle> Styles { get; set; }

        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintBlock> Blocks { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintDocument;
        }
    }
}