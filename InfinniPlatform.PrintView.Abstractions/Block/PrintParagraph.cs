using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Block
{
    /// <summary>
    /// Элемент для создания абзаца.
    /// </summary>
    [Serializable]
    public class PrintParagraph : PrintBlock
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Paragraph";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintParagraph()
        {
            Inlines = new List<PrintInline>();
        }


        /// <summary>
        /// Отступ первой строки.
        /// </summary>
        public double? IndentSize { get; set; }

        /// <summary>
        /// Единица измерения отступа первой строки.
        /// </summary>
        public PrintSizeUnit? IndentSizeUnit { get; set; }

        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintInline> Inlines { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintParagraph;
        }
    }
}