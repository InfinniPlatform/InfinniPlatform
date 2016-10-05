using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Элемент для создания группы строковых элементов.
    /// </summary>
    public class PrintSpan : PrintInline
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Span";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintSpan()
        {
            Inlines = new List<PrintInline>();
        }


        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintInline> Inlines { get; set; }
    }
}