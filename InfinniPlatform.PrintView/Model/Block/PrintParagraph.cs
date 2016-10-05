using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания абзаца.
    /// </summary>
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
    }
}